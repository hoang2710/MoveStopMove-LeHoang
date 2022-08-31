using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPauseCanvas : UICanvas
{
    public Toggle SoundToggle;
    public Toggle VibrateToggle;

    private bool isLoadUI;

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
        AudioManager.Instance.SetAudioStatus(!isOn);
        if (!isLoadUI)
        {
            AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        }
    }
    public void OnVibrateToggleValueChange(bool isOn)
    {
        //NOTE: toggle is ON --> hide toggle background
        if (isOn)
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 0); //NOTE: transparent image
        }
        else
        {
            VibrateToggle.image.color = new Color(1, 1, 1, 1);
        }

        //NOTE: Audio manager work
        AudioManager.Instance.SetVibrateStatus(isOn);
        if (!isLoadUI)
        {
            AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        }
    }
    protected override void OnOpenCanvas()
    {
        isLoadUI = true; //NOTE: prevent audio play on load UI
        //NOTE: load audio state from AudioManager --> change toggle state
        bool isSoundOn = AudioManager.Instance.IsSoundOn;
        bool isVibrateOn = AudioManager.Instance.IsVibrateOn;

        SoundToggle.isOn = isSoundOn;
        VibrateToggle.isOn = isVibrateOn;

        isLoadUI = false; //NOTE: prevent audio play on load UI
    }
    public void OnClickHomeButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void OnClickContinueButton()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);
        GameManager.Instance.ChangeGameState(GameState.Playing);
        UIGamePlayCanvas canvas = UIManager.Instance.OpenUI<UIGamePlayCanvas>(UICanvasID.GamePlay);
        canvas.SetActiveTipPanel(false);

        Close();
    }
}
