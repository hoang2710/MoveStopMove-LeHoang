using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    public ButtonType ButtonType;

    [HideInInspector] public WeaponType WeaponTag;
    [HideInInspector] public WeaponSkinType WeaponSkinTag;
    [HideInInspector] public PantSkinType PantSkinTag;
    [HideInInspector] public HatType HatTag;
    [HideInInspector] public int ItemCost;
    [HideInInspector] public bool IsUnlock;
    [HideInInspector] public PanelType PanelTag;
    [HideInInspector] public Image ButtonImage;
    [HideInInspector] public Image IconImage;
    [HideInInspector] public GameObject LockIcon;
    [HideInInspector] public RectTransform RectTrans;
}

#if UNITY_EDITOR
public enum ButtonType
{
    WeaponItem,
    PantItem,
    HatItem,
    SkinShopCategory,
    SkinShopItem
}

[CustomEditor(typeof(ButtonData))]
public class ButtonDataGUI : Editor
{
    SerializedProperty WeaponTag;
    SerializedProperty WeaponSkinTag;
    SerializedProperty PantSkinTag;
    SerializedProperty HatTag;
    SerializedProperty ItemCost;
    SerializedProperty IsUnlock;
    SerializedProperty PanelTag;
    SerializedProperty ButtonImage;
    SerializedProperty IconImage;
    SerializedProperty LockIcon;
    SerializedProperty RectTrans;

    private void OnEnable()
    {
        WeaponTag = serializedObject.FindProperty("WeaponTag");
        WeaponSkinTag = serializedObject.FindProperty("WeaponSkinTag");
        PantSkinTag = serializedObject.FindProperty("PantSkinTag");
        HatTag = serializedObject.FindProperty("HatTag");
        ItemCost = serializedObject.FindProperty("ItemCost");
        IsUnlock = serializedObject.FindProperty("IsUnlock");
        PanelTag = serializedObject.FindProperty("PanelTag");
        ButtonImage = serializedObject.FindProperty("ButtonImage");
        IconImage = serializedObject.FindProperty("IconImage");
        LockIcon = serializedObject.FindProperty("LockIcon");
        RectTrans = serializedObject.FindProperty("RectTrans");
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonData buttonData = (ButtonData)target;

        switch (buttonData.ButtonType)
        {
            case ButtonType.WeaponItem:
                DisplayWeaponButton();
                break;
            case ButtonType.PantItem:
                DisPlayPantButton();
                break;
            case ButtonType.HatItem:
                DisplayHatButton();
                break;
            case ButtonType.SkinShopCategory:
                DisplaySkinShopCategory();
                break;
            case ButtonType.SkinShopItem:
                DisplaySkinShopItem();
                break;
            default:
                break;
        }
    }

    private void DisplayWeaponButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(WeaponTag, new GUIContent("Weapon Tag"));
        EditorGUILayout.PropertyField(WeaponSkinTag, new GUIContent("Weapon Skin Tag"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisPlayPantButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(PantSkinTag, new GUIContent("Pant Skin Tag"));
        EditorGUILayout.PropertyField(ItemCost, new GUIContent("Item Cost"));
        EditorGUILayout.PropertyField(LockIcon, new GUIContent("Lock Icon"));
        EditorGUILayout.PropertyField(RectTrans, new GUIContent("RectTransform"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplayHatButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(HatTag, new GUIContent("Hat Tag"));
        EditorGUILayout.PropertyField(ItemCost, new GUIContent("Item Cost"));
        EditorGUILayout.PropertyField(LockIcon, new GUIContent("Lock Icon"));
        EditorGUILayout.PropertyField(RectTrans, new GUIContent("RectTransform"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplaySkinShopCategory()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(PanelTag, new GUIContent("Panel Tag"));
        EditorGUILayout.PropertyField(ButtonImage, new GUIContent("Button Image"));
        EditorGUILayout.PropertyField(IconImage, new GUIContent("Icon Image"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplaySkinShopItem()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(IsUnlock, new GUIContent("Is Item Unlocked"));

        serializedObject.ApplyModifiedProperties();
    }
}

#endif