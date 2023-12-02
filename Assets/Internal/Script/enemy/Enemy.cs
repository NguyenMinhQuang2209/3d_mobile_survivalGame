
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.Progress;

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

    [Space(20)]
    [Header("See enemy config")]
    [SerializeField] private float sawAngle = 80f;
    [SerializeField] private float sawDistance = 10f;
    [SerializeField] private float chaseMaxDistance = 20f;

    [Space(10)]
    [Header("Rotate when catch")]
    [SerializeField] private float rotateAngle = 10f;
    [SerializeField] private float rotateSpeed = 10f;
    Transform player = null;
    GameObject[] pets = null;

    bool attacking = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag(TagController.PLAYER_TAG).transform;
        pets = GameObject.FindGameObjectsWithTag(TagController.PET_TAG);

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
            if (enemyType == EnemyType.Attack_When_See)
            {
                SeeEnemies(player);
                if (target != null)
                {
                    foreach (GameObject pet in pets)
                    {
                        SeeEnemies(pet.transform);
                    }
                }
            }
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
        if (attacking)
        {
            currentFrame += Time.deltaTime;
            if (attackByCustomFrame && currentFrame >= attackInFrame)
            {
                currentFrame = 0f;
                attacking = false;
                Attack();
            }
        }
        currentTimeBwtAttack += Time.deltaTime;
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= attackStopDistance)
        {
            currentSpeed = 0f;
            agent.SetDestination(transform.position);
            Vector3 directionToItem = (target.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToItem);
            if (angle > rotateAngle)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(directionToItem, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
            }

            if (currentTimeBwtAttack >= timeBwtAttack)
            {
                animator.SetTrigger("Attack");
                currentTimeBwtAttack = 0f;
                attacking = true;
            }
        }
        else
        {
            currentSpeed = runSpeed;
            agent.SetDestination(target.position);
            if (distance >= chaseMaxDistance)
            {
                target = null;
            }
        }
    }
    public override bool TakeDamage(int damage, GameObject hittedBy)
    {
        bool v = base.TakeDamage(damage, hittedBy);
        if (enemyType != EnemyType.No_Attack)
        {
            if (target == null)
            {
                target = hittedBy.transform;
            }
        }
        return v;
    }
    public void Attack()
    {
        if (attackPos != null)
        {
            Collider[] hits = Physics.OverlapSphere(attackPos.position, attackRadious, attackMask);

            List<GameObject> wasHitted = new();
            foreach (Collider hit in hits)
            {
                if (wasHitted.Contains(hit.gameObject))
                {
                    continue;
                }
                wasHitted.Add(hit.gameObject);

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
    private void SeeEnemies(Transform item)
    {
        float distance = Vector3.Distance(transform.position, item.position);
        if (distance <= sawDistance)
        {
            Vector3 directionToItem = (item.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToItem);
            if (angle <= sawAngle && target == null)
            {
                target = item;
            }
        }
    }
}
