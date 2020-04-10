using UnityEngine;

public enum DamageOrigin
{
    Player, NormalEnemy, Boss, Obstacle, Item,
}

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

    public virtual void TakeDamage(int dmgAmount, DamageOrigin dmgOrigin)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Die(dmgOrigin);
        }
    }

    protected virtual void Die(DamageOrigin dmgOrigin)
    {
        animator.SetBool("dead", true);
        Debug.Log("KILLER: " + dmgOrigin);
    }
}
