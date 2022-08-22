using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPooledWeapon
{
    public Renderer WeaponRenderer;
    [SerializeField]
    private float flyingSpeed = ConstValues.WALUE_WEAPON_DEFAULT_FLY_SPEED;
    public WeaponType WeaponTag;
    public BulletType BulletTag;
    public Transform WeaponTrans;
    public GameObject WeaponObject;
    public Collider WeaponCollider;
    [SerializeField]
    private Vector3 weaponPositionOffset;
    public Vector3 HandRotateOffset; //NOTE: use this X,Y,Z of Vector to set up Quaternion only
    private Quaternion weaponHandRotationOffset;
    public Vector3 ThrowRotateOffset; //NOTE: use this X,Y,Z of Vector to set up Quaternion only
    private Quaternion weaponThrowRotationOffset;
    private Vector3 flyDir;
    private float lifeTime = ConstValues.VALUE_WEAPON_DEFAULT_LIFE_TIME;
    private float timer = 0;
    private CharacterBase bulletOwner;
    private bool isRotate;
    private Vector3 rotateDir = Vector3.up;
    [SerializeField]
    private float rotateSpeed = 5f;
    private bool isTripleShot;
    [SerializeField]
    private float tripleShotOffset = 30f;//NOTE: y axis in quaternion

    private void Awake()
    {
        weaponHandRotationOffset = Quaternion.Euler(HandRotateOffset.x, HandRotateOffset.y, HandRotateOffset.z);
        weaponThrowRotationOffset = Quaternion.Euler(ThrowRotateOffset.x, ThrowRotateOffset.y, ThrowRotateOffset.z);

        switch (WeaponTag)
        {
            case WeaponType.Axe:
                isRotate = true;
                break;
            case WeaponType.Hammer:
                isRotate = true;
                break;
            case WeaponType.Knife:
                isTripleShot = true;
                break;
            case WeaponType.Candy:
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        Move();
        CheckLifeTime();
    }
    private void OnTriggerEnter(Collider other)
    {
        WeaponHitHandle(other);
    }
    private void WeaponHitHandle(Collider other)
    {
        IHit hit = other.GetComponent<IHit>();
        if (hit != null)
        {
            hit.OnHit(bulletOwner);
            ItemStorage.Instance.PushWeaponToPool(WeaponTag, WeaponObject);
        }
    }
    public void Move()
    {
        WeaponTrans.position = Vector3.MoveTowards(WeaponTrans.position, WeaponTrans.position + flyDir, flyingSpeed * Time.deltaTime);

        if (isRotate)
        {
            WeaponTrans.Rotate(rotateDir * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
    public void CheckLifeTime()
    {
        if (timer <= lifeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ItemStorage.Instance.PushWeaponToPool(WeaponTag, WeaponObject);
        }
    }
    public void SetUpHandWeapon(CharacterBase owner)
    {
        this.enabled = false;
        WeaponCollider.enabled = false;

        WeaponTrans.localPosition = weaponPositionOffset;
        WeaponTrans.localRotation = weaponHandRotationOffset;

        owner?.SetUpThrowWeapon(weaponThrowRotationOffset, isTripleShot, tripleShotOffset);
    }
    public void SetUpThrowWeapon(Vector3 dir, CharacterBase owner)
    {
        SetFlyDir(dir);
        SetBulletOwner(owner);
        CalculateLifeTime();
    }
    public void SetFlyDir(Vector3 dir)
    {
        flyDir = dir;
    }
    public void SetBulletOwner(CharacterBase owner)
    {
        bulletOwner = owner;
    }
    public void CalculateLifeTime()
    {
        lifeTime = bulletOwner.AttackRange / flyingSpeed;
    }
    public void OnPopFromPool(Material weaponSkinMaterial)
    {
        switch (WeaponTag)
        {
            case WeaponType.Candy:
                WeaponRenderer.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial, weaponSkinMaterial }; //NOTE: Candy weapon have 3 material
                break;
            default:
                WeaponRenderer.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial };
                break;
        }

        timer = 0;
    }
    public void OnPushToPool()
    {
        this.enabled = true;
        WeaponCollider.enabled = true;
    }
}

public enum BulletType
{
    Default,
    Rotate,
    TripleShot
}
