using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (GameManager)target;

        if (GUILayout.Button("Reset Player Coin"))
        {
            script.ResetPLayerCoinValue();
        }
        if (GUILayout.Button("Reset Exp"))
        {
            script.ResetPlayerEXP();
        }
    }
}
#endif