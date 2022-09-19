using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuCanvas : UICanvas
{
    private Player playerRef;

    public RectTransform CanvasRectTrans;
    public Slider PlayerProgressBar;
    public TMP_Text RecordText;
    public TMP_Text CoinText;
    public TMP_InputField PlayerName;
    public RectTransform PlayerNameTrans;
    public Image StarIcon;
    public List<Sprite> StarIconSource;
    public Toggle SoundToggle;
    public Toggle VibrateToggle;
    private Camera curCam;
    [SerializeField] private float InputFieldYAxisOffset;
    private float curScreenHeight;
    private float targetScreenHeight = 1920f;

    private bool isLoadUI;
    private bool isFirstLoad = true; //NOTE: replace start method

    public void OnClickPlayButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.Playing);
        UIManager.Instance.OpenUI(UICanvasID.GamePlay);

        Close();
    }
    public void OnClickWeaponShopButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.WeaponShop);
        UIManager.Instance.OpenUI(UICanvasID.WeaponShop);

        Close();
    }
    public void OnClickSkinShopButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.SkinShop);
        UIManager.Instance.OpenUI(UICanvasID.SkinShop);

        Close();
    }
    public void OnClickRemoveAds()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
    }
    public void OnVibrationToggleValueChange(bool isVibrOn) //NOTE: true is on vibration
    {
        AudioManager.Instance.SetVibrateStatus(isVibrOn);
        if (!isLoadUI)
        {
            AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        }
    }
    public void OnSoundToggleValueChange(bool isMute) //NOTE: true is mute
    {
        AudioManager.Instance.SetAudioStatus(isMute);
        if (!isLoadUI)
        {
            AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        }
    }
    public void ChangeStarIconImageSource(int index)
    {
        //NOTE: check index status, working on detail
        StarIcon.sprite = StarIconSource[index];
    }
    public void SetCoinNumber(int value)
    {
        CoinText.text = value.ToString();
    }
    public void SetPlayerEXPBarValue(float value)
    {
        int index = (int)value / ConstValues.VALUE_EXP_PER_LEVEL;
        if (index > 2)
        {
            index = 2;
            PlayerProgressBar.value = 1f;
        }
        else
        {
            PlayerProgressBar.value = (value % ConstValues.VALUE_EXP_PER_LEVEL) / ConstValues.VALUE_EXP_PER_LEVEL;
        }

        ChangeStarIconImageSource(index);
    }
    public string GetPlayerName()
    {
        return PlayerName.text;
    }
    public void SetPlayerName(TMP_InputField inputField)
    {
        playerRef?.SetPlayerName(inputField.text);
    }
    public void SetupPlayerNameInputField()
    {
        Vector3 pos = curCam.WorldToScreenPoint(playerRef.CharaterTrans.position);
        pos += new Vector3(0, InputFieldYAxisOffset * curScreenHeight / targetScreenHeight, 0);

        PlayerNameTrans.position = pos;
    }
    public void SetupRecordText()
    {
        RecordText.text = "Zone: " + (int)LevelManager.Instance.GetCurrentnLevel() + " - Best: #" + DataManager.Instance.HighestRank;
    }
    protected override void OnOpenCanvas()
    {
        isLoadUI = true; //NOTE: prevent audio play on load UI

        if (isFirstLoad)
        {
            curCam = Camera.main;

            if (playerRef == null)
            {
                playerRef = Player.PlayerGlobalReference;
            }

            curScreenHeight = CanvasRectTrans.sizeDelta.y * CanvasRectTrans.localScale.y;
            SetupPlayerNameInputField();

            PlayerName.onEndEdit.AddListener(delegate { SetPlayerName(PlayerName); });

            isFirstLoad = false;
        }

        int currentCoin = DataManager.Instance.Coin;
        SetCoinNumber(currentCoin);

        float playerEXP = DataManager.Instance.PlayerExp;
        SetPlayerEXPBarValue(playerEXP);

        SetupRecordText();

        //NOTE: load audio state from AudioManager --> change toggle state
        bool isSoundOn = AudioManager.Instance.IsSoundOn;
        bool isVibrateOn = AudioManager.Instance.IsVibrateOn;

        SoundToggle.isOn = !isSoundOn; //NOTE: Icon display when sound on is MuteSound Icon
        VibrateToggle.isOn = isVibrateOn;

        PlayerName.text = playerRef.CharacterName;

        if (GameManager.Instance.PrevGameState == GameState.ResultPhase)
        {
            CheckUnlockOneTimeItem();
        }

        isLoadUI = false; //NOTE: prevent audio play on load UI
    }

    private void CheckUnlockOneTimeItem()
    {
        HatType hatTag = playerRef.HatTag;
        PantSkinType pantSkinTag = playerRef.PantSkinTag;
        ShieldType shieldTag = playerRef.ShieldTag;
        SkinSet skinSetTag = playerRef.SkinSetTag;

        if (DataManager.Instance.UnlockOneTimeHat.Contains(hatTag))
        {
            DataManager.Instance.HatUnlockState[hatTag] = false;
            DataManager.Instance.UnlockOneTimeHat.Remove(hatTag);

            playerRef.SetHat(HatType.None);
            playerRef.SetUpHat();
        }
        if (DataManager.Instance.UnlockOneTimePantSkin.Contains(pantSkinTag))
        {
            DataManager.Instance.PantSkinUnlockState[pantSkinTag] = false;
            DataManager.Instance.UnlockOneTimePantSkin.Remove(pantSkinTag);

            playerRef.SetPantSkin(PantSkinType.Invisible);
            playerRef.SetUpPantSkin();
        }
        if (DataManager.Instance.UnlockOneTimeShield.Contains(shieldTag))
        {
            DataManager.Instance.ShieldUnlockState[shieldTag] = false;
            DataManager.Instance.UnlockOneTimeShield.Remove(shieldTag);

            playerRef.SetShield(ShieldType.None);
            playerRef.SetUpShield();
        }
        if (DataManager.Instance.UnlockOneTimeSkinSet.Contains(skinSetTag))
        {
            DataManager.Instance.SkinSetUnlockState[skinSetTag] = false;
            DataManager.Instance.UnlockOneTimeSkinSet.Remove(skinSetTag);

            playerRef.SetSkinSet(SkinSet.None);
            playerRef.SetupSkinSet();
        }
    }
}
