using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWeaponShopCanvas : UICanvas
{
    private Player playerRef;

    public TMP_Text CoinDisplay;
    public List<GameObject> PanelList;
    private int currentPanelIndex = 0;

    public void OnClickWeaponButton(ButtonData data)
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        playerRef.SetWeaponType(data.WeaponTag);
        playerRef.SetWeaponSkin(data.WeaponSkinTag);
        playerRef.SetUpHandWeapon();
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
    public void OnClickNextWeaponPanel()
    {
        OpenWeaponPanel(currentPanelIndex + 1);
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
    }
    public void OnClickPreviousWeaponPanel()
    {
        OpenWeaponPanel(currentPanelIndex - 1);
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
    }
    public void OpenWeaponPanel(int index)
    {
        if (index >= 0 && index < PanelList.Count)
        {
            PanelList[currentPanelIndex].SetActive(false);
            currentPanelIndex = index;
            PanelList[currentPanelIndex].SetActive(true);
        }
    }
    protected override void OnOpenCanvas()
    {
        if (playerRef == null)
        {
            playerRef = GameObject.FindGameObjectWithTag(ConstValues.TAG_PLAYER).GetComponent<Player>();
        }

        int currentCoin = DataManager.Instance.Coin;
        SetCoinValue(currentCoin);

        playerRef?.PlayerObj.SetActive(false);
    }
    protected override void OnCloseCanvas()
    {
        playerRef?.PlayerObj.SetActive(true);
    }
}

