using UnityEngine;

public class BossHealth : CharacterHealth
{
    [SerializeField]
    protected GameObject fuel;
    [HideInInspector]
    public HealthBar healthBar;
    
    private GameObject enemiesHolder;
    private string enemyName;

    protected override void Start()
    {
        base.Start();
        enemiesHolder = GameObject.Find("EnemiesHolder");
        enemyName = GetComponent<Enemy>().enemyName;
        healthBar = GameManager.instance.bossHealthBar;
        healthBar.DisableHealthBar();
        healthBar.SetMaxHealth(maxHealth);
    }
    
    public override void TakeDamage(int dmgAmount)
    {
        base.TakeDamage(dmgAmount);
        healthBar.SetHealth(currentHealth, maxHealth);
    }

    protected override void Die()
    {
        base.Die();
        AudioManager.audioManagerInstance.PlaySFX(enemyName + " Die");
        GameManager.instance.bossesKilled++;
        healthBar.DisableHealthBar();
        Destroy(enemiesHolder);
        SpawnItems(1f - probability, minRarity, maxRarity);
        Vector3 pos = MapGenerator.RandomPositionAtDistance(MapGenerator.rooms.Length - 1, transform.position, 2, 6);
        Instantiate(fuel, pos, Quaternion.identity);  
    }
}
