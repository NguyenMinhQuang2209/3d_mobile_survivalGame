using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingHittedObject : ObjectHealth
{
    [SerializeField] private float spawnOffset = 0.5f;
    [SerializeField] private float delayDestroy = 1f;
    [SerializeField] private List<CollectingHittedItem> collects = new();

    private Animator animator;

    bool objectDie = false;

    bool collecting = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
        MyInitialized();
    }
    public override bool TakeDamage(int damage)
    {
        if (collecting)
        {
            int remainHealth = GetCurrentHealth();
            if (remainHealth <= damage)
            {
                collecting = false;
                CollectItem();
            }
        }
        bool v = base.TakeDamage(damage);
        return v;
    }
    private void Update()
    {
        if (ObjectDie() && !objectDie)
        {
            objectDie = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            Destroy(gameObject, delayDestroy);
        }
    }
    private void CollectItem()
    {
        List<InventoryItemByName> items = new();
        foreach (CollectingHittedItem item in collects)
        {
            int quantity = item.maxQuantity;
            bool canGet = true;
            if (item.isRareItem)
            {
                quantity = Random.Range(Mathf.Min(item.randomQuantity.x, item.randomQuantity.y), Mathf.Max(item.randomQuantity.x, item.randomQuantity.y) + 1);
                float getRate = Random.Range(0f, 100f);
                if (getRate < item.rate)
                {
                    canGet = false;
                }
            }
            if (canGet && quantity >= 1)
            {
                items.Add(new(item.itemName, quantity));
            }
        }
        BagController.instance.SpawnBag(items, transform.position - Vector3.forward * spawnOffset + Vector3.up * 2f, 0);
    }
}
[System.Serializable]
public class CollectingHittedItem
{
    public ItemName itemName;
    public int maxQuantity = 1;

    [Space(10)]
    [Tooltip("For rare item")]
    [Header("Rare items config")]
    public bool isRareItem = false;
    public Vector2Int randomQuantity = Vector2Int.one;
    public float rate = 100f;
}