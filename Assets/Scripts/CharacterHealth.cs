using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    protected int initHealth;
    
    protected int currentHealth;
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = initHealth;
    }

    protected virtual void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        animator.SetBool("dead", true);
        Destroy(gameObject);
    }
}
