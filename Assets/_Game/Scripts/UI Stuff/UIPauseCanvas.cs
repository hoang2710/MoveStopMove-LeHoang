using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseCanvas : UICanvas
{
    public Toggle SoundToggle;
    public Toggle VibrateToggle;

    public void OnSoundToggleValueChange(bool isOn)
    {
        //NOTE: toggle is ON --> hide toggle background
        if (isOn)
        {
            SoundToggle.image.color = new Color(1, 1, 1, 0); //NOTE: transparent image
        }
        else
        {
            SoundToggle.image.color = new Color(1, 1, 1, 1);
        }

        //NOTE: Audio manager work
        DataManager.Instance.SaveToggleSetting(DataKeys.SOUND_SETTING, isOn);
    }
    public void OnVibrateToggleValueChange(bool isVibrOn)
    {
        //NOTE: toggle is ON --> hide toggle background
        if (isVibrOn)
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 0); //NOTE: transparent image
        }
        else
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 1);
        }

        //NOTE: Audio manager work
        DataManager.Instance.SaveToggleSetting(DataKeys.VIBRATE_SETTING, isVibrOn);
    }
    protected override void OnOpenCanvas()
    {
        //NOTE: load audio state from playerPrefs --> change toggle state
        bool isSoundOn = DataManager.Instance.LoadToggleSetting(DataKeys.SOUND_SETTING);
        bool isVibrateOn = DataManager.Instance.LoadToggleSetting(DataKeys.VIBRATE_SETTING);

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
