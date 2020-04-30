using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterHealth : MonoBehaviour
{
    [SerializeField]
    protected int initHealth;
    [SerializeField]
    protected int minRarity, maxRarity;

    private int currentHealth;
    private Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth = initHealth;
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
