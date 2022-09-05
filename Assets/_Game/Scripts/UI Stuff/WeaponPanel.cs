using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponPanel : MonoBehaviour
{
    public GameObject PanelObj;
    public WeaponType WewaponTag { get; }
    public TMP_Text WeaponNameText;
    public Renderer WeaponDisplay;
    public GameObject ItemButtonGroup; //NOTE: use for set active all item button when weapon is bought or not
    public List<ButtonData> ItemButtons; //NOTE: assign button in right order pls
    public Transform ItemFrame;
    public ButtonData currentButtonData; //NOTE: assign prefer button to auto chose when first time enter the panel

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

            isFirstLoad = false;
        }
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
        Material material = ItemStorage.Instance.GetWeaponSkin(buttonData.WeaponSkinTag);

        if (buttonData.WeaponTag == WeaponType.Candy)
        {
            WeaponDisplay.materials = new Material[] { material, material, material }; //NOTE: Candy have 3 material, else have 2
        }
        else
        {
            WeaponDisplay.materials = new Material[] { material, material };
        }
    }
    public void BuyWeaponHandle()
    {
        DataManager.Instance.Coin -= currentButtonData.ItemCost;
        DataManager.Instance.WeaponSkinUnlockState[currentButtonData.WeaponSkinTag] = true;

        currentButtonData.IsUnlock = true;
        currentButtonData.LockIcon.SetActive(false);
    }

}

