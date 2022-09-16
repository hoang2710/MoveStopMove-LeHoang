using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int DataVersion;

    public WeaponType WeaponTag;
    public WeaponSkinType WeaponSkinTag;
    public PantSkinType PantSkinTag;
    public HatType HatTag;
    public ShieldType ShieldTag;
    public string PlayerName;
    public float PlayerExp;
    public Level CurrentLevel;
    public int HighestRank;
    public int Coin;
    public bool IsSoundOn;
    public bool IsVibrateOn;
    public SerializableDictionary<WeaponType, bool> WeaponUnlockState;
    public SerializableDictionary<WeaponSkinType, bool> WeaponSkinUnlockState;
    public SerializableDictionary<PantSkinType, bool> PantSkinUnlockState;
    public SerializableDictionary<HatType, bool> HatUnlockState;
    public SerializableDictionary<ShieldType, bool> ShieldUnlockState;
    public SerializableCustomColorDictionary<WeaponType, List<CustomColor>> CustomColorDict;

    public GameData()
    {
        DataVersion = 10010;

        WeaponTag = WeaponType.Axe;
        WeaponSkinTag = WeaponSkinType.Axe_0;
        PantSkinTag = PantSkinType.Invisible;
        HatTag = HatType.None;
        ShieldTag = ShieldType.None;
        PlayerName = "Anon";
        PlayerExp = 0f;
        CurrentLevel = Level.Level_1;
        HighestRank = 150;
        Coin = 70000;
        IsSoundOn = true;
        IsVibrateOn = true;

        InitDictionaryData<WeaponType>(out WeaponUnlockState);
        InitDictionaryData<WeaponSkinType>(out WeaponSkinUnlockState);
        InitDictionaryData<PantSkinType>(out PantSkinUnlockState);
        InitDictionaryData<HatType>(out HatUnlockState);
        InitDictionaryData<ShieldType>(out ShieldUnlockState);

        //NOTE: unlock some default item
        WeaponUnlockState[WeaponType.Axe] = true;
        WeaponSkinUnlockState[WeaponSkinType.Axe_0] = true;

        InitCustomColorListData();
    }

    private void InitDictionaryData<T>(out SerializableDictionary<T, bool> dict) where T : System.Enum
    {
        dict = new SerializableDictionary<T, bool>();
        foreach (var item in System.Enum.GetValues(typeof(T)))
        {
            dict.Add((T)item, false);
        }
    }
    private void InitCustomColorListData()
    {
        CustomColorDict = new SerializableCustomColorDictionary<WeaponType, List<CustomColor>>();
        foreach (WeaponType item in System.Enum.GetValues(typeof(WeaponType)))
        {
            List<CustomColor> temp = new List<CustomColor>();
            CustomColorDict.Add(item, temp);
        }
    }
}
