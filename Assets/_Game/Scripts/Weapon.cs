using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPooledWeapon
{
    public Renderer WeaponRenderer;

    [SerializeField]
    private float flyingSpeed = 8f;

    public WeaponType WeaponTag;
    public BulletType BulletTag;
    public Transform WeaponTrans;
    public GameObject WeaponObject;
    private Vector3 flyDir;
    private float lifeTime = ConstValues.VALUE_WEAPON_DEFAULT_LIFE_TIME;
    private float timer = 0;
    private CharacterBase bulletOwner;

    private void Update()
    {
        Move();
        CheckLifeTime();
    }
    private void OnTriggerEnter(Collider other)
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
    public void SetFlyDir(Vector3 dir)
    {
        flyDir = dir;
    }
    public void SetLifeTime(float value)
    {
        lifeTime = value;
    }
    public void DeactiveWeaponScript()
    {
        Destroy(this);
    }
    public void SetBulletOwner(CharacterBase owner)
    {
        bulletOwner = owner;
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
        // bulletOwner = null;
    }
}

public enum BulletType
{
    Default,
    Rotate,
    TripleShot
}
