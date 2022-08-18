using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : SingletonMono<GameManager>
{
    public static event Action<GameState> OnGameStateChange;

    private void Start()
    {
        StartCoroutine(DelayChangeGameState(GameState.LoadGame, 0.15f));
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
        StartCoroutine(DelayChangeGameState(GameState.MainMenu, 0.15f));
    }
    private void OnGameStateMainMenu()
    {
        Debug.Log("Main Menu State");
    }
    private void OnGameStatePlaying()
    {
        Debug.Log("Playing State");
        Time.timeScale = 1;
    }
    private void OnGameStatePause()
    {
        Debug.Log("Pause State");
        Time.timeScale = 0;
    }
    private void OnGameStateResultPhase()
    {
        Debug.Log("Result Phase State");
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


