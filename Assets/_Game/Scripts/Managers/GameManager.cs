using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonMono<GameManager>
{
    [HideInInspector]
    public bool isNextZone;
    public static event Action<GameState> OnGameStateChange;

    private void Start()
    {
        StartCoroutine(DelayChangeGameState(GameState.LoadGame, 0.15f));
        UIManager.Instance.OpenUI(UICanvasID.MainMenu);
    }
    public void ChangeGameState(GameState state)
    {
        switch (state)
        {
            case GameState.LoadGame:
                OnGameStateLoadGame();
                break;
            case GameState.LoadLevel:
                OnGameStateLoadLevel();
                break;
            case GameState.MainMenu:
                OnGameStateMainMenu();
                break;
            case GameState.Playing:
                OnGameStatePlaying();
                break;
            case GameState.Pause:
                OnGameStatePause();
                break;
            case GameState.ResultPhase:
                OnGameStateResultPhase();
                break;
            case GameState.Shopping:
                OnGameStateShopping();
                break;
            default:
                break;
        }

        AutoSetTimeScale(state);

        OnGameStateChange?.Invoke(state);
    }

    private void OnGameStateLoadGame()
    {
        Debug.Log("Load Game State");
        StartCoroutine(DelayChangeGameState(GameState.LoadLevel, 0.15f));
    }
    private void OnGameStateLoadLevel()
    {
        Debug.Log("Load Level State");
        if (isNextZone)
        {
            StartCoroutine(DelayChangeGameState(GameState.Playing, 0.15f));
            isNextZone = false;
        }
        else
        {
            StartCoroutine(DelayChangeGameState(GameState.MainMenu, 0.15f));
        }
    }
    private void OnGameStateMainMenu()
    {
        Debug.Log("Main Menu State");
    }
    private void OnGameStatePlaying()
    {
        Debug.Log("Playing State");
    }
    private void OnGameStatePause()
    {
        Debug.Log("Pause State");
    }
    private void OnGameStateResultPhase()
    {
        Debug.Log("Result Phase State");
        UIManager.Instance.OpenUI(UICanvasID.Result);
    }
    private void OnGameStateShopping()
    {
        Debug.Log("Shopping State");
    }
    private IEnumerator DelayChangeGameState(GameState state, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeGameState(state);
    }
    private void AutoSetTimeScale(GameState state)
    {
        if (state == GameState.Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}

public enum GameState
{
    LoadGame,
    LoadLevel,
    MainMenu,
    Playing,
    Pause,
    ResultPhase,
    Shopping
}


