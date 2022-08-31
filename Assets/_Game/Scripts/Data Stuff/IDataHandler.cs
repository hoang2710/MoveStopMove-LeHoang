using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataHandler
{
    /// <summary>
    /// Must assign IDataHandler through DataManager.Instance.AssignDataHandler to work
    /// </summary>
    public void LoadData(GameData data);
    /// <summary>
    /// Must assign IDataHandler through DataManager.Instance.AssignDataHandler to work
    /// </summary>
    public void SaveData(GameData data);
}
