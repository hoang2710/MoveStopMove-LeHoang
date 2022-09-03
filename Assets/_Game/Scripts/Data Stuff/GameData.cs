using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public WeaponType WeaponTag;
    public WeaponSkinType WeaponSkinTag;
    public PantSkinType PantSkinTag;
    public HatType HatTag;
    public string PlayerName;
    public float PlayerExp;
    public Level CurrentLevel;
    public int Coin;
    public bool IsSoundOn;
    public bool IsVibrateOn;
    public SerializableDictionary<WeaponType, bool> WeaponUnlockState;
    public SerializableDictionary<WeaponSkinType, bool> WeaponSkinUnlockState;
    public SerializableDictionary<PantSkinType, bool> PantSkinUnlockState;
    public SerializableDictionary<HatType, bool> HatUnlockState;

    public GameData()
    {
        WeaponTag = WeaponType.Axe;
        WeaponSkinTag = WeaponSkinType.Axe_0;
        PantSkinTag = PantSkinType.Invisible;
        HatTag = HatType.None;
        PlayerName = "Anon";
        PlayerExp = 0f;
        CurrentLevel = Level.Level_1;
        Coin = 0;
        IsSoundOn = true;
        IsVibrateOn = true;

        InitDictionaryData<WeaponType>(out WeaponUnlockState);
        InitDictionaryData<WeaponSkinType>(out WeaponSkinUnlockState);
        InitDictionaryData<PantSkinType>(out PantSkinUnlockState);
        InitDictionaryData<HatType>(out HatUnlockState);
    }

    private void InitDictionaryData<T>(out SerializableDictionary<T, bool> dict) where T : System.Enum
    {
        if (!typeof(T).IsEnum) throw new System.ArgumentException("T must be an enumerated type");

        dict = new SerializableDictionary<T, bool>();
        foreach (var item in System.Enum.GetValues(typeof(T)))
        {
            dict.Add((T)item, false);
        }
    }
}
