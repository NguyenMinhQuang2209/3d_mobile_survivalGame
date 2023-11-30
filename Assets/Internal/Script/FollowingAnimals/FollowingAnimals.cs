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
    [SerializeField] private int maxAttackAmount = 1;
    [SerializeField] private float attackRadious = 5f;
    [SerializeField] private float stopDitance = 1f;
    [SerializeField] private float attackDistance = 1f;
    [SerializeField] private bool objectRadiousCircle = false;
    [SerializeField] private bool objectAttackOffsetCircle = false;
    [SerializeField] private float runDistance = 10f;

    [SerializeField] private float stopPlayerDistance = 1f;

    string currentMode = "";

    private Transform player;

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
        float currentSpeed = walkSpeed;
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= stopPlayerDistance)
        {
            agent.SetDestination(transform.position);
            currentSpeed = 0f;
        }
        else
        {
            if (distance >= runDistance)
            {
                currentSpeed = runSpeed;
            }
            agent.SetDestination(player.position);
        }

        agent.speed = currentSpeed;
        animator.SetFloat("Speed", currentSpeed);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (objectRadiousCircle)
        {
            Gizmos.DrawWireSphere(transform.position, stopDitance);
        }

        if (objectAttackOffsetCircle)
        {
            Gizmos.DrawWireSphere(transform.position, stopDitance + attackDistance);
        }
    }
}
