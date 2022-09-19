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
    [HideInInspector] public ShieldType ShieldTag;
    [HideInInspector] public SkinSetDataSO SkinSetDataSO;
    [HideInInspector] public int ItemCost;
    [HideInInspector] public bool IsUnlock;
    [HideInInspector] public PanelType PanelTag;
    [HideInInspector] public Image ButtonImage;
    [HideInInspector] public Image IconImage;
    [HideInInspector] public GameObject LockIcon; //NOTE: default is set active true
    [HideInInspector] public RectTransform RectTrans;
    [HideInInspector] public CustomColor CustomColor;
    [HideInInspector] public RectTransform Trans;
    [HideInInspector] public GameObject Object;
    [HideInInspector] public int CustomPartIndex;
}

public enum ButtonType
{
    WeaponItem,
    PantItem,
    HatItem,
    ShieldItem,
    SkinShopCategory,
    ColorItem,
    ColorPartItem,
    SkinSetItem
}
#if UNITY_EDITOR

[CustomEditor(typeof(ButtonData))]
public class ButtonDataGUI : Editor
{
    SerializedProperty WeaponTag;
    SerializedProperty WeaponSkinTag;
    SerializedProperty PantSkinTag;
    SerializedProperty HatTag;
    SerializedProperty ShieldTag;
    SerializedProperty SkinSetDataSO;
    SerializedProperty ItemCost;
    SerializedProperty IsUnlock;
    SerializedProperty PanelTag;
    SerializedProperty ButtonImage;
    SerializedProperty IconImage;
    SerializedProperty LockIcon;
    SerializedProperty RectTrans;
    SerializedProperty CustomColor;
    SerializedProperty Object;
    SerializedProperty CustomPartIndex;

    private void OnEnable()
    {
        WeaponTag = serializedObject.FindProperty("WeaponTag");
        WeaponSkinTag = serializedObject.FindProperty("WeaponSkinTag");
        PantSkinTag = serializedObject.FindProperty("PantSkinTag");
        HatTag = serializedObject.FindProperty("HatTag");
        ShieldTag = serializedObject.FindProperty("ShieldTag");
        SkinSetDataSO = serializedObject.FindProperty("SkinSetDataSO");
        ItemCost = serializedObject.FindProperty("ItemCost");
        IsUnlock = serializedObject.FindProperty("IsUnlock");
        PanelTag = serializedObject.FindProperty("PanelTag");
        ButtonImage = serializedObject.FindProperty("ButtonImage");
        IconImage = serializedObject.FindProperty("IconImage");
        LockIcon = serializedObject.FindProperty("LockIcon");
        RectTrans = serializedObject.FindProperty("RectTrans");
        CustomColor = serializedObject.FindProperty("CustomColor");
        Object = serializedObject.FindProperty("Object");
        CustomPartIndex = serializedObject.FindProperty("CustomPartIndex");
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
            case ButtonType.ShieldItem:
                DisplayShieldButton();
                break;
            case ButtonType.SkinShopCategory:
                DisplaySkinShopCategory();
                break;
            case ButtonType.ColorItem:
                DisplayColorButton();
                break;
            case ButtonType.ColorPartItem:
                DisplayColorPartButton();
                break;
            case ButtonType.SkinSetItem:
                DisplaySkinSetItemButton();
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
        EditorGUILayout.PropertyField(ItemCost, new GUIContent("Item Cost"));
        EditorGUILayout.PropertyField(LockIcon, new GUIContent("Lock Icon"));
        EditorGUILayout.PropertyField(RectTrans, new GUIContent("RectTransform"));

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
    private void DisplayShieldButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(ShieldTag, new GUIContent("Shield Tag"));
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
    private void DisplayColorButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(CustomColor, new GUIContent("Custom Color"));
        EditorGUILayout.PropertyField(ButtonImage, new GUIContent("Button Image"));
        EditorGUILayout.PropertyField(WeaponSkinTag, new GUIContent("Weapon Skin Tag"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplayColorPartButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(ButtonImage, new GUIContent("Button Image"));
        EditorGUILayout.PropertyField(RectTrans, new GUIContent("RectTransform"));
        EditorGUILayout.PropertyField(Object, new GUIContent("Object"));
        EditorGUILayout.PropertyField(CustomPartIndex, new GUIContent("Part Button Index"));

        serializedObject.ApplyModifiedProperties();
    }
    private void DisplaySkinSetItemButton()
    {
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(SkinSetDataSO, new GUIContent("SkinSet DataSO"));
        EditorGUILayout.PropertyField(ItemCost, new GUIContent("Item Cost"));
        EditorGUILayout.PropertyField(LockIcon, new GUIContent("Lock Icon"));
        EditorGUILayout.PropertyField(RectTrans, new GUIContent("RectTransform"));

        serializedObject.ApplyModifiedProperties();
    }
}

#endif