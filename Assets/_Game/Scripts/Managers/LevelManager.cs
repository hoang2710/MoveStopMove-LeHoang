using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : SingletonMono<LevelManager>
{
    public float MapSpawnOuterRadius;
    public float MapSpawnInnerRadius;
    public int baseBotToSpawn;
    public Transform MapSpawnCenter;

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
                SpawnBaseBot();
                break;
            default:
                break;
        }
    }
    private void SpawnBaseBot()
    {
        Debug.Log("Spawn Base Bot");
        for (int i = 0; i < baseBotToSpawn; i++)
        {
            Vector3 spawnPos;
            if (GetRandomPos(MapSpawnCenter.position, out spawnPos))
            {
                BotPooling.Instance.PopBotFromPool(spawnPos, Quaternion.identity);
            }
        }
    }
    private bool GetRandomPos(Vector3 center, out Vector3 result)
    {
        float minDistSqr = MapSpawnInnerRadius * MapSpawnInnerRadius;
        int numnerOfTries = 30;
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
}

