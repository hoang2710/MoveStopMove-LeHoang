using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GameManager : SingletonMono<GameManager>
{
    private bool isNextZone;
    private bool isFirstLoad = true;
    public static event Action<GameState> OnGameStateChange;
    public GameState CurrentGameState { get; private set; } = GameState.InitState;
    public GameState PrevGameState { get; private set; }

    private void Start()
    {
        StartCoroutine(DelayChangeGameState(GameState.LoadGame, 5));
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
            case GameState.WeaponShop:
                OnGameStateWeaponShop();
                break;
            case GameState.SkinShop:
                OnGameStateSkinShop();
                break;
            case GameState.ReviveOption:
                OnGameStateReviveOption();
                break;
            default:
                break;
        }

        AutoSetTimeScale(state);
        PrevGameState = CurrentGameState;
        CurrentGameState = state;

        OnGameStateChange?.Invoke(state);
    }

    private void OnGameStateLoadGame()
    {
        Debug.Log("Load Game State");
        DataManager.Instance.LoadGame();
        StartCoroutine(DelayChangeGameState(GameState.LoadLevel, 5));
    }
    private void OnGameStateLoadLevel()
    {
        Debug.Log("Load Level State");
        if (isNextZone)
        {
            StartCoroutine(DelayChangeGameState(GameState.Playing, 5));
            isNextZone = false;
        }
        else
        {
            StartCoroutine(DelayChangeGameState(GameState.MainMenu, 5));
        }
    }
    private void OnGameStateMainMenu()
    {
        Debug.Log("Main Menu State");
        if (isFirstLoad)
        {
            UIManager.Instance.OpenUI(UICanvasID.MainMenu);
            isFirstLoad = false;
        }
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
    private void OnGameStateWeaponShop()
    {
        Debug.Log("Weapon Shop State");
    }
    private void OnGameStateSkinShop()
    {
        Debug.Log("Skin Shop State");
    }
    private void OnGameStateReviveOption()
    {
        Debug.Log("Revive Option State");
    }
    private IEnumerator DelayChangeGameState(GameState state, float time)
    {
        yield return new WaitForSeconds(time);
        ChangeGameState(state);
    }
    private IEnumerator DelayChangeGameState(GameState state, int numOfFrame)
    {
        for (int i = 0; i < numOfFrame; i++)
        {
            yield return null;
        }
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
    public void SetBoolIsNextZone(bool isNextZone)
    {
        this.isNextZone = isNextZone;
    }
    public void ResetPLayerCoinValue()
    {
        DataManager.Instance.Coin = 0;
    }
    public void ResetPlayerEXP()
    {
        DataManager.Instance.PlayerExp = 0f;
    }
    public void ResetGameData()
    {
        DataManager.Instance.DeleteData();
    }
    public void AddCoin()
    {
        DataManager.Instance.Coin += 200;
    }
    public void MinusCoin()
    {
        DataManager.Instance.Coin -= 200;
    }
    public void AddEXP()
    {
        DataManager.Instance.PlayerExp += 200f;
    }
}

public enum GameState
{
    InitState,
    LoadGame,
    LoadLevel,
    MainMenu,
    Playing,
    Pause,
    ResultPhase,
    WeaponShop,
    SkinShop,
    ReviveOption
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var script = (GameManager)target;

        if (GUILayout.Button("Reset Player Coin"))
        {
            script.ResetPLayerCoinValue();
        }
        if (GUILayout.Button("Reset Exp"))
        {
            script.ResetPlayerEXP();
        }
        if (GUILayout.Button("Reset Game Data"))
        {
            script.ResetGameData();
        }
        if (GUILayout.Button("Add more coin"))
        {
            script.AddCoin();
        }
        if (GUILayout.Button("Minus coin"))
        {
            script.MinusCoin();
        }
        if (GUILayout.Button("Add EXP"))
        {
            script.AddEXP();
        }
    }
}
#endif


