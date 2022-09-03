using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : MonoBehaviour, IPooledHat
{
    public GameObject HatObject;
    public Transform HatTrans;

    public Vector3 PositionOffSet;
    public Vector3 RotationOffset; //NOTE: use for setting quartenion
    private Quaternion rotationOffset;

    private void Awake()
    {
        rotationOffset = Quaternion.Euler(RotationOffset.x, RotationOffset.y, RotationOffset.z);
    }
    public void OnSpawn(Transform parentTrans)
    {
        HatTrans.SetParent(parentTrans);
        HatTrans.localPosition = PositionOffSet;
        HatTrans.localRotation = rotationOffset;
    }
}
