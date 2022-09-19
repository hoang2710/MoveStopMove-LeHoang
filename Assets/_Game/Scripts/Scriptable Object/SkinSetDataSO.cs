using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin Set ", menuName = "DataSO/Skin Set Data")]
public class SkinSetDataSO : ScriptableObject
{
    public SkinSet SkinSet;

    public Material CharSkinMat;
    public HatType HatTag;
    public ShieldType ShieldTag;
    public PantSkinType PantSkinTag;
    public BackItemType BackItemTag;
    public TailType TailTag;
}
