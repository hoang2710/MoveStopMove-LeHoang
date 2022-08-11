using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : CharacterBase, IPoolCharacter
{

    public void OnInit()
    {
        SetRandomEnumData();
        ResetScore();
    }
    public void OnDespawn()
    {
        throw new System.NotImplementedException();
    }
    private void SetRandomEnumData()
    {
        weaponTag = (WeaponType)Random.Range(0, System.Enum.GetNames(typeof(WeaponType)).Length);
        weaponSkinTag = (WeaponSkinType)Random.Range(0, System.Enum.GetNames(typeof(WeaponSkinType)).Length);
        pantSkinTag = (PantSkinType)Random.Range(0, System.Enum.GetNames(typeof(PantSkinType)).Length);
    }
    private void ResetScore()
    {
        Score = 0;
        KillScore = 0;
    }
}
