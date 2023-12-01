using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AdoptAnimal : ObjectHealth
{
    private Animator animator;
    private NavMeshAgent agent;

    [Header("Default config")]
    [SerializeField] private float walkSpeed = 1f;
    [SerializeField] private float runSpeed = 2f;
    [SerializeField] private Vector2 waitTime = Vector2.one;
    [SerializeField] private Vector2 randomDirectX = Vector2.zero;
    [SerializeField] private Vector2 randomDirectZ = Vector2.zero;
    [SerializeField] private Transform basePositionObject;

    Vector3 basePosition;

    float currentWaitTime = 0f;

    float currentSpeed = 0f;

    private AnimalInteract animalInteract;

    [Space(10)]
    [Header("Growing")]
    [SerializeField] private List<GrowingItem> growingList = new();


    float currentGrowingTime = 0f;
    int currentGrowingState = 0;

    private void Start()
    {
        MyInitialized();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed;
        if (TryGetComponent<NavMeshAgent>(out agent))
        {
            agent.enabled = true;
            agent.speed = currentSpeed;
        }
        animalInteract = GetComponent<AnimalInteract>();

        if (growingList != null)
        {
            currentGrowingTime = growingList[currentGrowingState].growingTime;
            transform.localScale = growingList[currentGrowingState].scale;
            if (animalInteract != null)
            {
                animalInteract.promptMessage = growingList[currentGrowingState].periodName;
            }
        }

        if (basePositionObject == null)
        {
            basePosition = transform.position;
        }
        else
        {
            basePosition = basePositionObject.position;
        }
    }
    private void Update()
    {
        if (animalInteract != null && animalInteract.WasInteracting())
        {
            agent.enabled = false;
            animator.SetFloat("Speed", 0f);
            return;
        }

        currentGrowingTime -= Time.deltaTime;

        if (agent.remainingDistance <= 0.1f)
        {
            currentWaitTime -= Time.deltaTime;
            currentSpeed = 0f;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
        if (currentWaitTime <= 0f)
        {
            PatrolState();
        }
        if (currentGrowingTime <= 0f)
        {
            ChangeSize();
        }
        agent.speed = currentSpeed;
        animator.SetFloat("Speed", currentSpeed);
    }
    public override bool TakeDamage(int damage)
    {
        return base.TakeDamage(damage);
    }
    private void ChangeSize()
    {
        if (growingList == null)
        {
            return;
        }
        currentGrowingState += 1;
        if (currentGrowingState <= growingList.Count - 1)
        {
            currentGrowingTime = growingList[currentGrowingState].growingTime;
            transform.localScale = growingList[currentGrowingState].scale;
            if (animalInteract != null)
            {
                animalInteract.promptMessage = growingList[currentGrowingState].periodName;
            }
        }
        else
        {
            if (animalInteract != null)
            {
                animalInteract.promptMessage = MessageController.COLLECTING_MESSAGE;
                animalInteract.InteractingObject();
            }
        }
    }

    private void PatrolState()
    {
        float randomX = Random.Range(Mathf.Min(randomDirectX.x, randomDirectX.y), Mathf.Max(randomDirectX.x, randomDirectX.y));
        float randomZ = Random.Range(Mathf.Min(randomDirectZ.x, randomDirectZ.y), Mathf.Max(randomDirectZ.x, randomDirectZ.y));

        Vector3 newPo = basePosition + new Vector3(randomX, transform.position.y, randomZ);

        currentWaitTime = Random.Range(Mathf.Min(waitTime.x, waitTime.y), Mathf.Max(waitTime.x, waitTime.y));
        agent.SetDestination(newPo);
    }
    public void RunAway()
    {
        currentSpeed = runSpeed;
    }

}
[System.Serializable]
public class GrowingItem
{
    public Vector3 scale = Vector3.one;
    public float growingTime = 1f;
    public string periodName = "";
}