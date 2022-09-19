using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkinShopCanvas : UICanvas
{
    private Player playerRef;

    public TMP_Text CoinDisplay;
    public List<GameObject> ItemPanels; //NOTE: set hat panel to active then disactive everything else, must place panel in right order with PanelType enum
    private PanelType currentPanel;

    public List<ButtonData> CategoryButtons; //NOTE: place in right order pls
    private ButtonData currentCategoryButton;
    public Color ButtonOnDeselectColor; //NOTE: category button
    public Color ButtonOnSelectColor;
    public Color IconOnDeselectColor;

    public GameObject BottomLockStateObj;
    public TMP_Text ItemCostText;
    public TMP_Text ItemCostSubText;
    public GameObject HollowButton; //NOTE: use this button to block buy button and display gray state of buy button
    public GameObject BottomUnlockStateObj;
    public GameObject UnequipButtonObj;
    public GameObject EquipButtonObj;
    public RectTransform SelectedFrame; //NOTE: display current selected item
    public RectTransform EquipedText; //NOTE: display currnet equiped item
    public GameObject EquipedTextObj; //NOTE: use for disable text 
    public GameObject UnlockOneTimeTextObj;

    [SerializeField] private ButtonData currentHatButtonData; //NOTE: assign first item in hat panel
    [SerializeField] private ButtonData currentPantButtonData; //NOTE: assign first item in pant panel
    [SerializeField] private ButtonData curretnShieldButtonData; //NOTE: assign first item in shield panel
    [SerializeField] private ButtonData currentSkinSetButtonData; //NOTE: assign first item in skinset panel

    public List<ButtonData> PantButtonDatas; //NOTE: use for setting item lock icon
    public List<ButtonData> HatButtonDatas; //NOTE: use for setting item lock icon
    public List<ButtonData> ShieldButtonDatas; //NOTE: use for setting item lock icon
    public List<ButtonData> SkinSetButtonDatas; //NOTE: use for setting item lock icon

    private PantSkinType finalPantSkinTag; //NOTE: use for decide which pant is equip when out shop
    private HatType finalHatTag; //NOTE: use for decide which hat is equip when out shop
    private ShieldType finalShieldTag; //NOTE: use for decide which hat is equip when out shop
    private SkinSet finalSkinSet; //NOTE: .........................

    private void Start() //NOTE: setting lock icon for each item
    {
        StartCoroutine(SetLockIcon());
    }
    private IEnumerator SetLockIcon()
    {
        foreach (ButtonData item in HatButtonDatas)
        {
            bool isUnlock = DataManager.Instance.HatUnlockState[item.HatTag];
            item.LockIcon.SetActive(!isUnlock);
        }
        yield return null;

        foreach (ButtonData item in PantButtonDatas)
        {
            bool isUnlock = DataManager.Instance.PantSkinUnlockState[item.PantSkinTag];
            item.LockIcon.SetActive(!isUnlock);
        }
        yield return null;

        foreach (ButtonData item in ShieldButtonDatas)
        {
            bool isUnlock = DataManager.Instance.ShieldUnlockState[item.ShieldTag];
            item.LockIcon.SetActive(!isUnlock);
        }
        yield return null;

        foreach (ButtonData item in SkinSetButtonDatas)
        {
            bool isUnlock = DataManager.Instance.SkinSetUnlockState[item.SkinSetDataSO.SkinSet];
            item.LockIcon.SetActive(!isUnlock);
        }
    }
    public void OnClickCategoryButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (buttonData.PanelTag != currentPanel)
        {
            ItemPanels[(int)currentPanel].SetActive(false);
            SetCategoryButtonState(false, currentCategoryButton);

            SetBackItemOnSwitchPanel();

            currentPanel = buttonData.PanelTag;
            currentCategoryButton = CategoryButtons[(int)currentPanel];

            ItemPanels[(int)currentPanel].SetActive(true);
            SetCategoryButtonState(true, currentCategoryButton);

            SetupEquipedMark(currentPanel);

            CategoryPanelHandle();
        }
    }
    private void SetBackItemOnSwitchPanel()
    {
        switch ((int)currentPanel)
        {
            case 0:
                SetBackHat();
                break;
            case 1:
                SetBackPant();
                break;
            case 2:
                SetBackShield();
                break;
            case 3:
                SetBackSkinSetOnChangePanel();
                break;
            default:
                break;
        }
    }
    private void CategoryPanelHandle()
    {
        switch ((int)currentPanel)
        {
            case 0:
                ItemStateHandler(DataManager.Instance.HatUnlockState[currentHatButtonData.HatTag], currentHatButtonData);
                SetSelectedFrame(currentHatButtonData.RectTrans);
                playerRef.SetHat(currentHatButtonData.HatTag);
                playerRef.SetUpHat();
                break;
            case 1:
                ItemStateHandler(DataManager.Instance.PantSkinUnlockState[currentPantButtonData.PantSkinTag], currentPantButtonData);
                SetSelectedFrame(currentPantButtonData.RectTrans);
                playerRef.SetPantSkin(currentPantButtonData.PantSkinTag);
                playerRef.SetUpPantSkin();
                break;
            case 2:
                ItemStateHandler(DataManager.Instance.ShieldUnlockState[curretnShieldButtonData.ShieldTag], curretnShieldButtonData);
                SetSelectedFrame(curretnShieldButtonData.RectTrans);
                playerRef.SetShield(curretnShieldButtonData.ShieldTag);
                playerRef.SetUpShield();
                break;
            case 3:
                ItemStateHandler(DataManager.Instance.SkinSetUnlockState[currentSkinSetButtonData.SkinSetDataSO.SkinSet], currentSkinSetButtonData);
                SetSelectedFrame(currentSkinSetButtonData.RectTrans);
                playerRef.SetSkinSet(currentSkinSetButtonData.SkinSetDataSO.SkinSet);
                playerRef.SetupSkinSet();
                break;
            default:
                break;
        }
    }
    public void OnClickHatItemButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (currentHatButtonData != buttonData)
        {
            currentHatButtonData = buttonData;

            playerRef.SetHat(currentHatButtonData.HatTag);
            playerRef.SetUpHat();

            SetSelectedFrame(buttonData.RectTrans);

            ItemStateHandler(DataManager.Instance.HatUnlockState[buttonData.HatTag], buttonData);
        }
    }
    public void OnClickPantSkinButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (currentPantButtonData != buttonData)
        {
            currentPantButtonData = buttonData;

            playerRef.SetPantSkin(currentPantButtonData.PantSkinTag);
            playerRef.SetUpPantSkin();

            SetSelectedFrame(buttonData.RectTrans);

            ItemStateHandler(DataManager.Instance.PantSkinUnlockState[buttonData.PantSkinTag], buttonData);
        }
    }
    public void OnClickShieldButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (curretnShieldButtonData != buttonData)
        {
            curretnShieldButtonData = buttonData;

            playerRef.SetShield(curretnShieldButtonData.ShieldTag);
            playerRef.SetUpShield();

            SetSelectedFrame(buttonData.RectTrans);

            ItemStateHandler(DataManager.Instance.ShieldUnlockState[buttonData.ShieldTag], buttonData);
        }
    }
    public void OnclickSkinSetButton(ButtonData buttonData)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        if (currentSkinSetButtonData != buttonData)
        {
            currentSkinSetButtonData = buttonData;

            playerRef.SetSkinSet(currentSkinSetButtonData.SkinSetDataSO.SkinSet);
            playerRef.SetupSkinSet();

            SetSelectedFrame(buttonData.RectTrans);

            ItemStateHandler(DataManager.Instance.SkinSetUnlockState[buttonData.SkinSetDataSO.SkinSet], buttonData);
        }
    }
    private void ItemStateHandler(bool isUnlock, ButtonData buttonData)
    {
        if (isUnlock)
        {
            ItemUnlockHandle(buttonData);
        }
        else
        {
            ItemLockHandle(buttonData);
        }
    }
    public void OnClickBuyButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        switch ((int)currentPanel)
        {
            case 0:
                DataManager.Instance.Coin -= currentHatButtonData.ItemCost;
                DataManager.Instance.HatUnlockState[currentHatButtonData.HatTag] = true;
                ItemUnlockHandle(currentHatButtonData);
                currentHatButtonData.LockIcon.SetActive(false);
                break;
            case 1:
                DataManager.Instance.Coin -= currentPantButtonData.ItemCost;
                DataManager.Instance.PantSkinUnlockState[currentPantButtonData.PantSkinTag] = true;
                ItemUnlockHandle(currentPantButtonData);
                currentPantButtonData.LockIcon.SetActive(false);
                break;
            case 2:
                DataManager.Instance.Coin -= curretnShieldButtonData.ItemCost;
                DataManager.Instance.ShieldUnlockState[curretnShieldButtonData.ShieldTag] = true;
                ItemUnlockHandle(curretnShieldButtonData);
                curretnShieldButtonData.LockIcon.SetActive(false);
                break;
            case 3:
                DataManager.Instance.Coin -= currentSkinSetButtonData.ItemCost;
                DataManager.Instance.SkinSetUnlockState[currentSkinSetButtonData.SkinSetDataSO.SkinSet] = true;
                ItemUnlockHandle(currentSkinSetButtonData);
                currentSkinSetButtonData.LockIcon.SetActive(false);
                break;
            default:
                break;
        }

        SetCoinValue(DataManager.Instance.Coin);
    }
    public void OnClickUnlockOneTimeButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        switch ((int)currentPanel)
        {
            case 0:
                DataManager.Instance.UnlockOneTimeHat.Add(currentHatButtonData.HatTag);
                DataManager.Instance.HatUnlockState[currentHatButtonData.HatTag] = true;
                ItemUnlockHandle(currentHatButtonData);
                currentHatButtonData.LockIcon.SetActive(false);
                break;
            case 1:
                DataManager.Instance.UnlockOneTimePantSkin.Add(currentPantButtonData.PantSkinTag);
                DataManager.Instance.PantSkinUnlockState[currentPantButtonData.PantSkinTag] = true;
                ItemUnlockHandle(currentPantButtonData);
                currentPantButtonData.LockIcon.SetActive(false);
                break;
            case 2:
                DataManager.Instance.UnlockOneTimeShield.Add(curretnShieldButtonData.ShieldTag);
                DataManager.Instance.ShieldUnlockState[curretnShieldButtonData.ShieldTag] = true;
                ItemUnlockHandle(curretnShieldButtonData);
                curretnShieldButtonData.LockIcon.SetActive(false);
                break;
            case 3:
                DataManager.Instance.UnlockOneTimeSkinSet.Add(currentSkinSetButtonData.SkinSetDataSO.SkinSet);
                DataManager.Instance.SkinSetUnlockState[currentSkinSetButtonData.SkinSetDataSO.SkinSet] = true;
                ItemUnlockHandle(currentSkinSetButtonData);
                currentSkinSetButtonData.LockIcon.SetActive(false);
                break;
            default:
                break;
        }
    }
    private void ItemLockHandle(ButtonData buttonData)
    {
        BottomLockStateObj.SetActive(true);
        BottomUnlockStateObj.SetActive(false);
        UnlockOneTimeTextObj.SetActive(false);

        SetItemCost(buttonData.ItemCost);

        if (buttonData.ItemCost > DataManager.Instance.Coin)
        {
            HollowButton.SetActive(true);
        }
        else
        {
            HollowButton.SetActive(false);
        }
    }
    private void ItemUnlockHandle(ButtonData buttonData)
    {
        BottomLockStateObj.SetActive(false);
        BottomUnlockStateObj.SetActive(true);
        UnlockOneTimeTextObj.SetActive(false);

        switch ((int)currentPanel)
        {
            case 0:
                EquipHandler(buttonData.HatTag == finalHatTag);
                SetUnlockOneTimeTextState(DataManager.Instance.UnlockOneTimeHat.Contains(buttonData.HatTag));
                break;
            case 1:
                EquipHandler(buttonData.PantSkinTag == finalPantSkinTag);
                SetUnlockOneTimeTextState(DataManager.Instance.UnlockOneTimePantSkin.Contains(buttonData.PantSkinTag));
                break;
            case 2:
                EquipHandler(buttonData.ShieldTag == finalShieldTag);
                SetUnlockOneTimeTextState(DataManager.Instance.UnlockOneTimeShield.Contains(buttonData.ShieldTag));
                break;
            case 3:
                EquipHandler(buttonData.SkinSetDataSO.SkinSet == finalSkinSet);
                SetUnlockOneTimeTextState(DataManager.Instance.UnlockOneTimeSkinSet.Contains(buttonData.SkinSetDataSO.SkinSet));
                break;
            default:
                break;
        }
    }
    public void OnClickEquipButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        switch ((int)currentPanel)
        {
            case 0:
                finalHatTag = currentHatButtonData.HatTag;
                finalSkinSet = SkinSet.None;
                SetEquipedMark(currentHatButtonData);
                EquipHandler(true);
                break;
            case 1:
                finalPantSkinTag = currentPantButtonData.PantSkinTag;
                finalSkinSet = SkinSet.None;
                SetEquipedMark(currentPantButtonData);
                EquipHandler(true);
                break;
            case 2:
                finalShieldTag = curretnShieldButtonData.ShieldTag;
                finalSkinSet = SkinSet.None;
                SetEquipedMark(curretnShieldButtonData);
                EquipHandler(true);
                break;
            case 3:
                finalSkinSet = currentSkinSetButtonData.SkinSetDataSO.SkinSet;
                finalHatTag = HatType.None;
                finalPantSkinTag = PantSkinType.Invisible;
                finalShieldTag = ShieldType.None;
                SetEquipedMark(currentSkinSetButtonData);
                EquipHandler(true);
                break;
            default:
                break;
        }
    }
    private void EquipHandler(bool isEquiped)
    {
        if (!isEquiped)
        {
            EquipButtonObj.SetActive(true);
            UnequipButtonObj.SetActive(false);
        }
        else
        {
            EquipButtonObj.SetActive(false);
            UnequipButtonObj.SetActive(true);
        }
    }
    private void SetUnlockOneTimeTextState(bool isOn)
    {
        UnlockOneTimeTextObj.SetActive(isOn);
    }
    private void SetEquipedMark(ButtonData buttonData)
    {
        EquipedTextObj.SetActive(true);
        EquipedText.SetParent(buttonData.RectTrans);
        EquipedText.position = buttonData.RectTrans.position;
    }
    public void OnClickHollowButton() //NOTE: the button display when not have enough coin
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
    }
    public void OnClickUnequipButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        switch ((int)currentPanel)
        {
            case 0:
                finalHatTag = HatType.None;
                EquipedTextObj.SetActive(false);
                EquipHandler(false);
                break;
            case 1:
                finalPantSkinTag = PantSkinType.Invisible;
                EquipedTextObj.SetActive(false);
                EquipHandler(false);
                break;
            case 2:
                finalShieldTag = ShieldType.None;
                EquipedTextObj.SetActive(false);
                EquipHandler(false);
                break;
            case 3:
                finalSkinSet = SkinSet.None;
                EquipedTextObj.SetActive(false);
                EquipHandler(false);
                break;
            default:
                break;
        }
    }
    public void OnCLickExitButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.MainMenu);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void SetCoinValue(int value)
    {
        CoinDisplay.text = value.ToString();
    }
    private void SetCategoryButtonState(bool isSelected, ButtonData buttonData)
    {
        if (isSelected)
        {
            buttonData.ButtonImage.color = ButtonOnSelectColor;
            buttonData.IconImage.color = Color.white;
        }
        else
        {
            buttonData.ButtonImage.color = ButtonOnDeselectColor;
            buttonData.IconImage.color = IconOnDeselectColor;
        }
    }
    public void SetItemCost(int value)
    {
        ItemCostText.text = value.ToString();
        ItemCostSubText.text = value.ToString();
    }


    protected override void OnOpenCanvas()
    {
        if (playerRef == null)
        {
            playerRef = Player.PlayerGlobalReference;
        }

        playerRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_DANCE_CHAR_SKIN);

        finalSkinSet = playerRef.SkinSetTag;
        if (finalSkinSet == SkinSet.None)
        {
            finalHatTag = playerRef.HatTag;
            finalPantSkinTag = playerRef.PantSkinTag;
            finalShieldTag = playerRef.ShieldTag;
        }
        else
        {
            finalHatTag = HatType.None;
            finalPantSkinTag = PantSkinType.Invisible;
            finalShieldTag = ShieldType.None;
        }

        playerRef.UnequipSkinSet();

        SetupEquipedMark(currentPanel);

        SetCoinValue(DataManager.Instance.Coin);

        currentCategoryButton = CategoryButtons[(int)currentPanel];
        CategoryPanelHandle();
    }

    protected override void OnCloseCanvas()
    {
        playerRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);

        SetBackSKinSet();
    }
    private void SetBackHat()
    {
        if (finalSkinSet == SkinSet.None)
        {
            playerRef.SetHat(finalHatTag);
        }
        else
        {
            playerRef.SetHat(HatType.None);
        }

        playerRef.SetUpHat();
    }
    private void SetBackPant()
    {
        if (finalSkinSet == SkinSet.None)
        {
            playerRef.SetPantSkin(finalPantSkinTag);
        }
        else
        {
            playerRef.SetPantSkin(PantSkinType.Invisible);
        }

        playerRef.SetUpPantSkin();
    }
    private void SetBackShield()
    {
        if (finalSkinSet == SkinSet.None)
        {
            playerRef.SetShield(finalShieldTag);
        }
        else
        {
            playerRef.SetShield(ShieldType.None);
        }

        playerRef.SetUpShield();
    }
    private void SetBackSKinSet()
    {
        playerRef.SetSkinSet(finalSkinSet);

        if (finalSkinSet == SkinSet.None)
        {
            playerRef.UnequipSkinSet();
            SetBackHat();
            SetBackPant();
            SetBackShield();
        }
        else
        {
            playerRef.SetupSkinSet();
        }
    }
    private void SetBackSkinSetOnChangePanel()
    {
        playerRef.UnequipSkinSet();
        SetBackHat();
        SetBackPant();
        SetBackShield();
    }
    private void SetSelectedFrame(RectTransform parentButton)
    {
        SelectedFrame.position = parentButton.position;
        SelectedFrame.SetParent(parentButton);
    }
    private void SetupEquipedMark(PanelType panelType)
    {
        switch (panelType)
        {
            case PanelType.HatPanel:
                SetupEquipedMarkHatPanel();
                break;
            case PanelType.PantPanel:
                SetupEquipedMarkPantPanel();
                break;
            case PanelType.ShieldPanel:
                SetupEquipedMarkShieldPanel();
                break;
            case PanelType.SkinSetPanel:
                SetupEquipedMarkSkinSetPanel();
                break;
            default:
                break;
        }
    }
    private void SetupEquipedMarkHatPanel()
    {
        if (finalHatTag == HatType.None || finalSkinSet != SkinSet.None)
        {
            EquipedTextObj.SetActive(false);
        }
        else
        {
            for (int i = 0; i < HatButtonDatas.Count; i++)
            {
                if (HatButtonDatas[i].HatTag == finalHatTag)
                {
                    SetEquipedMark(HatButtonDatas[i]);
                    break;
                }
            }
        }
    }
    private void SetupEquipedMarkPantPanel()
    {
        if (finalPantSkinTag == PantSkinType.Invisible || finalSkinSet != SkinSet.None)
        {
            EquipedTextObj.SetActive(false);
        }
        else
        {
            for (int i = 0; i < PantButtonDatas.Count; i++)
            {
                if (PantButtonDatas[i].PantSkinTag == finalPantSkinTag)
                {
                    SetEquipedMark(PantButtonDatas[i]);
                    break;
                }
            }
        }
    }
    private void SetupEquipedMarkShieldPanel()
    {
        if (finalShieldTag == ShieldType.None || finalSkinSet != SkinSet.None)
        {
            EquipedTextObj.SetActive(false);
        }
        else
        {
            for (int i = 0; i < ShieldButtonDatas.Count; i++)
            {
                if (ShieldButtonDatas[i].ShieldTag == finalShieldTag)
                {
                    SetEquipedMark(ShieldButtonDatas[i]);
                    break;
                }
            }
        }
    }
    private void SetupEquipedMarkSkinSetPanel()
    {
        if (finalSkinSet == SkinSet.None)
        {
            EquipedTextObj.SetActive(false);
        }
        else
        {
            for (int i = 0; i < SkinSetButtonDatas.Count; i++)
            {
                if (SkinSetButtonDatas[i].SkinSetDataSO.SkinSet == finalSkinSet)
                {
                    SetEquipedMark(SkinSetButtonDatas[i]);
                    break;
                }
            }
        }
    }

    public void SetBackData() //NOTE: use for set back player origin data when quit on skinshop panel
    {
        playerRef.SetHat(finalHatTag);
        playerRef.SetPantSkin(finalPantSkinTag);
        playerRef.SetShield(finalShieldTag);
        playerRef.SetSkinSet(finalSkinSet);
    }
}

public enum PanelType
{
    HatPanel,
    PantPanel,
    ShieldPanel,
    SkinSetPanel
}
