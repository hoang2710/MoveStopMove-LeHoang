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

    [SerializeField] private ButtonData currentHatButtonData; //NOTE: assign first item in hat panel
    [SerializeField] private ButtonData currentPantButtonData; //NOTE: assign first item in pant panel
    [SerializeField] private ButtonData curretnShieldButtonData; //NOTE: assign first item in shield panel
    private ButtonData currentButtonData; //NOTE: use for set cost value when re enter skin shop panel, assign first item of hat panel

    public List<ButtonData> PantButtonDatas; //NOTE: use for setting item lock icon
    public List<ButtonData> HatButtonDatas; //NOTE: use for setting item lock icon
    public List<ButtonData> ShieldButtonDatas; //NOTE: use for setting item lock icon

    private PantSkinType finalPantSkinTag; //NOTE: use for decide which pant is equip when out shop
    private HatType finalHatTag; //NOTE: use for decide which hat is equip when out shop
    private ShieldType finalShieldTag; //NOTE: use for decide which hat is equip when out shop

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
                break;
            default:
                break;
        }

        SetCoinValue(DataManager.Instance.Coin);
    }
    private void ItemLockHandle(ButtonData buttonData)
    {
        BottomLockStateObj.SetActive(true);
        BottomUnlockStateObj.SetActive(false);

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

        switch ((int)currentPanel)
        {
            case 0:
                EquipHandler(buttonData.HatTag == finalHatTag);
                break;
            case 1:
                EquipHandler(buttonData.PantSkinTag == finalPantSkinTag);
                break;
            case 2:
                EquipHandler(buttonData.ShieldTag == finalShieldTag);
                break;
            case 3:
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
                SetEquipedMark(currentHatButtonData);
                EquipHandler(true);
                break;
            case 1:
                finalPantSkinTag = currentPantButtonData.PantSkinTag;
                SetEquipedMark(currentPantButtonData);
                EquipHandler(true);
                break;
            case 2:
                finalShieldTag = curretnShieldButtonData.ShieldTag;
                SetEquipedMark(curretnShieldButtonData);
                EquipHandler(true);
                break;
            case 3:
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

        finalHatTag = playerRef.HatTag;
        finalPantSkinTag = playerRef.PantSkinTag;
        finalShieldTag = playerRef.ShieldTag;

        SetupEquipedMark(currentPanel);

        SetCoinValue(DataManager.Instance.Coin);

        currentCategoryButton = CategoryButtons[(int)currentPanel];
        CategoryPanelHandle();
    }

    protected override void OnCloseCanvas()
    {
        playerRef.ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);

        SetBackHat();
        SetBackPant();
        SetBackShield();
    }
    private void SetBackHat()
    {
        playerRef.SetHat(finalHatTag);
        playerRef.SetUpHat();
    }
    private void SetBackPant()
    {
        playerRef.SetPantSkin(finalPantSkinTag);
        playerRef.SetUpPantSkin();
    }
    private void SetBackShield()
    {
        playerRef.SetShield(finalShieldTag);
        playerRef.SetUpShield();
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
                break;
            default:
                break;
        }
    }
    private void SetupEquipedMarkHatPanel()
    {
        if (finalHatTag == HatType.None)
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
        if (finalPantSkinTag == PantSkinType.Invisible)
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
        if (finalShieldTag == ShieldType.None)
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

    private void OnApplicationPause(bool isPause) //NOTE: Set back player setting when quit on skin shop ui, for android
    {
        if (isPause)
        {
            playerRef.SetHat(finalHatTag);
            playerRef.SetPantSkin(finalPantSkinTag);
            playerRef.SetShield(finalShieldTag);
        }
    }
    private void OnApplicationQuit() //NOTE: for window
    {
        playerRef.SetHat(finalHatTag);
        playerRef.SetPantSkin(finalPantSkinTag);
        playerRef.SetShield(finalShieldTag);
    }
}

public enum PanelType
{
    HatPanel,
    PantPanel,
    ShieldPanel,
    SkinSetPanel
}
