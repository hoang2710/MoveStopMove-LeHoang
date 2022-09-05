using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResultCanvas : UICanvas
{
    [Header("Progress Zone")]
    public Image IconCurrentZone;
    public TMP_Text TextCurrentZone;
    public Image IconNextZone;
    public TMP_Text TextNextZone;
    public Image IconLockNextZone;
    public Image IconUnlockNextZone;
    public Image IconSkull;
    public Image UnlockBackground;
    public Image ZoneIconUnlock;
    public TMP_Text QuoteText;
    public Slider ProgressBar;
    [Header("Win Panel")]
    public GameObject WinPanel;
    [Header("Lose Panel")]
    public GameObject LosePanel;
    public TMP_Text RankingText;
    public TMP_Text KillerNameText;
    [Header("Bottom Part")]
    public TMP_Text CoinDisplayText;
    public GameObject LoseBottomPart;
    public GameObject WinBottomPart;

    private int playerRank;
    private float progressPercentage;
    private int numOfCoinReward;

    public void SetActiveResultPanel(bool isWin)
    {
        WinPanel.SetActive(isWin);
        LosePanel.SetActive(!isWin);
    }
    /// <param name ="value"> Range 0 ~ 1.0</param>
    public void SetProgressBarValue(float value)
    {
        ProgressBar.value = value;
    }
    public void SetZoneText(int curZone, int nextZone)
    {
        TextCurrentZone.text = "ZONE: " + curZone.ToString();
        TextNextZone.text = "ZONE: " + nextZone.ToString();
    }
    public void SetActiveSkullIcon(bool isOn)
    {
        IconSkull.enabled = isOn;
    }
    public void SetLockZoneState(bool isLock)
    {
        IconLockNextZone.enabled = isLock;
        IconUnlockNextZone.enabled = !isLock;
        UnlockBackground.enabled = !isLock;
        ZoneIconUnlock.enabled = !isLock;
    }
    public void SetQuoteText(string text)
    {
        QuoteText.text = text;
    }
    public void SetCoinValue(int value)
    {
        CoinDisplayText.text = value.ToString();
    }
    public void SetRankingLosePanel(int rank)
    {
        RankingText.text = "#" + rank.ToString();
    }
    public void SetKillerName(string name, Color color)
    {
        KillerNameText.text = name;
        KillerNameText.color = color;
    }
    public void SetUpBottomPartState(bool isWin)
    {
        if (isWin)
        {
            WinBottomPart.SetActive(true);
            LoseBottomPart.SetActive(false);
        }
        else
        {
            WinBottomPart.SetActive(false);
            LoseBottomPart.SetActive(true);
        }
    }
    public void OnClickHomeButton()
    {
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        Close();
    }
    public void OnClickNextZoneButton()
    {
        LevelManager.Instance.ChangeLevelToLoad(true);
        GameManager.Instance.SetBoolIsNextZone(true);
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.GamePlay);
        AudioManager.Instance.PlayAudioClip(AudioType.ButtonClick);

        Close();
    }
    public void OnCLickScreenShotButton()
    {
        //NOTE: empty
    }

    protected override void OnOpenCanvas()
    {
        UIManager.Instance.CloseUI(UICanvasID.GamePlay);

        LevelManager.Instance.GetLevelResult(out playerRank, out numOfCoinReward, out progressPercentage);

        if (playerRank > 1)
        {
            SetActiveResultPanel(false);
            SetUpBottomPartState(false);
            LoseResultHandle();
        }
        else
        {
            SetActiveResultPanel(true);
            SetUpBottomPartState(true);
            WinResultHandle();
        }
    }

    private void LoseResultHandle()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.Lose);

        SetRankingLosePanel(playerRank);
        SetProgressBarValue(progressPercentage);
        SetCoinValue(numOfCoinReward);
        SetActiveSkullIcon(true);
        SetLockZoneState(true);
    }
    private void WinResultHandle()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.Win);

        SetProgressBarValue(1f);
        SetCoinValue(numOfCoinReward);
        SetActiveSkullIcon(false);
        SetLockZoneState(false);
    }
}
