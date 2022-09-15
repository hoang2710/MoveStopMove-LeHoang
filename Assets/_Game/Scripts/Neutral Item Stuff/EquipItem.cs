using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    public GameObject Object;
    public Transform Trans;

    public Vector3 PositionOffSet;
    public Vector3 RotationOffset; //NOTE: use for setting quartenion
    protected Quaternion rotationOffset;

    protected virtual void Awake()
    {
        rotationOffset = Quaternion.Euler(RotationOffset.x, RotationOffset.y, RotationOffset.z);
    }
}
