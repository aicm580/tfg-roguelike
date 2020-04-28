﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BreakableObjectType
{
    Explosive, EnemyContainer, ItemContainer, 
}

public class BreakableObject : CharacterHealth
{
    [SerializeField]
    protected BreakableObjectType objectType;
    
    private EnemiesGenerator enemiesGenerator;

    private void Awake()
    {
        enemiesGenerator = GameManager.instance.GetComponent<EnemiesGenerator>();
    }

    protected override void Die()
    {
        switch (objectType)
        {
            case BreakableObjectType.Explosive:
                bool playerHit = false; //el player tiene 2 colliders, pero solo queremos dañarlo 1 vez, por lo que utilizamos un bool para saber si ya se le ha dañado
                Collider2D[] charactersInRange = Physics2D.OverlapCircleAll(transform.position, 2.5f);

                foreach (Collider2D col in charactersInRange)
                {
                    Debug.Log(col.gameObject.name);
                    if (col.GetComponent<PlayerHealth>() != null && !playerHit)
                    {
                        playerHit = true; 
                        col.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.Item);
                    } 
                    else if (col.GetComponent<NormalEnemyHealth>() != null)
                    {
                        col.GetComponent<NormalEnemyHealth>().TakeDamage(10);
                    }
                }
                break;

            case BreakableObjectType.EnemyContainer:
                int randomEnemy = Random.Range(0, enemiesGenerator.enemiesPrefabs.Count); 
                GameObject enemy = Instantiate(enemiesGenerator.enemiesPrefabs[randomEnemy], transform.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = enemiesGenerator.enemiesHolder.transform;
                enemiesGenerator.enemies.Add(enemy);
                break;

            case BreakableObjectType.ItemContainer:
                Item currentItem = ItemsManager.itemsManagerInstance.GetItemByProbability(6, 9);
                ItemOnMap.SpawnItemOnMap(transform.position, currentItem);
                break;
        }

        base.Die();
    }
}