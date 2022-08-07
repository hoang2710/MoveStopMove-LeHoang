using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStorage : Singleton<ItemStorage>
{
    [System.Serializable]
    public class WeaponTypeData
    {
        public WeaponType WeaponTag;
        public GameObject WeaponPrefab;
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

    private Dictionary<WeaponType, GameObject> weaponItems = new Dictionary<WeaponType, GameObject>();
    private Dictionary<WeaponSkinType, Material> weaponSkins = new Dictionary<WeaponSkinType, Material>();
    private Dictionary<PantSkinType, Material> pantSkins = new Dictionary<PantSkinType, Material>();

    private void Start()
    {
        DataToDictionary();
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