using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Obstacle Data", menuName = "DataSO/Obstacle Data")]
public class ObstacleDataSO : ScriptableObject
{
    public List<Material> ObstacleMaterials;
}
