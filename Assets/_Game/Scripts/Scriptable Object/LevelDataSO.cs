using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level ", menuName = "DataSO/Level Data")]
public class LevelDataSO : ScriptableObject
{
    public int numOfBaseBot;
    public int numOfCharater;
    public int botLevelFloorBound;
    public int botLevelCeilBound;
}
