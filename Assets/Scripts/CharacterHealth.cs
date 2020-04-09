using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    protected int initHealth;
    
    protected int currentHealth;

    private void Start()
    {
        currentHealth = initHealth;
    }

    public void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
