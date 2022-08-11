using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterBase : MonoBehaviour
{
    [SerializeField]
    protected WeaponType weaponTag;
    [SerializeField]
    protected WeaponSkinType weaponSkinTag;
    [SerializeField]
    protected PantSkinType pantSkinTag;

    public int Score { get; protected set; }
    public int KillScore { get; protected set; }
    public float AttackRange { get; protected set; }
    public float AttackRate { get; protected set; }

    public Transform charaterTrans;
    public Animator anim;

    public Transform AttackPos;
    public Transform AttackTarget;

    protected Quaternion weaponRotation = Quaternion.Euler(-90f, 0, 90f);
    protected float attackAnimThrow = ConstValues.VALUE_PLAYER_ATTACK_ANIM_THROW_TIME_POINT;
    protected float attackAnimEnd = ConstValues.VALUE_PLAYER_ATTACK_ANIM_END_TIME_POINT;

    public GameObject WeaponPlaceHolder;
    protected GameObject handWeapon;
    protected Quaternion handWeaponRotation = Quaternion.Euler(-90f, -90f, 180f); //TODO: understand why this shit not (90, -90, 180)

    public Renderer PantRenderer;

    private void Awake()
    {
        Score = 0;
        KillScore = 0;
        AttackRange = ConstValues.VALUE_BASE_ATTACK_RANGE;
        AttackRate = ConstValues.VALUE_BASE_ATTACK_RATE;
    }
    protected virtual void Start()
    {
        GameManager.OnGameStateChange += GameManagerOnGameStateChange;
    }
    protected virtual void OnDestroy()
    {
        GameManager.OnGameStateChange -= GameManagerOnGameStateChange;
    }
    protected virtual void GameManagerOnGameStateChange(GameState state)
    {

    }
    protected void SetUpHandWeapon()
    {
        if (handWeapon != null)
        {
            Destroy(handWeapon);
        }

        anim.Play(ConstValues.ANIM_PLAY_DEFAULT_IDLE); //NOTE: make sure character model is in right position for assign hand weapon

        Transform WeaponPlaceHolderTrans = WeaponPlaceHolder.transform;
        handWeapon = Instantiate(ItemStorage.Instance.GetWeaponType(weaponTag), 
                                WeaponPlaceHolderTrans.position, 
                                Quaternion.LookRotation(WeaponPlaceHolderTrans.forward) * handWeaponRotation, 
                                WeaponPlaceHolderTrans);

        Weapon weapon = handWeapon.GetComponent<Weapon>();
        weapon?.DeactiveWeaponScript();

        Renderer objRen = handWeapon.GetComponent<Renderer>();

        if (objRen != null)
        {
            switch (weaponTag)
            {
                case WeaponType.Candy:
                    Material materialCandy = ItemStorage.Instance.GetWeaponSkin(weaponSkinTag);
                    objRen.materials = new Material[] {materialCandy, materialCandy, materialCandy}; //NOTE: Candy weapon have 3 material
                    break;
                default:
                    Material material = ItemStorage.Instance.GetWeaponSkin(weaponSkinTag);
                    objRen.materials = new Material[] { material, material };
                    break;
            }
        }
    }
    protected void SetUpPantSkin()
    {
        PantRenderer.material = ItemStorage.Instance.GetPantSkin(pantSkinTag);
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
