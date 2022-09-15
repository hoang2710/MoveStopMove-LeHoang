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
    public GameObject RevivePanel; //NOTE: Must assign block raycast panel with revive panel inside
    public TMP_Text CounterNumberText;
    public int SecondToCountDown = 5;
    private IEnumerator coroutine;
    public TMP_Text CoinText;
    public int NumOfCoinToRevive = 100;

    private void Start()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    private void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.ReviveOption:
                RevivePanel.SetActive(true);
                coroutine = CountDown(SecondToCountDown);
                StartCoroutine(coroutine);
                break;
            default:
                break;
        }
    }
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
    public void OnClickRevivePanelExitButton()
    {
        StopCountDown();
        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    public void OnClickBuyWithCoinButton()
    {
        if (DataManager.Instance.Coin >= NumOfCoinToRevive)
        {
            StopCountDown();
            DataManager.Instance.Coin -= NumOfCoinToRevive;
            GameManager.Instance.ChangeGameState(GameState.Playing);
            RevivePanel.SetActive(false);
        }
    }
    public void OnClickBuyWithAdsButton()
    {
        StopCountDown();
        GameManager.Instance.ChangeGameState(GameState.Playing);
        RevivePanel.SetActive(false);
    }

    protected override void OnOpenCanvas()
    {
        SetActiveTipPanel(true);

        RevivePanel.SetActive(false);
        CoinText.text = NumOfCoinToRevive.ToString();
    }
    protected override void OnCloseCanvas()
    {
        JoystickInput.SetBackState();

        StopCountDown();
    }

    public IEnumerator CountDown(int second)
    {
        for (int i = second; i >= 0; i--)
        {
            AudioManager.Instance.PlayAudioClip(AudioType.CountDown);
            CounterNumberText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.ChangeGameState(GameState.ResultPhase);
    }
    private void StopCountDown()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
