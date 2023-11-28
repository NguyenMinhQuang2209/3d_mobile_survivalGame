using UnityEngine;

public abstract class ObjectHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    int currentHealth = 0;
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public bool BaseTakeDamage(int damage)
    {
        return TakeDamage(damage);
    }
    public virtual bool TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        return currentHealth == 0;
    }
}
