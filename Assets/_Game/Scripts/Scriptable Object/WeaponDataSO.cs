using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "DataSO/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    public List<ItemStorage.WeaponTypeData> WeaponDatas;
    public List<ItemStorage.WeaponSkinData> WeaponSkinDatas;
}
