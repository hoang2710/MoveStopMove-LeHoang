using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Custom Color Data", menuName = "DataSO/Custom Color Data")]
public class CustomColorDataSO : ScriptableObject
{
    public List<ItemStorage.ColorData> ColorDatas;
}
