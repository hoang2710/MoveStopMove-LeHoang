using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkinShopCanvas : UICanvas
{
    private Player playerRef;

    public TMP_Text CoinDisplay;

    public void OnClickPantSkinButton(ButtonData data)
    {
        PlayerPrefs.SetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG, (int)data.PantSkinTag);

        playerRef?.LoadDataFromPlayerPrefs();
        playerRef?.SetUpPantSkin();
    }
    public void OnCLickExitButton()
    {
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void SetCoinValue(int value)
    {
        CoinDisplay.text = value.ToString();
    }

    protected override void OnOpenCanvas()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag(ConstValues.TAG_PLAYER).GetComponent<Player>();
        }

        int currentCoin = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_INT_PLAYER_COIN, 0);
        SetCoinValue(currentCoin);

        playerRef?.ChangeAnimation(ConstValues.ANIM_TRIGGER_DANCE_CHAR_SKIN);
    }

    protected override void OnCloseCanvas()
    {
        playerRef?.ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);
    }
}
