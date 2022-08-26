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
        Debug.Log(isVibrOn);
        //NOTE: Audio manager work
        //..........
    }
    public void OnSoundToggleValueChange(bool isMute) //NOTE: true is mute
    {
        Debug.Log(isMute);
        //NOTE: Audio manager work
        //..........
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
        int currentCoin = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_INT_PLAYER_COIN);
        SetCoinNumber(currentCoin);
    }
}
