using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseCanvas : UICanvas
{
    public Toggle SoundToggle;
    public Toggle VibrateToggle;

    public void OnSoundToggleValueChange(bool value)
    {
        //NOTE: toggle is ON --> hide toggle background
        if (value)
        {
            SoundToggle.image.color = new Color(1, 1, 1, 0); //NOTE: transparent image
        }
        else
        {
            SoundToggle.image.color = new Color(1, 1, 1, 1);
        }

        //NOTE: Audio manager work
        //..........
    }
    public void OnVibrateToggleValueChange(bool value)
    {
        //NOTE: toggle is ON --> hide toggle background
        if (value)
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 0); //NOTE: transparent image
        }
        else
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 1);
        }

        //NOTE: Audio manager work
        //..........
    }
    protected override void OnOpenCanvas()
    {
        //NOTE: load audio state from playerPrefs --> change toggle state
        bool isSoundOn = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_BOOL_SOUND_SETTING) == ConstValues.PLAYER_PREFS_BOOL_TRUE_VALUE;
        bool isVibrateOn = PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_BOOL_VIBRATE_SETTING) == ConstValues.PLAYER_PREFS_BOOL_TRUE_VALUE;

        SoundToggle.isOn = isSoundOn;
        VibrateToggle.isOn = isVibrateOn;
    }
    public void OnClickHomeButton()
    {
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void OnClickContinueButton()
    {
        GameManager.Instance.ChangeGameState(GameState.Playing);
        UIGamePlayCanvas canvas = UIManager.Instance.OpenUI<UIGamePlayCanvas>(UICanvasID.GamePlay);
        canvas.SetActiveTipPanel(false);

        Close();
    }
}
