using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    protected WeaponType weaponTag;
    [SerializeField]
    protected WeaponSkinType weaponSkinTag;
    [SerializeField]
    protected PantSkinType pantSkinTag;

    [SerializeField]
    protected int score = 0;
    [SerializeField]
    protected int killScore = 0;
    [SerializeField]
    protected float attackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
    [SerializeField]
    protected float attackRate = ConstValues.VALUE_BASE_ATTACK_RATE;

    public int Score
    {
        get
        {
            return score;
        }
    }
    public int KillScore
    {
        get
        {
            return killScore;
        }
    }

    [SerializeField]
    protected Transform charaterTrans;

    private void Start()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    protected virtual void GameManagerOnGameStateChange(GameState state)
    {

    }
    public void SetWeaponType(WeaponType tag)
    {
        weaponTag = tag;
    }
    public void SetWeaponSkin(WeaponSkinType tag)
    {
        weaponSkinTag = tag;
    }
    public void SetPantSkin(PantSkinType tag)
    {
        pantSkinTag = tag;
    }

}
