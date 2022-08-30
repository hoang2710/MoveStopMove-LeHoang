using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMono<DataManager>
{
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public void DeleteData(string dataKey)
    {
        PlayerPrefs.DeleteKey(dataKey);
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public void SaveData(string dataKey, int value)
    {
        PlayerPrefs.SetInt(dataKey, value);
        OnSaveData(dataKey);
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public void SaveData(string dataKey, float value)
    {
        PlayerPrefs.SetFloat(dataKey, value);
        OnSaveData(dataKey);
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public void SaveData(string dataKey, string value)
    {
        PlayerPrefs.SetString(dataKey, value);
        OnSaveData(dataKey);
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public bool LoadData(string dataKey, out int data)
    {
        if (PlayerPrefs.HasKey(dataKey))
        {
            data = PlayerPrefs.GetInt(dataKey);
            return true;
        }

        data = 0;
        return false;
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public bool LoadData(string dataKey, out float data)
    {
        if (PlayerPrefs.HasKey(dataKey))
        {
            data = PlayerPrefs.GetFloat(dataKey);
            return true;
        }

        data = 0f;
        return false;
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public bool LoadData(string dataKey, out string data)
    {
        if (PlayerPrefs.HasKey(dataKey))
        {
            data = PlayerPrefs.GetString(dataKey);
            return true;
        }

        data = "";
        return false;
    }

    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public void SaveToggleSetting(string dataKey, bool value)
    {
        PlayerPrefs.SetInt(dataKey, BoolToInt(value));
    }
    /// <param name="dataKey"> use string value in DataKeys Script </param>
    public bool LoadToggleSetting(string dataKey)
    {
        return IntToBool(PlayerPrefs.GetInt(dataKey));
    }
    private int BoolToInt(bool value)
    {
        if (value)
            return 1;
        else
            return 0;
    }
    private bool IntToBool(int value)
    {
        if (value == 1)
            return true;
        else
            return false;
    }

    private void OnSaveData(string dataKey)
    {
        //NOTE: do smt
    }

    public void SavePlayerInGameData(PlayerInGameData playerInGameData)
    {
        PlayerPrefs.SetInt(DataKeys.PLAYER_WEAPON_TYPE_ENUM, (int)playerInGameData.WeaponTag);
        PlayerPrefs.SetInt(DataKeys.PLAYER_WEAPON_SKIN_ENUM, (int)playerInGameData.WeaponSkinTag);
        PlayerPrefs.SetInt(DataKeys.PLAYER_PANT_SKIN_ENUM, (int)playerInGameData.PantSkinTag);
    }
    public PlayerInGameData LoadPlayerInGameData()
    {
        WeaponType WeaponTag = (WeaponType)PlayerPrefs.GetInt(DataKeys.PLAYER_WEAPON_TYPE_ENUM);
        WeaponSkinType WeaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(DataKeys.PLAYER_WEAPON_SKIN_ENUM);
        PantSkinType PantSkinTag = (PantSkinType)PlayerPrefs.GetInt(DataKeys.PLAYER_PANT_SKIN_ENUM);

        PlayerInGameData playerInGameData = new PlayerInGameData()
        {
            WeaponTag = WeaponTag,
            WeaponSkinTag = WeaponSkinTag,
            PantSkinTag = PantSkinTag
        };

        return playerInGameData;
    }
}

public class PlayerInGameData
{
    public WeaponType WeaponTag;
    public WeaponSkinType WeaponSkinTag;
    public PantSkinType PantSkinTag;
}