using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanel : MonoBehaviour
{
    public GameObject PanelObj;
    public WeaponType WeaponTag;
    public TMP_Text WeaponNameText;
    public Renderer WeaponDisplay;
    public GameObject NoteTextObj;
    public TMP_Text NoteText;
    private string note_1 = "Unlock previous weapon to buy";
    private string note_2 = "Cheap weapon buy buy";
    public GameObject ItemButtonGroup; //NOTE: use for set active all item button when weapon is bought or not
    public List<ButtonData> ItemButtons; //NOTE: assign button in right order pls
    public Transform ItemFrame;
    public ButtonData currentButtonData; //NOTE: assign prefer button to auto chose when first time enter the panel
    public Renderer CustomWeaponRenderer;
    public List<CustomColor> CustomColors;

    private bool isFirstLoad = true;

    public void SetActive(bool isActive)
    {
        PanelObj.SetActive(isActive);
    }
    public void SetUpPanel()
    {
        if (isFirstLoad)
        {
            for (int i = 1; i < ItemButtons.Count; i++) //NOTE: first button is custom item --> always free
            {
                if (DataManager.Instance.WeaponSkinUnlockState[ItemButtons[i].WeaponSkinTag])
                {
                    ItemButtons[i].LockIcon.SetActive(false);
                    ItemButtons[i].IsUnlock = true;
                }
            }

            ItemFrame.position = currentButtonData.RectTrans.position;

            SetupDefaultColorList();

            isFirstLoad = false;
        }

        SetupDefaultCustomWeaponColor();
        WeaponUnlockStateHandle();
    }
    public void WeaponUnlockStateHandle()
    {
        if (DataManager.Instance.WeaponUnlockState[WeaponTag])
        {
            ItemButtonGroup.SetActive(true);
            NoteTextObj.SetActive(false);
        }
        else
        {
            ItemButtonGroup.SetActive(false);
            NoteTextObj.SetActive(true);
            CheckPreviousWeaponUnlockState();
        }
    }
    public bool CheckPreviousWeaponUnlockState()
    {
        if (WeaponTag != WeaponType.Axe)
        {
            int index = (int)WeaponTag - 1;

            if (index >= 0 && DataManager.Instance.WeaponUnlockState[(WeaponType)index])
            {
                NoteText.text = note_2;
                return true;
            }

            NoteText.text = note_1;
            return false;
        }

        return true;
    }

    public void SetItemFrame(ButtonData buttonData)
    {
        ItemFrame.position = buttonData.RectTrans.position;
    }
    public void SetCurrentButtonData(ButtonData buttonData)
    {
        currentButtonData = buttonData;
    }
    public void SetWeaponDisplay(ButtonData buttonData)
    {
        if (buttonData.WeaponSkinTag != WeaponSkinType.Custom)
        {
            Material weaponSkinMaterial = ItemStorage.Instance.GetWeaponSkin(buttonData.WeaponSkinTag);
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    WeaponDisplay.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial, weaponSkinMaterial }; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    WeaponDisplay.materials = new Material[] { weaponSkinMaterial, weaponSkinMaterial };
                    break;
            }
        }
        else
        {
            List<CustomColor> colorList = DataManager.Instance.CustomColorDict[WeaponTag];
            switch (WeaponTag)
            {
                case WeaponType.Candy:
                    WeaponDisplay.materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1], colorList[2]); ; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    WeaponDisplay.materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1]);
                    break;
            }
        }
    }
    public void BuyWeaponHandle()
    {
        DataManager.Instance.Coin -= currentButtonData.ItemCost;
        DataManager.Instance.WeaponSkinUnlockState[currentButtonData.WeaponSkinTag] = true;

        currentButtonData.IsUnlock = true;
        currentButtonData.LockIcon.SetActive(false);
    }
    public void SetupDefaultColorList()
    {
        if (DataManager.Instance.CustomColorDict[WeaponTag].Count == 0)
        {
            foreach (CustomColor item in CustomColors)
            {
                DataManager.Instance.CustomColorDict[WeaponTag].Add(item);
            }
        }
    }
    public void SetupDefaultCustomWeaponColor()
    {
        List<CustomColor> colorList = DataManager.Instance.CustomColorDict[WeaponTag];
        Material[] materials;
        switch (WeaponTag)
        {
            case WeaponType.Candy:
                materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1], colorList[2]); ; //NOTE: Candy weapon have 3 material
                CustomWeaponRenderer.materials = materials;
                break;
            default:
                materials = ItemStorage.Instance.GetCustomMaterials(colorList[0], colorList[1]);
                CustomWeaponRenderer.materials = materials;
                break;
        }
    }
}

