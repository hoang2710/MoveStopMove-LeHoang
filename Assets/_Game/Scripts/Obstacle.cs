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

        defaultMat = ItemStorage.Instance.ObstacleMaterial[0];
        transMat = ItemStorage.Instance.ObstacleMaterial[1];

        ObstacleRenderer.material = defaultMat;

        DetectRangeTrans.localScale = Vector3.one * (ConstValues.VALUE_BASE_ATTACK_RANGE / ObstacleTrans.localScale.x + ObstacleDistOffset); //NOTE: or y or z will work
    }
    private void OnDestroy()
    {
        Player.OnPlayerSizeUp -= PlayerOnPlayerSizeUp;
    }
    private void PlayerOnPlayerSizeUp(Player player)
    {
        DetectRangeTrans.localScale = player.CharaterTrans.localScale * (ConstValues.VALUE_BASE_ATTACK_RANGE / ObstacleTrans.localScale.x + ObstacleDistOffset); //NOTE: or y or z will work
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ConstValues.TAG_PLAYER))
        {
            ChangeMat(transMat);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ConstValues.TAG_PLAYER))
        {
            ChangeMat(defaultMat);
        }
    }
    private void ChangeMat(Material mat)
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
        ItemStorage.Instance.PushWeaponToPool(weapon.WeaponTag, weapon.WeaponObject);
    }
}
