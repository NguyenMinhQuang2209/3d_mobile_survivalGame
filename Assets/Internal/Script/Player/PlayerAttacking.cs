using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacking : MonoBehaviour
{
    [Header("Hand punch config")]
    [SerializeField] private int maxHandAttackingType = 4;
    [SerializeField] private float handTimeBwtAttack = 0f;
    [SerializeField] private float handAttackRadious = 1f;
    [SerializeField] private WeaponConfig handWeaponConfig;

    [Space(10)]
    [Header("Sword attacking config")]
    [SerializeField] private int maxSwordAttackingType = 3;
    [SerializeField] private float swordTimeBwtAttack = 0f;
    [SerializeField] private float swordAttackRadious = 1f;

    int currentMaxAttackingType = 0;
    float currentTimeBwtAttack = 0f;
    float currentTimeBwtAttackDelay = 0f;

    private WeaponType weaponType = WeaponType.Hand;


    Animator animator;

    private WeaponConfig currentWeaponConfig = null;
    [Space(10)]
    [Header("Attack config")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private bool showAttackCircle = false;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private LayerMask treeMask;
    [SerializeField] private LayerMask rockMask;
    float currentAttackRadious = 0f;


    private void Start()
    {
        animator = GetComponent<PlayerMovement>().GetAnimator();

        SwitchAttackingType(WeaponType.Hand);
        currentTimeBwtAttackDelay = currentTimeBwtAttack;
    }

    private void Update()
    {
        currentTimeBwtAttackDelay += Time.deltaTime;
    }
    public void StartAttacking()
    {
        if (currentTimeBwtAttackDelay >= currentTimeBwtAttack)
        {
            currentTimeBwtAttackDelay = 0f;
            int currentAttackingType = Random.Range(0, currentMaxAttackingType);
            if (animator != null)
            {
                animator.SetFloat("AttackIndex", currentAttackingType);
                animator.SetTrigger("Attack");
            }
        }
    }
    public void SwitchAttackingType(WeaponType newWeaponType, float newAttackRadious = 0f, WeaponConfig newWeaponConfig = null)
    {
        weaponType = newWeaponType;

        currentWeaponConfig = newWeaponConfig ?? handWeaponConfig;

        switch (weaponType)
        {
            case WeaponType.Hand:
                currentTimeBwtAttack = handTimeBwtAttack;
                currentMaxAttackingType = maxHandAttackingType;
                currentAttackRadious = handAttackRadious;
                break;
            case WeaponType.Sword:
                currentTimeBwtAttack = swordTimeBwtAttack;
                currentMaxAttackingType = maxSwordAttackingType;
                currentAttackRadious = swordAttackRadious;
                break;
            default:
                currentAttackRadious = newAttackRadious;
                break;
        }
        int weaponTypeValue = (int)weaponType;
        animator.SetInteger("AttackType", weaponTypeValue);
    }
    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPos.position, currentAttackRadious, attackMask);
        foreach (Collider hit in hits)
        {
            if (CheckAttackObject(treeMask, hit, currentWeaponConfig.treeDamage))
            {
                continue;
            }
            if (CheckAttackObject(rockMask, hit, currentWeaponConfig.rockDamage))
            {
                continue;
            }
            CheckAttackObject(attackMask, hit, currentWeaponConfig.enemyDamage);
        }
    }
    private bool CheckAttackObject(LayerMask mask, Collider hit, Vector2Int damage)
    {
        int nextDamage = Random.Range(Mathf.Min(damage.x, damage.y), Mathf.Max(damage.x, damage.y));
        if (((1 << hit.gameObject.layer) & mask) != 0)
        {
            if (hit.gameObject.TryGetComponent<ObjectHealth>(out var objectHealth))
            {
                objectHealth.TakeDamage(nextDamage);
            }
            return true;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (attackPos != null && showAttackCircle)
        {
            Gizmos.DrawWireSphere(attackPos.position, handAttackRadious);
        }
    }
}
[System.Serializable]
public class WeaponConfig
{
    public Vector2Int treeDamage = Vector2Int.one;
    public Vector2Int rockDamage = Vector2Int.one;
    public Vector2Int enemyDamage = Vector2Int.one;
}