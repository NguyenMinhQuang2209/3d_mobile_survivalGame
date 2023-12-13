using UnityEngine;

public abstract class ObjectHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth = 0;
    int plusHealthObject = 0;

    [SerializeField] private float txtShowDelay = 1f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    protected void MyInitialized()
    {
        currentHealth = maxHealth + plusHealthObject;
    }
    public bool ObjectDie()
    {
        return currentHealth == 0;
    }
    public bool BaseTakeDamage(int damage)
    {
        return TakeDamage(damage);
    }
    public virtual bool TakeDamage(int damage)
    {
        if (currentHealth == 0)
        {
            return true;
        }
        currentHealth = Mathf.Max(0, currentHealth - damage);
        ViewUIController.instance.ShowTxt(damage.ToString(), transform.position + offset, txtShowDelay);
        return currentHealth == 0;
    }
    public virtual bool TakeDamage(int damage, GameObject hittedBy)
    {
        return TakeDamage(damage);
    }
    public int GetMaxHealth()
    {
        return maxHealth + plusHealthObject;
    }
    protected void ChangePlusHealth(int v)
    {
        plusHealthObject = v;
    }
    public string GetHealthTxt()
    {
        return currentHealth + "/" + (maxHealth + plusHealthObject);
    }
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
