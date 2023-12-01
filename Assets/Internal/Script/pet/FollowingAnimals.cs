using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowingAnimals : ObjectHealth
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Default config")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private float stopDitance = 1f;
    [SerializeField] private float runDistance = 10f;
    [SerializeField] private float stopPlayerDistance = 1f;


    [Space(10)]
    [Header("Attack config")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private int maxAttackAmount = 1;

    [Header("Attack Advance")]
    [SerializeField] private float timeBwtAttack = 1f;
    float currentTimeBwtAttack = 0f;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackPosRadious = 1f;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private bool showAttackCircle = false;

    [SerializeField] private bool useAttackInFirstFrame = true;

    [Header("Pet config")]
    public string petName = "";
    string currentMode = "";
    public Sprite petSprite;

    [Space(20)]
    [Header("Upgrade level")]
    [SerializeField] private List<FollowingAnimalGrowing> growings = new();

    int plusHealth = 0;
    float plusDamage = 0f;
    float plusSpeed = 0f;
    float plusTimeBwtAttack = 0f;

    int currentGrowingProgess = 0;

    private Transform player;

    Transform target = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        currentMode = MessageController.PROTECT;

        agent.speed = walkSpeed;

        player = GameObject.FindGameObjectWithTag(TagController.PLAYER_TAG).transform;
    }
    private void Update()
    {
        ChangePlusHealth(plusHealth);
        float currentSpeed = walkSpeed + plusSpeed;
        float speedAnimator = walkSpeed;
        currentTimeBwtAttack += Time.deltaTime;
        if (target != null)
        {
            if (agent.remainingDistance <= stopDitance)
            {
                currentSpeed = 0f;
                speedAnimator = 0f;
                if (currentTimeBwtAttack >= timeBwtAttack - plusTimeBwtAttack)
                {
                    BaseAttacking();
                }
            }
            else
            {
                currentSpeed = runSpeed + plusSpeed;
                speedAnimator = runSpeed;
                agent.SetDestination(target.position);
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance <= stopPlayerDistance)
            {
                agent.SetDestination(transform.position);
                currentSpeed = 0f;
                speedAnimator = 0f;
            }
            else
            {
                if (distance >= runDistance)
                {
                    currentSpeed = runSpeed + plusSpeed;
                    speedAnimator = runSpeed;
                }
                agent.SetDestination(player.position);
            }

        }
        agent.speed = currentSpeed;
        animator.SetFloat("Speed", speedAnimator);

    }
    public string GetCurrentMode()
    {
        return currentMode;
    }
    private void BaseAttacking()
    {
        currentTimeBwtAttack = 0f;
        int next = Random.Range(1, maxAttackAmount + 1);
        animator.SetTrigger("Attack");
        animator.SetFloat("AttackIndex", next);

        if (useAttackInFirstFrame)
        {
            Attack();
        }
    }
    public void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(attackPos.position, attackPosRadious, attackMask);
        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<ObjectHealth>(out var objectHealth))
            {
                bool objectDie = objectHealth.TakeDamage((int)(damage + plusDamage));
                if (objectDie)
                {
                    target = null;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (showAttackCircle)
        {
            Gizmos.DrawWireSphere(attackPos.position, attackPosRadious);
        }

    }
    public FollowingAnimalGrowing NextProgress()
    {
        if (growings != null)
        {
            return currentGrowingProgess < growings.Count ? growings[currentGrowingProgess] : null;
        }
        return null;
    }
    public void UpgradeLevel()
    {
        FollowingAnimalGrowing nextProgress = NextProgress();
        if (nextProgress != null)
        {
            int price = nextProgress.price;

            bool enoughCoin = CoinController.instance.RemoveCoin(price);
            if (enoughCoin)
            {
                currentGrowingProgess += 1;
                UpdatePlusValue();
            }
            else
            {
                LogController.instance.Log(MessageController.LACK_OF_COIN);
            }

        }
    }
    private void UpdatePlusValue()
    {
        if (growings == null && currentGrowingProgess == 0)
            return;

        plusHealth = 0;
        plusDamage = 0f;
        plusSpeed = 0f;
        plusTimeBwtAttack = 0f;
        for (int i = 0; i < currentGrowingProgess; i++)
        {
            FollowingAnimalGrowing cur = growings[i];
            switch (cur.growingType)
            {
                case GrowingType.Health:
                    plusHealth += (int)cur.growingValue;
                    break;
                case GrowingType.Speed:
                    plusSpeed += cur.growingValue;
                    break;
                case GrowingType.Damage:
                    plusDamage += cur.growingValue;
                    break;
                case GrowingType.TimeBwtAttack:
                    plusTimeBwtAttack += cur.growingValue;
                    break;
            }
        }
    }

    public int GetPetLevel()
    {
        return currentGrowingProgess <= growings.Count - 1 ? currentGrowingProgess : -1;
    }

    public float GetDamage()
    {
        return damage + plusDamage;
    }
    public float GetSpeed()
    {
        return walkSpeed + plusSpeed;
    }
    public float GetTimeBwtAttack()
    {
        return timeBwtAttack + plusTimeBwtAttack;
    }
}
[System.Serializable]
public class FollowingAnimalGrowing
{
    public int price = 1;
    public GrowingType growingType;
    public float growingValue = 0f;
}