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
    public List<string> WinText;
    public List<string> LoseText;
    public Slider ProgressBar;
    [Header("Win Panel")]
    public GameObject WinPanel;
    [Header("Lose Panel")]
    public GameObject LosePanel;
    public TMP_Text RankingText;
    public TMP_Text KillerNameText;
    [Header("Bottom Part")]
    public TMP_Text CoinDisplayText;
    public GameObject NextZoneButton;
    public GameObject HomeButton;
    public Transform CoinDisplay;
    public Transform Holder;
    public GameObject TripleRewardButton;
    public TMP_Text NextZoneButtonText;
    private Vector3 defaultCoinDisplayPosition;

    private int playerRank;
    private float progressPercentage;
    private int numOfCoinReward;

    private bool isFisrtLoad = true;

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
    public void SetQuoteText(bool isWin)
    {
        if (isWin)
        {
            if (WinText.Count > 0)
            {
                int ran = Random.Range(0, WinText.Count);
                QuoteText.text = WinText[ran];
            }
        }
        else
        {
            if (LoseText.Count > 0)
            {
                int ran = Random.Range(0, LoseText.Count);
                QuoteText.text = LoseText[ran];
            }
        }
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
    public void SetUpBottomButtons(bool isWin)
    {
        if (isWin)
        {
            NextZoneButton.SetActive(true);
            HomeButton.SetActive(false);
        }
        else
        {
            NextZoneButton.SetActive(false);
            HomeButton.SetActive(true);
        }
    }
    private void SetupTripleRewardButton(bool isDisplay)
    {
        if (isDisplay)
        {
            CoinDisplay.position = defaultCoinDisplayPosition;
            TripleRewardButton.SetActive(true);
        }
        else
        {
            CoinDisplay.position = Holder.position;
            TripleRewardButton.SetActive(false);
        }
    }
    private void SetNextZoneButtonText()
    {
        NextZoneButtonText.text = "Play Zone " + ((int)LevelManager.Instance.GetCurrentnLevel() + 1);
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
    public void OnClickTripleRewardButton()
    {
        //NOTE: gain 2 time the reward coin, the one third is obtain on default
        DataManager.Instance.Coin += numOfCoinReward;

        SetupTripleRewardButton(false);
        SetCoinValue(numOfCoinReward * 3);
    }

    protected override void OnOpenCanvas()
    {
        if (isFisrtLoad)
        {
            defaultCoinDisplayPosition = CoinDisplay.position;

            isFisrtLoad = false;
        }

        UIManager.Instance.CloseUI(UICanvasID.GamePlay);

        LevelManager.Instance.GetLevelResult(out playerRank, out numOfCoinReward, out progressPercentage);

        int curzone = (int)LevelManager.Instance.GetCurrentnLevel();
        SetZoneText(curzone, curzone + 1);

        float ran = Random.Range(0f, 100f);
        if (ran > ConstValues.VALUE_PERCENTAGE_OF_TRIPLE_REWARD)
        {
            SetupTripleRewardButton(true);
        }

        if (playerRank > 1)
        {
            SetActiveResultPanel(false);
            SetUpBottomButtons(false);
            LoseResultHandle();
        }
        else
        {
            SetActiveResultPanel(true);
            SetUpBottomButtons(true);
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
        SetQuoteText(false);
    }
    private void WinResultHandle()
    {
        AudioManager.Instance.PlayAudioClip(AudioType.Win);

        SetProgressBarValue(1f);
        SetCoinValue(numOfCoinReward);
        SetActiveSkullIcon(false);
        SetLockZoneState(false);
        SetNextZoneButtonText();
        SetQuoteText(true);
    }
}
