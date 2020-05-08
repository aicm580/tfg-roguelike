using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour
{
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;

    [SerializeField]
    protected int minRarity, maxRarity;
    [SerializeField][Range(0,1)]
    protected float probability; //probabilidad de que spawnee un item al morir
    
    private Animator animator;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        //animator.SetBool("dead", true);
        Destroy(gameObject);
    }

    protected void SpawnItems(float minRandom, int minProbability, int maxProbability)
    {
        float random = Random.Range(0, 1f);
        if (random >= minRandom)
        {
            Item currentItem = ItemsManager.itemsManagerInstance.GetItemByRarity(minProbability, maxProbability);
            ItemOnMap.SpawnItemOnMap(transform.position, currentItem);
        }
    }
}
