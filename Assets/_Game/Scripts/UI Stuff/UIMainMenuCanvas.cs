using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMainMenuCanvas : UICanvas
{
    public Slider PlayerProgressBar;
    public TMP_Text RecordText;
    public TMP_Text CoinText;
    public TMP_InputField PlayerName;
    public Image StarIcon;
    public List<Sprite> StarIconSource;
    public Toggle SoundToggle;
    public Toggle VibrateToggle;

    private bool isLoadUI;

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
    public void SetProgressBarValue(int value)
    {
        // int tmp = value % ConstValues.VALUE_EXP_PER_LEVEL;
        // PlayerProgressBar.value = (float)tmp / ConstValues.VALUE_EXP_PER_LEVEL;
    }
    public string GetPlayerName()
    {
        return PlayerName.text;
    }
    protected override void OnOpenCanvas()
    {
        isLoadUI = true; //NOTE: prevent audio play on load UI

        int currentCoin = DataManager.Instance.Coin;
        SetCoinNumber(currentCoin);

        //NOTE: load audio state from AudioManager --> change toggle state
        bool isSoundOn = AudioManager.Instance.IsSoundOn;
        bool isVibrateOn = AudioManager.Instance.IsVibrateOn;

        SoundToggle.isOn = !isSoundOn; //NOTE: Icon display when sound on is MuteSound Icon
        VibrateToggle.isOn = isVibrateOn;

        PlayerName.text = DataManager.Instance.GetGameData().PlayerName;

        isLoadUI = false; //NOTE: prevent audio play on load UI
    }
}
