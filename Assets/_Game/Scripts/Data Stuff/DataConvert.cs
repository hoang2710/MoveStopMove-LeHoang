using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class DataConvert
{
    private string path;
    public DataConvert(string dataDirPath, string dataFileName)
    {
        path = Path.Combine(dataDirPath, dataFileName);
    }
    public string GetPath()
    {
        return path;
    }

    public GameData Load()
    {
        GameData gameData = null;

        if (File.Exists(path))
        {
            try
            {
                string data = "";
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                gameData = JsonUtility.FromJson<GameData>(data);
            }
            catch (Exception e)
            {
                Debug.LogError("Error Load Data with path: " + path + "\n" + e);
            }
        }

        return gameData;
    }

    public void Save(GameData gameData)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            string data = JsonUtility.ToJson(gameData, true);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(data);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error Save Data with path: " + path + "\n" + e);
        }
    }

    public void DeleteData()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
}
