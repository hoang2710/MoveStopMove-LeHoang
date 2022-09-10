using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWeaponShopCanvas : UICanvas
{
    private Player playerRef;

    public TMP_Text CoinDisplay;
    public List<WeaponPanel> WeaponPanelList;
    public WeaponPanel CurrentPanel; //NOTE: assign fisrt panel in list pls
    private int currentPanelIndex = 0; //NOTE: shop start with axe panel as first panel in panel list

    public GameObject BottomButtons;
    public GameObject BuyButton;
    public GameObject EquipButton;
    public GameObject SelectedButton;
    public TMP_Text ItemCostText;

    public List<ButtonData> ColorButtons;
    public GameObject CustomColorGroup;
    public GameObject EquipCustomButton;
    public List<ButtonData> ColorPartButtons; //NOTE: Must assign all 3 button in this
    private ButtonData currentCustomButtonData;
    public RectTransform SelectMark;

    private bool isFirstLoad = true;

    public void OnClickWeaponButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        WeaponButtonClickHandle(buttonData);
    }
    private void WeaponButtonClickHandle(ButtonData buttonData)
    {
        bool isBuyButtonVisible = SetBottomPartState(buttonData.IsUnlock,
                                                     buttonData.WeaponSkinTag == playerRef.WeaponSkinTag);

        if (isBuyButtonVisible)
        {
            SetItemCost(buttonData.ItemCost);
        }

        SetBottomCustomPartState(false);

        CurrentPanel.SetItemFrame(buttonData);
        CurrentPanel.SetCurrentButtonData(buttonData);
        CurrentPanel.SetWeaponDisplay(buttonData);
    }
    private bool SetBottomPartState(bool isBought, bool isEquip) //NOTE: return if is need to display buy button
    {
        if (!isBought)
        {
            BuyButton.SetActive(true);
            EquipButton.SetActive(false);
            SelectedButton.SetActive(false);

            return true;
        }
        else if (!isEquip)
        {
            BuyButton.SetActive(false);
            EquipButton.SetActive(true);
            SelectedButton.SetActive(false);
        }
        else
        {
            BuyButton.SetActive(false);
            EquipButton.SetActive(false);
            SelectedButton.SetActive(true);
        }

        return false;
    }
    private void SetBottomCustomPartState(bool isCustom)
    {
        if (isCustom)
        {
            BottomButtons.SetActive(false);
            CustomColorGroup.SetActive(true);

            switch (CurrentPanel.WeaponTag)
            {
                case WeaponType.Candy:
                    ColorPartButtons[2].Object.SetActive(true);
                    break;
                default:
                    ColorPartButtons[2].Object.SetActive(false);
                    break;
            }

            currentCustomButtonData = ColorPartButtons[0];
        }
        else
        {
            BottomButtons.SetActive(true);
            CustomColorGroup.SetActive(false);
        }
    }
    public void OnClickBuyButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (!CurrentPanel.CheckPreviousWeaponUnlockState())
        {
            return;
        }

        if (DataManager.Instance.Coin > CurrentPanel.currentButtonData.ItemCost) //NOTE: optimize later or not
        {
            CurrentPanel.BuyWeaponHandle();
            SetCoinValue(DataManager.Instance.Coin);

            SetBottomPartState(true, false);

            DataManager.Instance.WeaponUnlockState[CurrentPanel.WeaponTag] = true;
            CurrentPanel.WeaponUnlockStateHandle();
        }
    }

    public void OnClickEquipButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        playerRef.SetWeaponType(CurrentPanel.currentButtonData.WeaponTag);
        playerRef.SetWeaponSkin(CurrentPanel.currentButtonData.WeaponSkinTag);
        playerRef.SetUpHandWeapon();

        SetBottomPartState(true, true);
    }
    public void OnClickSelectedButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
    }
    public void SetItemCost(int itemCost)
    {
        ItemCostText.text = itemCost.ToString();
    }
    public void SetCoinValue(int value)
    {
        CoinDisplay.text = value.ToString();
    }
    public void OnClickNextWeaponPanel()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        OpenWeaponPanel(currentPanelIndex + 1);
    }
    public void OnClickPreviousWeaponPanel()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        OpenWeaponPanel(currentPanelIndex - 1);
    }
    public void OpenWeaponPanel(int index)
    {
        if (index >= 0 && index < WeaponPanelList.Count)
        {
            CurrentPanel.SetActive(false);
            currentPanelIndex = index;
            CurrentPanel = WeaponPanelList[currentPanelIndex];
            CurrentPanel.SetActive(true);

            CurrentPanel.SetUpPanel();
            WeaponButtonClickHandle(CurrentPanel.currentButtonData);
        }
    }
    public void OnCLickExitButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void OnCLickCustomWeapon(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        SetBottomCustomPartState(true);
        SetupCustomPartButton();
        CurrentPanel.SetItemFrame(buttonData);
        CurrentPanel.SetWeaponDisplay(buttonData);
        StartCoroutine(DelaySetSelectMark());
    }
    public void OnClickColorButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        SetPartButtonColor(buttonData.CustomColor);

        int index = currentCustomButtonData.CustomPartIndex;
        DataManager.Instance.CustomColorDict[CurrentPanel.WeaponTag][index] = buttonData.CustomColor;

        CurrentPanel.SetWeaponDisplay(buttonData);
        CurrentPanel.SetupDefaultCustomWeaponColor();

        playerRef.SetUpHandWeapon(); //NOTE: change color when click color button with out cilck equip button
    }
    private void SetPartButtonColor(CustomColor color)
    {
        currentCustomButtonData.ButtonImage.color = ItemStorage.Instance.GetCustomColor(color);
    }
    public void OnClickColorPartButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        currentCustomButtonData = buttonData;
        SelectMark.position = currentCustomButtonData.RectTrans.position;
    }
    public void OnClickEquipCustomButton()
    {
        playerRef.SetWeaponType(CurrentPanel.currentButtonData.WeaponTag);
        playerRef.SetWeaponSkin(WeaponSkinType.Custom);
        playerRef.SetUpHandWeapon();

        OnCLickExitButton(); //NOTE: have play audio func in this, no need play more
    }
    private void SetupCustomPartButton()
    {
        List<CustomColor> colors = DataManager.Instance.CustomColorDict[CurrentPanel.WeaponTag];
        switch (CurrentPanel.WeaponTag)
        {
            case WeaponType.Candy:
                ColorPartButtons[0].ButtonImage.color = ItemStorage.Instance.GetCustomColor(colors[0]);
                ColorPartButtons[1].ButtonImage.color = ItemStorage.Instance.GetCustomColor(colors[1]);
                ColorPartButtons[2].ButtonImage.color = ItemStorage.Instance.GetCustomColor(colors[2]);
                break;
            default:
                ColorPartButtons[0].ButtonImage.color = ItemStorage.Instance.GetCustomColor(colors[0]);
                ColorPartButtons[1].ButtonImage.color = ItemStorage.Instance.GetCustomColor(colors[1]);
                break;
        }
    }
    private void SetupColorButton()
    {
        foreach (ButtonData item in ColorButtons)
        {
            item.ButtonImage.color = ItemStorage.Instance.GetCustomColor(item.CustomColor);
        }
    }

    protected override void OnOpenCanvas()
    {
        if (playerRef == null)
        {
            playerRef = Player.PlayerGlobalReference;
        }

        SetCoinValue(DataManager.Instance.Coin);

        playerRef.PlayerObj.SetActive(false);

        if (isFirstLoad)
        {
            CurrentPanel.SetUpPanel();
            WeaponButtonClickHandle(CurrentPanel.currentButtonData);

            SetupColorButton();

            isFirstLoad = false;
        }
    }
    protected override void OnCloseCanvas()
    {
        playerRef.PlayerObj.SetActive(true);
    }

    public IEnumerator DelaySetSelectMark() //NOTE: Wait for grid layout update when disable or enable child 
    {
        yield return null;
        SelectMark.position = currentCustomButtonData.RectTrans.position;
    }
}

