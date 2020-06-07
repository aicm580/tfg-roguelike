using System.Collections;
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
    private SpriteRenderer spriteRend;
    public Color dmgColor = new Color(255f / 255f, 177f / 255f, 177f / 255f);


    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRend = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int dmgAmount)
    {
        spriteRend.color = dmgColor;
        StartCoroutine(ReturnToOriginalColor());

        GameManager.instance.damageDone += dmgAmount;
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
            Die();
        else
            AudioManager.audioManagerInstance.PlaySFX(charName + " Hit");
    }

    private IEnumerator ReturnToOriginalColor()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRend.color = Color.white;
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
