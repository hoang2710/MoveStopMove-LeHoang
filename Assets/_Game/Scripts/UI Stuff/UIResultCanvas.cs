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
    public TMP_Text QuoteText;
    public Slider ProgressBar;
    [Header("Win Panel")]
    public GameObject WinPanel;
    public TMP_Text CoinDisplayTextWin;
    [Header("Lose Panel")]
    public GameObject LosePanel;
    public TMP_Text RankingText;
    public TMP_Text KillerNameText;
    public TMP_Text CoinDisplayTextLose;

    public void SetActiveResultPanel(bool isWin)
    {
        WinPanel.SetActive(isWin);
        LosePanel.SetActive(!isWin);
    }
    public void SetProgressBarValue(float value) //NOTE: Range 0 ~ 1.0
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
    }
    public void SetQuoteText(string text)
    {
        QuoteText.text = text;
    }
    public void SetCoinValue(int value)
    {
        CoinDisplayTextWin.text = value.ToString();
        CoinDisplayTextLose.text = value.ToString();
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
    public void OnClickHomeButton()
    { Debug.LogWarning("Home button");
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);

        Close();
    }
    public void OnClickNextZoneButton()
    { Debug.LogWarning("NextLevel button");
        GameManager.Instance.isNextZone = true;
        GameManager.Instance.ChangeGameState(GameState.LoadLevel);
        UIManager.Instance.OpenUI(UICanvasID.GamePlay);

        Close();
    }
    public void OnCLickScreenShotButton()
    {
        //NOTE: empty
    }

    protected override void OnOpenCanvas()
    {
        UIManager.Instance.CloseUI(UICanvasID.GamePlay);
    }
}
