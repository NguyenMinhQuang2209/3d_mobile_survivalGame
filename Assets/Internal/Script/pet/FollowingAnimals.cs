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
    private float currentTimeBwtAttack = 0f;
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

    private int plusHealth = 0;
    private float plusDamage = 0f;
    private float plusSpeed = 0f;
    private float plusTimeBwtAttack = 0f;
    private int currentGrowingProgess = 0;
    private Transform player;
    private Transform target = null;


    [Header("Not pet state")]
    private bool wasPet = false;
    [SerializeField] private Vector2 patrolRandomX;
    [SerializeField] private Vector2 patrolRandomZ;
    [SerializeField] private float patrolWaitTime = 2f;
    float currentPatrolWaitTime = 0f;

    private void Start()
    {
        MyInitialized();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();


        currentMode = MessageController.PROTECT;

        agent.speed = walkSpeed;

        player = GameObject.FindGameObjectWithTag(TagController.PLAYER_TAG).transform;
    }
    private void Update()
    {
        float currentSpeed = walkSpeed + plusSpeed;
        float speedAnimator = walkSpeed;
        if (!wasPet)
        {
            if (agent.remainingDistance <= 0.1f)
            {
                currentSpeed = 0f;
                speedAnimator = 0f;
                PatrolState();
            }
        }
        else
        {
            ChangePlusHealth(plusHealth);
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
        }
        agent.speed = currentSpeed;
        animator.SetFloat("Speed", speedAnimator);

    }

    private void PatrolState()
    {
        currentPatrolWaitTime += Time.deltaTime;
        if (currentPatrolWaitTime >= patrolWaitTime)
        {
            currentPatrolWaitTime = 0f;
            float ranX = Random.Range(Mathf.Min(patrolRandomX.x, patrolRandomX.y), Mathf.Max(patrolRandomX.x, patrolRandomX.y));
            float ranZ = Random.Range(Mathf.Min(patrolRandomZ.x, patrolRandomZ.y), Mathf.Max(patrolRandomZ.x, patrolRandomZ.y));
            Vector3 target = transform.position + new Vector3(ranX, 0f, ranZ);
            agent.SetDestination(target);
        }
    }
    public string GetCurrentMode()
    {
        return currentMode == MessageController.PROTECT ? "Phòng thủ" : "Tấn công";
    }
    public string GetNextMode()
    {
        return currentMode != MessageController.PROTECT ? "Phòng thủ" : "Tấn công";
    }
    public void ChangeMode()
    {
        currentMode = currentMode == MessageController.PROTECT ? MessageController.ATTACK : MessageController.PROTECT;
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
    public bool UpgradeLevel()
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
                return true;
            }
            else
            {
                LogController.instance.Log(MessageController.LACK_OF_COIN);
            }
        }
        return false;
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
    public void WasPet(bool v)
    {
        wasPet = v;
    }
    public string GetNextPrice()
    {
        return currentGrowingProgess <= growings.Count - 1 ? growings[currentGrowingProgess].price + " coins" : "";
    }
}
[System.Serializable]
public class FollowingAnimalGrowing
{
    public int price = 1;
    public GrowingType growingType;
    public float growingValue = 0f;
}