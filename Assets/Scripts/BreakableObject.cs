using System.Collections;
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

                break;
            case BreakableObjectType.EnemyContainer:
                int randomEnemy = Random.Range(0, enemiesGenerator.enemiesPrefabs.Count); 
                GameObject enemy = Instantiate(enemiesGenerator.enemiesPrefabs[randomEnemy], transform.position, Quaternion.identity) as GameObject;
                enemy.transform.parent = enemiesGenerator.enemiesHolder.transform;
                enemiesGenerator.enemies.Add(enemy);
                break;
            case BreakableObjectType.ItemContainer:

                break;
        }
        base.Die();
    }
}
