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
        UIManager.Instance.OpenUI(UICanvasID.WeaponShop);

        Close();
    }
    public void OnClickSkinShopButton()
    {
        UIManager.Instance.OpenUI(UICanvasID.SkinShop);

        Close();
    }
    public void OnClickRemoveAds()
    {

    }
    public void OnClickVibrationButton(bool value) //NOTE: true is on vibration
    {
        Debug.Log(value);
    }
    public void OnClickSoundButton(bool value) //NOTE: true is mute
    {
        Debug.Log(value);
    }
    public void ChangeStarIconImageSource(int index)
    {
        //NOTE: check index status
        StarIcon.sprite = StarIconSource[index];
    }
    public void SetCoinNumber(int value)
    {
        CoinText.text = value.ToString();
    }
    public void SetProgressBarValue(float value)
    {
        PlayerProgressBar.value = value;
    }
    public string GetPlayerName()
    {
        return PlayerName.text;
    }
    
}
