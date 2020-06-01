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

    protected string charName;

    private Animator animator;
   

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int dmgAmount)
    {
        GameManager.instance.damageDone += dmgAmount;
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
            Die();
        else
            AudioManager.audioManagerInstance.PlaySFX(charName + " Hit");
    }
    
    protected virtual void Die()
    {
        AudioManager.audioManagerInstance.PlaySFX(charName + " Die");
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
