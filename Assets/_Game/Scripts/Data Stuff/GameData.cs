using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public WeaponType WeaponTag;
    public WeaponSkinType WeaponSkinTag;
    public PantSkinType PantSkinTag;
    public string PlayerName;
    public float PlayerExp;
    public Level CurrentLevel;
    public int Coin;
    public bool IsSoundOn;
    public bool IsVibrateOn;

    public GameData()
    {
        WeaponTag = WeaponType.Axe;
        WeaponSkinTag = WeaponSkinType.Axe_0;
        PantSkinTag = PantSkinType.Onion;
        PlayerName = "Anon";
        PlayerExp = 0f;
        CurrentLevel = Level.Level_1;
        Coin = 0;
        IsSoundOn = true;
        IsVibrateOn = true;
    }
}
