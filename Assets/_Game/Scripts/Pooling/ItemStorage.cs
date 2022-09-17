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
    [System.Serializable]
    public class ShieldData
    {
        public ShieldType ShieldTag;
        public GameObject ShieldPrefabs;
        public int poolSize;
    }
    [System.Serializable]
    public class ColorData
    {
        public CustomColor CustomColor;
        public Material Color;
    }

    public WeaponDataSO WeaponDataSO;
    public PantDataSO PantDataSO;
    public HatDataSO HatDataSO;
    public ShieldDataSO ShieldDataSO;
    public CustomColorDataSO CustomColorDataSO;
    public BotDataSO BotDataSO;
    public ObstacleDataSO ObstacleDataSO;

    private List<WeaponTypeData> weaponTypeDatas;
    private List<WeaponSkinData> weaponSkinDatas;
    private List<PantData> pantDatas;
    private List<HatData> hatDatas;
    private List<ShieldData> shieldDatas;
    private List<ColorData> colorDatas;
    private List<Material> botMaterials;
    private List<string> botNames;
    private List<Material> obstacleMaterial; //NOTE: assign trans material on second element 

    private Dictionary<WeaponType, GameObject> weaponItems = new Dictionary<WeaponType, GameObject>();
    private Dictionary<WeaponSkinType, Material> weaponSkins = new Dictionary<WeaponSkinType, Material>();
    private Dictionary<PantSkinType, Material> pantSkins = new Dictionary<PantSkinType, Material>();
    private Dictionary<HatType, GameObject> hatItems = new Dictionary<HatType, GameObject>();
    private Dictionary<ShieldType, GameObject> shieldItems = new Dictionary<ShieldType, GameObject>();
    private Dictionary<CustomColor, Material> customColors = new Dictionary<CustomColor, Material>();

    //Pool of weapon
    private Dictionary<WeaponType, Stack<GameObject>> weaponPool = new Dictionary<WeaponType, Stack<GameObject>>();
    //Pool of Hat
    private Dictionary<HatType, Stack<GameObject>> hatPool = new Dictionary<HatType, Stack<GameObject>>();
    //Pool of Shield
    private Dictionary<ShieldType, Stack<GameObject>> shieldPool = new Dictionary<ShieldType, Stack<GameObject>>();

    private void Start()
    {
        ConvertSO();
        DataToDictionary();
        StartCoroutine(InitPool());
    }
    private void ConvertSO()
    {
        weaponTypeDatas = WeaponDataSO.WeaponDatas;
        weaponSkinDatas = WeaponDataSO.WeaponSkinDatas;
        pantDatas = PantDataSO.PantDatas;
        hatDatas = HatDataSO.HatDatas;
        shieldDatas = ShieldDataSO.ShieldDatas;
        colorDatas = CustomColorDataSO.ColorDatas;
        botMaterials = BotDataSO.BotMaterials;
        botNames = BotDataSO.BotNames;
        obstacleMaterial = ObstacleDataSO.ObstacleMaterials;
    }
    private void DataToDictionary()
    {
        foreach (var item in weaponTypeDatas)
        {
            weaponItems.Add(item.WeaponTag, item.WeaponPrefab);
        }
        foreach (var item in weaponSkinDatas)
        {
            weaponSkins.Add(item.WeaponSkinTag, item.WeaponSkinMaterial);
        }
        foreach (var item in pantDatas)
        {
            pantSkins.Add(item.PantSkinTag, item.PantMaterial);
        }
        foreach (var item in hatDatas)
        {
            hatItems.Add(item.HatTag, item.HatPrefabs);
        }
        foreach (var item in shieldDatas)
        {
            shieldItems.Add(item.ShieldTag, item.ShieldPrefabs);
        }
        foreach (var item in colorDatas)
        {
            customColors.Add(item.CustomColor, item.Color);
        }
    }
    private IEnumerator InitPool() //NOTE: might optimize later or not
    {
        foreach (var item in weaponTypeDatas)
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
        yield return null;

        foreach (var item in hatDatas)
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
        yield return null;

        foreach (var item in shieldDatas)
        {
            Stack<GameObject> tmpStack = new Stack<GameObject>();
            for (int i = 0; i < item.poolSize; i++)
            {
                GameObject tmpObj = Instantiate(item.ShieldPrefabs);
                tmpStack.Push(tmpObj);

                tmpObj.SetActive(false);
            }

            shieldPool.Add(item.ShieldTag, tmpStack);
        }
    }
    public GameObject PopWeaponFromPool<T>(WeaponType weaponTag, WeaponSkinType skinTag, Vector3 position, Quaternion rotation, out T weapon) where T : Weapon
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;
        weapon = CacheWeapon.Get(obj) as T;

        obj.SetActive(true);
        objTrans.position = position;
        objTrans.rotation = rotation;

        IPooledWeapon pooledWeapon = CacheIpooledWeapon.Get(obj);
        pooledWeapon?.OnPopFromPool(skinTag);

        return obj;
    }
    public GameObject PopWeaponFromPool<T>(WeaponType weaponTag, WeaponSkinType skinTag, Transform parentTrans, Vector3 localPosition, Quaternion localRotation, out T weapon) where T : Weapon
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;
        weapon = CacheWeapon.Get(obj) as T;

        obj.SetActive(true);
        objTrans.SetParent(parentTrans);
        objTrans.localPosition = localPosition;
        objTrans.localRotation = localRotation;

        IPooledWeapon pooledWeapon = CacheIpooledWeapon.Get(obj);
        pooledWeapon?.OnPopFromPool(skinTag);

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

    public GameObject PopShieldFromPool(ShieldType shieldTag, Transform parentTrans)
    {
        GameObject obj = CheckIfHaveShieldLeftInPool(shieldTag);

        obj.SetActive(true);

        IPooledShield pooledShield = CacheIPooledShield.Get(obj);
        pooledShield?.OnSpawn(parentTrans);

        return obj;
    }
    public void PushShieldToPool(ShieldType shieldTag, GameObject obj)
    {
        shieldPool[shieldTag].Push(obj);
        obj.SetActive(false);
    }
    private GameObject CheckIfHaveShieldLeftInPool(ShieldType tag)
    {
        if (shieldPool[tag].Count > 0)
        {
            return shieldPool[tag].Pop();
        }
        else
        {
            return Instantiate(shieldItems[tag]);
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
    public Material GetCustomColorMaterial(CustomColor tag)
    {
        return customColors[tag];
    }
    public Color GetCustomColor(CustomColor tag)
    {
        return customColors[tag].color;
    }
    public Material GetRandomBotMaterial()
    {
        int ran = Random.Range(0, botMaterials.Count);
        return botMaterials[ran];
    }
    public string GetRandomBotName()
    {
        int ran = Random.Range(0, botNames.Count);
        return botNames[ran];
    }
    public Material[] GetCustomMaterials(CustomColor color1, CustomColor color2)
    {
        Material[] materials = new Material[]
        {
            GetCustomColorMaterial(color1),
            GetCustomColorMaterial(color2)
        };

        return materials;
    }
    public Material[] GetCustomMaterials(CustomColor color1, CustomColor color2, CustomColor color3) //NOTE: for candy
    {
        Material[] materials = new Material[]
        {
            GetCustomColorMaterial(color1),
            GetCustomColorMaterial(color2),
            GetCustomColorMaterial(color3)
        };

        return materials;
    }
    public Material GetObstacleMaterial(int index)
    {
        return obstacleMaterial[index];
    }
}

public enum WeaponType
{
    Axe,
    Hammer,
    Candy,
    Knife
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
    Knife_2,
    Custom
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
public enum ShieldType
{
    None,
    Star,
    Knight
}
public enum CustomColor
{
    Melon,
    Red,
    Flamingo,
    BubbleGum,
    Orange,
    Tangerine,
    SunShine,
    Banana,
    Green,
    Lime,
    Aqua,
    Tropical,
    Royal,
    Blue,
    SugarPlum,
    Lavender,
    Storm,
    Smoke,
    Brown,
    Nutmeg
}
