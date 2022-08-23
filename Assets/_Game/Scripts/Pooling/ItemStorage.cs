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

    [NonReorderable]
    public List<WeaponTypeData> WeaponTypeDatas;
    [NonReorderable]
    public List<WeaponSkinData> WeaponSkinDatas;
    [NonReorderable]
    public List<PantData> PantDatas;
    public List<Material> BotMaterials;

    private Dictionary<WeaponType, GameObject> weaponItems = new Dictionary<WeaponType, GameObject>();
    private Dictionary<WeaponSkinType, Material> weaponSkins = new Dictionary<WeaponSkinType, Material>();
    private Dictionary<PantSkinType, Material> pantSkins = new Dictionary<PantSkinType, Material>();

    //Pool of weapon
    private Dictionary<WeaponType, Stack<GameObject>> weaponPool = new Dictionary<WeaponType, Stack<GameObject>>();

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

        Debug.Log(weaponItems.Count + "  " + weaponSkins.Count + "   " + pantSkins.Count);
    }
    private void InitPool()
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

            weaponPool.Add(item.WeaponTag, tmpStack); Debug.Log("Pool " + item.WeaponTag + "  " + tmpStack.Count);
        }
    }
    public GameObject PopWeaponFromPool(WeaponType weaponTag, WeaponSkinType skinTag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;

        obj.SetActive(true);
        objTrans.position = position;
        objTrans.rotation = rotation;

        IPooledWeapon weapon = obj.GetComponent<IPooledWeapon>();
        weapon?.OnPopFromPool(weaponSkins[skinTag]);

        return obj;
    }
    public GameObject PopWeaponFromPool(WeaponType weaponTag, WeaponSkinType skinTag, Transform parentTrans, Vector3 localPosition, Quaternion localRotation)
    {
        GameObject obj = CheckIfHaveWeaponLeftInPool(weaponTag);
        Transform objTrans = obj.transform;

        obj.SetActive(true);
        objTrans.SetParent(parentTrans);
        objTrans.localPosition = localPosition;
        objTrans.localRotation = localRotation;

        IPooledWeapon weapon = obj.GetComponent<IPooledWeapon>();
        weapon?.OnPopFromPool(weaponSkins[skinTag]);

        return obj;
    }
    public void PushWeaponToPool(WeaponType weaponTag, GameObject obj, bool isHandWeapon = false)
    {
        if (isHandWeapon)
        {
            obj.transform.SetParent(null); //NOTE: single use no cache
        }

        weaponPool[weaponTag].Push(obj);

        IPooledWeapon weapon = obj.GetComponent<IPooledWeapon>();
        weapon?.OnPushToPool();

        obj.SetActive(false);
    }
    private GameObject CheckIfHaveWeaponLeftInPool(WeaponType tag)
    {
        if (weaponPool[tag].Count > 0)
        {
            GameObject obj = weaponPool[tag].Peek();
            weaponPool[tag].Pop();
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(weaponItems[tag]);
            return obj;
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
    Vantim
}