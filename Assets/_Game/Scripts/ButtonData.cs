using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ButtonData : MonoBehaviour
{
    public ButtonType ButtonType;

    [HideInInspector]
    public WeaponType WeaponTag;
    [HideInInspector]
    public WeaponSkinType WeaponSkinTag;
    [HideInInspector]
    public PantSkinType PantSkinTag;
    [HideInInspector]
    public int ItemCost;
}

#if UNITY_EDITOR
public enum ButtonType
{
    WeaponItem,
    PantItem,
    HatItem
}

[CustomEditor(typeof(ButtonData))]
public class ButtonDataGUI : Editor
{
    SerializedProperty WeaponTag;
    SerializedProperty WeaponSkinTag;
    SerializedProperty PantSkinTag;

    private void OnEnable()
    {
        WeaponTag = serializedObject.FindProperty("WeaponTag");
        WeaponSkinTag = serializedObject.FindProperty("WeaponSkinTag");
        PantSkinTag = serializedObject.FindProperty("PantSkinTag");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonData buttonData = (ButtonData)target;

        switch (buttonData.ButtonType)
        {
            case ButtonType.WeaponItem:
                DisplayWeaponButton(buttonData);
                break;
            case ButtonType.PantItem:
                DisPlayPantButton(buttonData);
                break;
            case ButtonType.HatItem:
                DisplayHatItem(buttonData);
                break;
            default:
                break;
        }
    }

    private void DisplayWeaponButton(ButtonData buttonData)
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(WeaponTag, new GUIContent("Weapon Tag"));
        EditorGUILayout.PropertyField(WeaponSkinTag, new GUIContent("Weapon Skin Tag"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisPlayPantButton(ButtonData buttonData)
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(PantSkinTag, new GUIContent("Pant Skin Tag"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplayHatItem(ButtonData buttonData)
    {
        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();
    }
}

#endif