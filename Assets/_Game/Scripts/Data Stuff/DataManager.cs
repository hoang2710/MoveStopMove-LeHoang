using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMono<DataManager>
{
    public int DataVersion { get; set; }

    [SerializeField]
    private string fileName;
    private DataConvert dataConvert;
    private GameData gameData;
    private List<IDataHandler> dataHandlers = new List<IDataHandler>();

    //NOTE: Global Game Data
    public int Coin { get; set; }
    public float PlayerExp { get; set; }
    public Dictionary<WeaponType, bool> WeaponUnlockState;
    public Dictionary<WeaponSkinType, bool> WeaponSkinUnlockState;
    public Dictionary<PantSkinType, bool> PantSkinUnlockState;
    public Dictionary<HatType, bool> HatUnlockState;
    public Dictionary<ShieldType, bool> ShieldUnlockState;
    public Dictionary<SkinSet, bool> SkinSetUnlockState;
    public List<HatType> UnlockOneTimeHat;
    public List<PantSkinType> UnlockOneTimePantSkin;
    public List<ShieldType> UnlockOneTimeShield;
    public List<SkinSet> UnlockOneTimeSkinSet;
    public Dictionary<WeaponType, List<CustomColor>> CustomColorDict;
    public int HighestRank { get; set; }

    private void Start()
    {
        dataConvert = new DataConvert(Application.persistentDataPath, fileName);
        DataVersion = 10011;
    }
    public void AssignDataHandler(IDataHandler dataHandler)
    {
        dataHandlers.Add(dataHandler);
    }
    public GameData GetGameData()
    {
        return gameData;
    }

    public void NewGame()
    {
        gameData = new GameData();
    }
    public void LoadGame()
    {
        gameData = dataConvert.Load();

        if (gameData == null || gameData.DataVersion != DataVersion)
        {
            NewGame();
        }

        foreach (IDataHandler dataHandler in dataHandlers)
        {
            dataHandler.LoadData(gameData);
        }

        LoadGlobalData(gameData);
    }
    public void SaveGame()
    {
        foreach (IDataHandler dataHandler in dataHandlers)
        {
            dataHandler.SaveData(gameData);
        }

        SaveGlobalData(gameData);

        dataConvert.Save(gameData);
    }
    private void OnApplicationPause(bool isPause) // NOTE: for android
    {
        if (isPause)
        {
            SaveGame();
        }
    }
    private void OnApplicationQuit() // NOTE: for window
    {
        SaveGame();
    }

    private void LoadGlobalData(GameData gameData)
    {
        Coin = gameData.Coin;
        PlayerExp = gameData.PlayerExp;
        HighestRank = gameData.HighestRank;

        WeaponUnlockState = gameData.WeaponUnlockState;
        WeaponSkinUnlockState = gameData.WeaponSkinUnlockState;
        PantSkinUnlockState = gameData.PantSkinUnlockState;
        HatUnlockState = gameData.HatUnlockState;
        ShieldUnlockState = gameData.ShieldUnlockState;
        SkinSetUnlockState = gameData.SkinSetUnlockState;
        CustomColorDict = gameData.CustomColorDict;

        UnlockOneTimeHat = gameData.UnlockOneTimeHat;
        UnlockOneTimePantSkin = gameData.UnlockOneTimePantSkin;
        UnlockOneTimeShield = gameData.UnlockOneTimeShield;
        UnlockOneTimeSkinSet = gameData.UnlockOneTimeSkinSet;
    }
    private void SaveGlobalData(GameData gameData)
    {
        gameData.Coin = Coin;
        gameData.PlayerExp = PlayerExp;
        gameData.HighestRank = HighestRank;
    }

    public void DeleteData()
    {
        dataConvert.DeleteData();
    }
}
