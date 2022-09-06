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
        int index = (int)WeaponTag - 1;

        if (index >= 0 && DataManager.Instance.WeaponUnlockState[(WeaponType)index])
        {
            NoteText.text = note_2;
            return true;
        }

        NoteText.text = note_1;
        return false;
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

