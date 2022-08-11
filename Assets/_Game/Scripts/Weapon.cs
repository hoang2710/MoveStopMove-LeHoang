using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPooledWeapon
{
    public Renderer WeaponRenderer;

    [SerializeField]
    private float flyingSpeed = 8f;

    public WeaponType WeaponType;
    public BulletType BulletType;
    public Transform WeaponTrans;
    public GameObject WeaponObject;
    private Vector3 flyDir;
    private float lifeTime = ConstValues.VALUE_WEAPON_DEFAULT_LIFE_TIME;
    private float timer = 0;

    private void Update()
    {
        Move();
        CheckLifeTime();
    }
    private void OnTriggerEnter(Collider other)
    {
        IHit hit = other.GetComponent<IHit>();
        hit?.OnHit();
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
            ItemStorage.Instance.PushWeaponToPool(WeaponType, WeaponObject);
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
        this.enabled = false;
    }
    public void OnPopFromPool(Material skinMaterial)
    {
        Material[] materials = new Material[] { skinMaterial, skinMaterial };
        WeaponRenderer.materials = materials;

        timer = 0;
    }
    public void OnPushToPool()
    {

    }
}

public enum BulletType
{
    Default,
    Rotate,
    TripleShot
}
