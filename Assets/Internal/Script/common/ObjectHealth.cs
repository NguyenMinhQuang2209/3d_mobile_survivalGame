using UnityEngine;

public abstract class ObjectHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    int currentHealth = 0;
    int plusHealthObject = 0;
    private void Start()
    {
        currentHealth = maxHealth + plusHealthObject;
    }
    public bool BaseTakeDamage(int damage)
    {
        return TakeDamage(damage);
    }
    public virtual bool TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth + plusHealthObject - damage);
        return currentHealth == 0;
    }
    public int GetMaxHealth()
    {
        return maxHealth + plusHealthObject;
    }
    protected void ChangePlusHealth(int v)
    {
        plusHealthObject = v;
    }
}
