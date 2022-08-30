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
        DataManager.Instance.SaveData(DataKeys.PLAYER_WEAPON_TYPE_ENUM, (int)data.WeaponTag);
        DataManager.Instance.SaveData(DataKeys.PLAYER_WEAPON_SKIN_ENUM, (int)data.WeaponSkinTag);

        playerRef.LoadDataFromPlayerPrefs();
        playerRef.SetUpHandWeapon();
    }
    public void OnCLickExitButton()
    {
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
    }
    public void OnClickPreviousWeaponPanel()
    {
        OpenWeaponPanel(currentPanelIndex - 1);
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

        int currentCoin = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_INT_PLAYER_COIN, 0);
        SetCoinValue(currentCoin);

        playerRef?.PlayerObj.SetActive(false);
    }
    protected override void OnCloseCanvas()
    {
        playerRef?.PlayerObj.SetActive(true);
    }
}

