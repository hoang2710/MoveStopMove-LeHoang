using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGamePlayCanvas : UICanvas
{
    public TMP_Text PlayerAliveCount;
    public GameObject TipPanel;
    private bool TipPanelActiveFlag = true; //NOTE: must set active true on editor 
    public JoystickInput JoystickInput;

    public void OnClickSettingButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.Pause);
        UIManager.Instance.OpenUI(UICanvasID.Setting);

        Close();
    }
    public void SetActiveTipPanel(bool value)
    {
        if (TipPanelActiveFlag != value)
        {
            TipPanelActiveFlag = value;
            TipPanel.SetActive(value);
        }
    }
    public void SetPlayerAliveCount(int numOfPlayer)
    {
        PlayerAliveCount.text = "Alive: " + numOfPlayer.ToString();
    }

    protected override void OnOpenCanvas()
    {
        SetActiveTipPanel(true);
    }
    protected override void OnCloseCanvas()
    {
        JoystickInput.SetBackState();
    }
}
