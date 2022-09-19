using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IHit
{
    private float targetTime;
    public Transform ObstacleTrans;
    public Transform DetectRangeTrans;
    public Renderer ObstacleRenderer;
    private Material defaultMat;
    private Material transMat;
    [SerializeField] private float ObstacleDistOffset = 0.1f;

    private void Start()
    {
        Player.OnPlayerSizeUp += PlayerOnPlayerSizeUp;
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;

        defaultMat = ItemStorage.Instance.GetObstacleMaterial(0);
        transMat = ItemStorage.Instance.GetObstacleMaterial(1);

        ObstacleRenderer.material = defaultMat;

        DetectRangeTrans.localScale = Vector3.one * (ConstValues.VALUE_BASE_ATTACK_RANGE / ObstacleTrans.localScale.x + ObstacleDistOffset); //NOTE: or y or z will work
    }
    private void OnDestroy()
    {
        Player.OnPlayerSizeUp -= PlayerOnPlayerSizeUp;
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    private void PlayerOnPlayerSizeUp(Player player)
    {
        DetectRangeTrans.localScale = player.CharaterTrans.localScale * (ConstValues.VALUE_BASE_ATTACK_RANGE / ObstacleTrans.localScale.x + ObstacleDistOffset); //NOTE: or y or z will work
    }
    private void GameManagerOnGameStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.LoadLevel:
                SetMat(defaultMat);
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstValues.TAG_PLAYER))
        {
            SetMat(transMat);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ConstValues.TAG_PLAYER))
        {
            SetMat(defaultMat);
        }
    }
    private void SetMat(Material mat)
    {
        ObstacleRenderer.material = mat;
    }
    public void OnHit(CharacterBase bulletOwner, Weapon weapon)
    {
        targetTime = weapon.GetRemainLifeTime();
        weapon.enabled = false;
        weapon.WeaponCollider.enabled = false;

        StartCoroutine(FreezeWeapon(targetTime, weapon));
    }

    public IEnumerator FreezeWeapon(float duration, Weapon weapon)
    {
        yield return new WaitForSeconds(duration);
        ItemStorage.Instance.PushWeaponToPool(weapon);
    }
}
