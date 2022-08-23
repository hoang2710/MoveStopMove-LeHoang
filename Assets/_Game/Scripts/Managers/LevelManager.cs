using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : SingletonMono<LevelManager>
{
    public float MapSpawnOuterRadius;
    public float MapSpawnInnerRadius;
    public Transform MapSpawnCenter;
    [SerializeField]
    private int numOfBaseBot;
    private int numOfTotalCharacter;
    private int numOfCurrentCharacter;
    private int numOfBotToSpawn;
    private UIGamePlayCanvas gamePlayCanvas;

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
            case GameState.LoadLevel:
                SetData();
                Invoke(nameof(SpawnBaseBot), 0.1f); //NOTE: wait for remain bot to be push to pool --> avoid instantiate more bot, may optimize later 
                // SpawnBaseBot();
                break;
            default:
                break;
        }
    }
    private void SetData()
    {
        numOfTotalCharacter = 50; //temp
        numOfBaseBot = 10;  //temp
        numOfCurrentCharacter = numOfTotalCharacter;
        numOfBotToSpawn = numOfTotalCharacter - numOfBaseBot - 1;//NOTE: minus player
        gamePlayCanvas = UIManager.Instance.GetUICanvas<UIGamePlayCanvas>(UICanvasID.GamePlay);
        gamePlayCanvas.SetPlayerAliveCount(numOfTotalCharacter);
        gamePlayCanvas.Close();
    }
    public void KillHandle()
    {
        numOfCurrentCharacter--;
        gamePlayCanvas.SetPlayerAliveCount(numOfCurrentCharacter);
        if (numOfCurrentCharacter > 1)
        {
            if (numOfBotToSpawn > 0)
            {
                SpawnBotRandomPos();
                numOfBotToSpawn--;
            }
        }
        else
        {
            GameManager.Instance.ChangeGameState(GameState.ResultPhase);
        }
    }
    private void SpawnBaseBot()
    {
        Debug.Log("Spawn Base Bot");
        for (int i = 0; i < numOfBaseBot; i++)
        {
            SpawnBotRandomPos();
        }
    }
    public void SpawnBotRandomPos()
    {
        Vector3 spawnPos;
        if (GetRandomPos(MapSpawnCenter.position, out spawnPos))
        {
            BotPooling.Instance.PopBotFromPool(spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Failed To Spawn New Bot");
        }
    }
    private bool GetRandomPos(Vector3 center, out Vector3 result)
    {
        float minDistSqr = MapSpawnInnerRadius * MapSpawnInnerRadius;
        int numnerOfTries = int.MaxValue;
        for (int i = 0; i < numnerOfTries; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * MapSpawnOuterRadius;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
            {
                if ((hit.position - center).sqrMagnitude > minDistSqr)
                {
                    result = hit.position; Debug.DrawRay(hit.position, Vector3.up * 10f, Color.blue, 5f);
                    return true;
                }
            }
            else
            {
                Debug.DrawRay(randomPoint, Vector3.up * 10f, Color.red, 5f);
            }
        }
        result = Vector3.zero;
        return false;
    }
    public void GetLevelResult(out int rank, out int reward, out float percent)
    {
        rank = GetPlayerRanking();
        reward = GetNumOfCoinReward();
        percent = GetProgressPercentage();
    }
    public int GetPlayerRanking()
    {
        return numOfCurrentCharacter;
    }
    public int GetNumOfCoinReward()
    {
        return (numOfTotalCharacter - numOfCurrentCharacter) * 5; //temp
    }
    public float GetProgressPercentage()
    {
        return (float)(numOfTotalCharacter - numOfCurrentCharacter) / numOfTotalCharacter;
    }
}

