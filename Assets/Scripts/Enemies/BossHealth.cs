using UnityEngine;

public class BossHealth : CharacterHealth
{
    [SerializeField]
    protected GameObject fuel;
    [HideInInspector]
    public HealthBar healthBar;

    protected override void Start()
    {
        base.Start();
        healthBar = GameObject.Find("GameManager").GetComponent<EnemiesGenerator>().bossHealthBar;
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
        healthBar.DisableHealthBar();
        SpawnItems(0.2f, minRarity, maxRarity); //Hay un 80% de probabilidades de que, al morir, el enemigo deje un item
        Instantiate(fuel, transform.position + new Vector3(0.35f, 0.35f, 0), Quaternion.identity);
    }
}
