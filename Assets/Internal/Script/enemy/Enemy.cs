
using UnityEngine;
using UnityEngine.AI;

public class Enemy : ObjectHealth
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Default config")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 1f;
    float currentSpeed = 0f;


    [Space(10)]
    [Header("Patrol state")]
    [SerializeField] private Vector2 ranXPos = Vector2.zero;
    [SerializeField] private Vector2 ranZPos = Vector2.zero;
    [SerializeField] private bool patrolInSamePlace = false;
    [SerializeField] private float waitTime = 2f;
    float currentWaitTime = 0f;
    Vector3 defaultPlace;

    [Space(10)]
    [Header("Attack State")]
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadious = 1f;
    [SerializeField] private bool showAttackCircle = false;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private float attackStopDistance = 0.1f;
    [SerializeField] private float timeBwtAttack = 1f;
    [SerializeField] private LayerMask attackMask;
    [SerializeField] private bool attackByCustomFrame = true;

    [Tooltip("Only for attackByCustomerFrame = true")]
    [SerializeField] private float attackInFrame = 1f;
    float currentTimeBwtAttack = 0f;
    float currentFrame = 0f;
    Transform target = null;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        defaultPlace = transform.position;
    }
    private void Update()
    {
        currentSpeed = walkSpeed;
        if (target != null && enemyType != EnemyType.No_Attack)
        {
            AttackState();
        }
        else
        {
            if (agent.remainingDistance <= 0.1f)
            {
                currentSpeed = 0f;
                currentWaitTime += Time.deltaTime;
                if (currentWaitTime >= waitTime)
                {
                    PatrolState();
                }
            }
        }
        agent.speed = currentSpeed;
        if (animator != null)
        {
            animator.SetFloat("Speed", currentSpeed);
        }
    }

    private void PatrolState()
    {

        currentWaitTime = 0f;
        float ranX = Random.Range(Mathf.Min(ranXPos.x, ranXPos.y), Mathf.Max(ranXPos.x, ranXPos.y));
        float ranZ = Random.Range(Mathf.Min(ranZPos.x, ranZPos.y), Mathf.Max(ranZPos.x, ranZPos.y));
        Vector3 newPos;
        if (patrolInSamePlace)
        {
            newPos = defaultPlace + new Vector3(ranX, 0f, ranZ);
        }
        else
        {
            newPos = transform.position + new Vector3(ranX, 0f, ranZ);
        }
        agent.SetDestination(newPos);
    }
    private void AttackState()
    {
        currentTimeBwtAttack += Time.deltaTime;
        if (currentTimeBwtAttack >= timeBwtAttack)
        {
            currentTimeBwtAttack = 0f;
            currentFrame += Time.deltaTime;
            if (attackByCustomFrame && currentFrame >= attackInFrame)
            {
                currentFrame = 0f;
                Attack();
            }
        }

        if (agent.remainingDistance <= attackStopDistance)
        {
            currentSpeed = 0f;
            agent.SetDestination(transform.position);
        }
        else
        {
            currentSpeed = runSpeed;
            agent.SetDestination(target.position);
        }
    }
    public void Attack()
    {
        if (attackPos != null)
        {
            Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, attackMask);
            foreach (Collider hit in hits)
            {
                if (hit.TryGetComponent<ObjectHealth>(out var health))
                {
                    bool objectDie = health.TakeDamage(damage);
                    if (objectDie)
                    {
                        target = null;
                    }
                }
            }
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (attackPos != null && showAttackCircle)
        {
            Gizmos.DrawWireSphere(attackPos.position, attackRadious);
        }
    }
}
