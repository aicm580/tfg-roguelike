using UnityEngine;

public enum BreakableObjectType
{
    Explosive, EnemyContainer, ItemContainer, 
}

public class BreakableObject : CharacterHealth
{
    [SerializeField]
    protected string breakableObjectName;
    [SerializeField]
    protected BreakableObjectType objectType;
    [SerializeField]
    protected GameObject explosionPrefab;
    
    private EnemiesGenerator enemiesGenerator;

    private void Awake()
    {
        enemiesGenerator = GameManager.instance.GetComponent<EnemiesGenerator>();
    }

    protected override void Start()
    {
        base.Start();
        charName = breakableObjectName;
    }

    protected override void Die()
    {
        switch (objectType)
        {
            case BreakableObjectType.Explosive:
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                float destroyTime = explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
                Destroy(explosion, destroyTime);

                Collider2D[] charactersInRange = Physics2D.OverlapCircleAll(transform.position, 1.75f);

                foreach (Collider2D col in charactersInRange)
                {
                    Debug.Log(col.gameObject.name);
                    if (col.GetComponent<PlayerHealth>() != null)
                    {
                        col.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.Obstacle, breakableObjectName);
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
                SpawnItems(1f - probability, 1, 3);
                break;
        }

        base.Die();
    }
}
