using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPooledWeapon
{
    public Renderer WeaponRenderer;
    [SerializeField]
    protected float flyingSpeed = ConstValues.WALUE_WEAPON_DEFAULT_FLY_SPEED;
    public WeaponType WeaponTag;
    public Transform WeaponTrans;
    public GameObject WeaponObject;
    public Collider WeaponCollider;
    [SerializeField]
    protected Vector3 weaponPositionOffset;
    public Vector3 HandRotateOffset; //NOTE: use this X,Y,Z of Vector to set up Quaternion only
    protected Quaternion weaponHandRotationOffset;
    public Vector3 ThrowRotateOffset; //NOTE: use this X,Y,Z of Vector to set up Quaternion only
    protected Quaternion weaponThrowRotationOffset;
    public float DefaultScale;
    protected Vector3 flyDir;
    protected float lifeTime = ConstValues.VALUE_WEAPON_DEFAULT_LIFE_TIME;
    protected float timer = 0;
    protected CharacterBase bulletOwner;
    protected Vector3 rotateDir = Vector3.up;
    [SerializeField] protected float rotateSpeed = -180f;

    protected virtual void Awake()
    {
        weaponHandRotationOffset = Quaternion.Euler(HandRotateOffset.x, HandRotateOffset.y, HandRotateOffset.z);
        weaponThrowRotationOffset = Quaternion.Euler(ThrowRotateOffset.x, ThrowRotateOffset.y, ThrowRotateOffset.z);
    }
    protected virtual void Update()
    {
        Move();
        CheckLifeTime();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        WeaponHitHandle(other);
    }
    protected void WeaponHitHandle(Collider other)
    {
        IHit hit = CacheIHit.Get(other);
        hit?.OnHit(bulletOwner, this);
    }
    public void Move()
    {
        WeaponTrans.position = Vector3.MoveTowards(WeaponTrans.position, WeaponTrans.position + flyDir, flyingSpeed * Time.deltaTime);
    }
    public void CheckLifeTime() //NOTE: aware of double push to pool bug when onHit and lifeTime trigger at once 
    {
        if (timer <= lifeTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ItemStorage.Instance.PushWeaponToPool(this);
        }
    }
    public void SetUpHandWeapon(CharacterBase owner)
    {
        this.enabled = false;
        WeaponCollider.enabled = false;
        WeaponTrans.localScale = DefaultScale * Vector3.one;

        WeaponTrans.localPosition = weaponPositionOffset;
        WeaponTrans.localRotation = weaponHandRotationOffset;

        owner?.SetUpThrowWeapon(weaponThrowRotationOffset);
    }
    public void SetUpThrowWeapon(Vector3 dir, CharacterBase owner)
    {
        SetFlyDir(dir);
        SetBulletOwner(owner);
        CalculateLifeTime();
        SetAdaptiveSize(owner);
        OnThrowHandle();
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
        lifeTime = (bulletOwner.AttackRange - bulletOwner.AttackPosOffset) / flyingSpeed;
    }
    public void SetAdaptiveSize(CharacterBase owner)
    {
        WeaponTrans.localScale = owner.CharaterTrans.localScale * DefaultScale;
        flyingSpeed = ConstValues.WALUE_WEAPON_DEFAULT_FLY_SPEED * owner.CharaterTrans.localScale.x;
    }
    public float GetRemainLifeTime()
    {
        return lifeTime - timer;
    }
    protected virtual void OnThrowHandle() //NOTE: use for set up weapon behaviour for each type of weapon when throw weapon
    {

    }
    public virtual void OnPopFromPool(WeaponSkinType weaponSkinTag)
    {
        if (weaponSkinTag != WeaponSkinType.Custom)
        {
            Material weaponSkinMaterial = ItemStorage.Instance.GetWeaponSkin(weaponSkinTag);
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    WeaponRenderer.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial, weaponSkinMaterial }; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    WeaponRenderer.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial };
                    break;
            }
        }
        else
        {
            List<CustomColor> colorList = DataManager.Instance.CustomColorDict[WeaponTag];
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    WeaponRenderer.materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1], colorList[2]); ; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    WeaponRenderer.materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1]);
                    break;
            }
        }

        timer = 0;
    }
    public virtual void OnPushToPool()
    {
        this.enabled = true;
        WeaponCollider.enabled = true;
    }
}
