using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : SingletonMono<ItemStorage>
{
    [System.Serializable]
    public class WeaponTypeData
    {
        public WeaponType WeaponTag;
        public GameObject WeaponPrefab;
        public int PoolSize;
    }
    [System.Serializable]
    public class WeaponSkinData
    {
        public WeaponSkinType WeaponSkinTag;
        public Material WeaponSkinMaterial;
    }
    [System.Serializable]
    public class PantData
    {
        public PantSkinType PantSkinTag;
        public Material PantMaterial;
    }
    [System.Serializable]
    public class HatData
    {
        public HatType HatTag;
        public GameObject HatPrefabs;
        public int poolSize;
    }

    [NonReorderable]
    public List<WeaponTypeData> WeaponTypeDatas;
    [NonReorderable]
    public List<WeaponSkinData> WeaponSkinDatas;
    [NonReorderable]
    public List<PantData> PantDatas;
    public List<HatData> HatDatas;
    public List<Material> BotMaterials;
    public List<string> BotNames;

    private Dictionary<WeaponType, GameObject> weaponItems = new Dictionary<WeaponType, GameObject>();
    private Dictionary<WeaponSkinType, Material> weaponSkins = new Dictionary<WeaponSkinType, Material>();
    private Dictionary<PantSkinType, Material> pantSkins = new Dictionary<PantSkinType, Material>();
    private Dictionary<HatType, GameObject> hatItems = new Dictionary<HatType, GameObject>();

    //Pool of weapon
    private Dictionary<WeaponType, Stack<GameObject>> weaponPool = new Dictionary<WeaponType, Stack<GameObject>>();
    //Pool of Hat
    private Dictionary<HatType, Stack<GameObject>> hatPool = new Dictionary<HatType, Stack<GameObject>>();

    private void Start()
    {
        DataToDictionary();
        InitPool();
    }
    private void DataToDictionary()
    {
        foreach (var item in WeaponTypeDatas)
        {
            weaponItems.Add(item.WeaponTag, item.WeaponPrefab);
        }
        foreach (var item in WeaponSkinDatas)
        {
            weaponSkins.Add(item.WeaponSkinTag, item.WeaponSkinMaterial);
        }
        foreach (var item in PantDatas)
        {
            pantSkins.Add(item.PantSkinTag, item.PantMaterial);
        }
        foreach (var item in HatDatas)
        {
            hatItems.Add(item.HatTag, item.HatPrefabs);
        }
    }
    private void InitPool() //NOTE: might optimize later or not
    {
        foreach (var item in WeaponTypeDatas)
        {
            Stack<GameObject> tmpStack = new Stack<GameObject>();
            for (int i = 0; i < item.PoolSize; i++)
            {
                GameObject tmpObj = Instantiate(item.WeaponPrefab);
                tmpStack.Push(tmpObj);

                tmpObj.SetActive(false);
            }

            weaponPool.Add(item.WeaponTag, tmpStack);
        }

        foreach (var item in HatDatas)
        {
            Stack<GameObject> tmpStack = new Stack<GameObject>();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject tmpObj = Instantiate(item.HatPrefabs);
                tmpStack.Push(tmpObj);

                tmpObj.SetActive(false);
            }

            hatPool.Add(item.HatTag, tmpStack);
        }
    }
    public GameObject PopWeaponFromPool(WeaponType weaponTag, WeaponSkinType skinTag, Vector3 position, Quaternion rotation, out Weapon weapon)
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;
        weapon = CacheWeapon.Get(obj);

        obj.SetActive(true);
        objTrans.position = position;
        objTrans.rotation = rotation;

        IPooledWeapon pooledWeapon = CacheIpooledWeapon.Get(obj);
        pooledWeapon?.OnPopFromPool(weaponSkins[skinTag]);

        return obj;
    }
    public GameObject PopWeaponFromPool(WeaponType weaponTag, WeaponSkinType skinTag, Transform parentTrans, Vector3 localPosition, Quaternion localRotation, out Weapon weapon)
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;
        weapon = CacheWeapon.Get(obj);

        obj.SetActive(true);
        objTrans.SetParent(parentTrans);
        objTrans.localPosition = localPosition;
        objTrans.localRotation = localRotation;

        IPooledWeapon pooledWeapon = CacheIpooledWeapon.Get(obj);
        pooledWeapon?.OnPopFromPool(weaponSkins[skinTag]);

        return obj;
    }

    public void PushWeaponToPool(WeaponType weaponTag, GameObject obj)
    {
        weaponPool[weaponTag].Push(obj);

        IPooledWeapon pooledWeapon = CacheIpooledWeapon.Get(obj);
        pooledWeapon?.OnPushToPool();

        obj.SetActive(false);
    }
    /// <summary>
    /// Use to push onHand Weapon to pool
    /// </summary>
    public void PushWeaponToPool(WeaponType weaponTag, GameObject obj, Transform objTrans)
    {
        objTrans.SetParent(null);
        PushWeaponToPool(weaponTag, obj);
    }
    private GameObject CheckIfHaveWeaponLeftInPool(WeaponType tag)
    {
        if (weaponPool[tag].Count > 0)
        {
            return weaponPool[tag].Pop();
        }
        else
        {
            return Instantiate(weaponItems[tag]);
        }
    }
    public GameObject PopHatFromPool(HatType hatTag, Transform parentTrans)
    {
        GameObject obj = CheckIfHaveHatLeftInPool(hatTag);

        obj.SetActive(true);

        IPooledHat pooledHat = CacheIpooledHat.Get(obj);
        pooledHat?.OnSpawn(parentTrans);

        return obj;
    }
    public void PushHatToPool(HatType hatTag, GameObject obj)
    {
        hatPool[hatTag].Push(obj);
        obj.SetActive(false);
    }
    private GameObject CheckIfHaveHatLeftInPool(HatType tag)
    {
        if (hatPool[tag].Count > 0)
        {
            return hatPool[tag].Pop();
        }
        else
        {
            return Instantiate(hatItems[tag]);
        }
    }

    public GameObject GetWeaponType(WeaponType tag)
    {
        return weaponItems[tag];
    }
    public Material GetWeaponSkin(WeaponSkinType tag)
    {
        return weaponSkins[tag];
    }
    public Material GetPantSkin(PantSkinType tag)
    {
        return pantSkins[tag];
    }
    public Material GetRandomBotMaterial()
    {
        int ran = Random.Range(0, BotMaterials.Count);
        return BotMaterials[ran];
    }
    public string GetRandomBotName()
    {
        int ran = Random.Range(0, BotNames.Count);
        return BotNames[ran];
    }
}

public enum WeaponType
{
    Axe,
    Hammer,
    Knife,
    Candy
}
public enum WeaponSkinType
{
    Axe_0,
    Axe_0_2,
    Axe_1,
    Axe_1_2,
    Hammer_1,
    Hammer_2,
    Candy_1,
    Candy_2,
    Knife_1,
    Knife_2
}
public enum PantSkinType
{
    Batman,
    Chambi,
    Comy,
    Dabao,
    Onion,
    Pokemon,
    Rainbow,
    Skull,
    Vantim,
    Invisible
}
public enum HatType
{
    None,
    Arrow,
    Cowboy,
    Crown,
    Ear,
    Hat,
    Cap,
    StrawHat,
    HeadPhone,
    Horn,
    Beard
}
