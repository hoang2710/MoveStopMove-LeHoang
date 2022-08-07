using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    public static Vector3 MoveDir;
    [SerializeField]
    private float moveSpeed = 1.5f;

    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        if (MoveDir.sqrMagnitude > 0.01f)
        {
            charaterTrans.position = Vector3.MoveTowards(charaterTrans.position, charaterTrans.position + MoveDir, moveSpeed * Time.deltaTime);
            SetCharacterRotation();
        }
    }
    private void SetCharacterRotation()
    {
        float tmp = Mathf.Atan2(MoveDir.x, MoveDir.z) * Mathf.Rad2Deg;
        charaterTrans.rotation = Quaternion.Lerp(charaterTrans.rotation, Quaternion.Euler(0, tmp, 0), Time.deltaTime * 5);
    }
    protected override void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.LoadGame:
                LoadDataFromPlayerPrefs();
                break;
            default:
                break;
        }
    }
    private void LoadDataFromPlayerPrefs()
    {
        weaponTag = (WeaponType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_TAG);
        weaponSkinTag = (WeaponSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_WEAPON_SKIN_TAG);
        pantSkinTag = (PantSkinType)PlayerPrefs.GetInt(ConstValues.PLAYER_PREFS_ENUM_PANT_SKIN_TAG);
    }
}
