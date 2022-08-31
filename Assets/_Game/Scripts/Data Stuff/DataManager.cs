using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMono<DataManager>
{
    [SerializeField]
    private string fileName;
    private DataConvert dataConvert;
    private GameData gameData;
    private List<IDataHandler> dataHandlers = new List<IDataHandler>();

    //NOTE: Global Game Data
    public int Coin { get; set; }
    public float PlayerExp { get; set; }

    private void Start()
    {
        dataConvert = new DataConvert(Application.persistentDataPath, fileName);
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

        if (gameData == null)
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
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void LoadGlobalData(GameData gameData)
    {
        Coin = gameData.Coin;
        PlayerExp = gameData.PlayerExp;
    }
    private void SaveGlobalData(GameData gameData)
    {
        gameData.Coin = Coin;
        gameData.PlayerExp = PlayerExp;
    }

}
