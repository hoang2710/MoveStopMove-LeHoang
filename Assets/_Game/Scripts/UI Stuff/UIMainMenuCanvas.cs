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

    public void OnClickPlayButton()
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        UIManager.Instance.OpenUI(UICanvasID.GamePlay);

        Close();
    }
    public void OnClickWeaponShopButton()
    {
        GameManager.Instance.ChangeGameState(GameState.WeaponShop);
        UIManager.Instance.OpenUI(UICanvasID.WeaponShop);

        Close();
    }
    public void OnClickSkinShopButton()
    {
        GameManager.Instance.ChangeGameState(GameState.SkinShop);
        UIManager.Instance.OpenUI(UICanvasID.SkinShop);

        Close();
    }
    public void OnClickRemoveAds()
    {

    }
    public void OnVibrationToggleValueChange(bool isVibrOn) //NOTE: true is on vibration
    {
        AudioManager.Instance.SetVibrateStatus(isVibrOn);
    }
    public void OnSoundToggleValueChange(bool isMute) //NOTE: true is mute
    {
        AudioManager.Instance.SetAudioStatus(isMute);
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
        //NOTE: Save player name when enter playing mode
        SavePlayerName();
        return PlayerName.text;
    }
    public void SavePlayerName()
    {
        DataManager.Instance.SaveData(DataKeys.PLAYER_NAME, PlayerName.text);
    }
    protected override void OnOpenCanvas()
    {
        int currentCoin = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_INT_PLAYER_COIN);
        SetCoinNumber(currentCoin);

        //NOTE: load audio state from playerPrefs --> change toggle state
        bool isSoundOn = DataManager.Instance.LoadToggleSetting(DataKeys.SOUND_SETTING);
        bool isVibrateOn = DataManager.Instance.LoadToggleSetting(DataKeys.VIBRATE_SETTING);

        SoundToggle.isOn = !isSoundOn; //NOTE: Icon display when sound on is MuteSound Icon
        VibrateToggle.isOn = isVibrateOn;

        string name;
        if (DataManager.Instance.LoadData(DataKeys.PLAYER_NAME, out name))
        {
            PlayerName.text = name;
        }
    }
}
