using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bot Data", menuName = "DataSO/Bot Data")]
public class BotDataSO : ScriptableObject
{
    public List<Material> BotMaterials;
    public List<string> BotNames;
}
