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
    private void Start()
    {
        animator = GetComponent<Animator>();
        MyInitialized();
    }
    public override bool TakeDamage(int damage)
    {
        bool v = base.TakeDamage(damage);
        if (!ObjectDie())
        {
            CollectItem();
        }
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
                Destroy(gameObject, delayDestroy);
            }
        }
    }
    private void CollectItem()
    {
        List<InventoryItemByName> items = new();
        foreach (CollectingHittedItem item in collects)
        {
            int quantity = Random.Range(Mathf.Min(item.quantity.x, item.quantity.y), Mathf.Max(item.quantity.x, item.quantity.y) + 1);
            bool canGet = true;
            if (item.useRandomRate)
            {
                float getRate = Random.Range(0f, 100f);
                if (getRate < item.rate)
                {
                    canGet = false;
                }
            }
            if (canGet)
            {
                items.Add(new(item.itemName, quantity));
            }
        }
        if (items != null)
        {
            BagController.instance.SpawnBag(items, transform.position - Vector3.forward * spawnOffset);
        }
    }
}
[System.Serializable]
public class CollectingHittedItem
{
    public ItemName itemName;
    public Vector2Int quantity = Vector2Int.zero;
    public bool useRandomRate = false;
    public float rate = 100f;
}