using UnityEngine;

public class BossHealth : CharacterHealth
{
    [SerializeField]
    protected GameObject fuel;
    
    protected HealthBar healthBar;

    protected override void Start()
    {
        base.Start();
        healthBar = GameObject.Find("BossHealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
    }

    public override void TakeDamage(int dmgAmount)
    {
        base.TakeDamage(dmgAmount);
        healthBar.SetHealth(currentHealth);
    }

    protected override void Die()
    {
        base.Die();

        SpawnItems(0.2f, minRarity, maxRarity); //Hay un 80% de probabilidades de que, al morir, el enemigo deje un item
        Instantiate(fuel, transform.position + new Vector3(0.35f, 0.35f, 0), Quaternion.identity);
    }
}
