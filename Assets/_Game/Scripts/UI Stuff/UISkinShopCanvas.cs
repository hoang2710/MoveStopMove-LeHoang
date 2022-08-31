using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UISkinShopCanvas : UICanvas
{
    private Player playerRef;

    public TMP_Text CoinDisplay;
    public List<GameObject> ItemPanel;

    public void OnClickPantSkinButton(ButtonData data)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        playerRef?.SetPantSkin(data.PantSkinTag);
        playerRef?.SetUpPantSkin();
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

    protected override void OnOpenCanvas()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag(ConstValues.TAG_PLAYER).GetComponent<Player>();
        }

        int currentCoin = DataManager.Instance.Coin;
        SetCoinValue(currentCoin);

        playerRef?.ChangeAnimation(ConstValues.ANIM_TRIGGER_DANCE_CHAR_SKIN);
    }

    protected override void OnCloseCanvas()
    {
        playerRef?.ChangeAnimation(ConstValues.ANIM_TRIGGER_IDLE);
    }
}
