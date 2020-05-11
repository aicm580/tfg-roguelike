using UnityEngine;
using UnityEngine.UI;

public enum DamageOrigin
{
    Player, NormalEnemy, Boss, Obstacle, Item,
}

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    protected int maxHearts; //nº de corazones máximo que puede recolectar

    [SerializeField]
    protected GameObject heartPrefab;
    [SerializeField]
    protected Transform heartPanel;
    protected GameObject[] hearts;

    private int currentHealth;
    private int currentHearts; //nº de corazones actuales (llenos o no)
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPlayerHealth(int initHealth, int initHearts, int maxHeart)
    {
        currentHealth = initHealth;
        currentHearts = initHearts;
        maxHearts = maxHeart;
        SetUIHearts();
    }

    public void Heal(int healAmount)
    {
        if (currentHealth < currentHearts)
        {
            currentHealth += healAmount;
            IncreaseHealthAnimation();
        }
    }

    public void TakeDamage(int dmgAmount, DamageOrigin dmgOrigin)
    {
        if (currentHealth > 0)
        {
            AudioManager.audioManagerInstance.PlaySFX("LoseHeart");
            DecreaseHealthAnimation();
            currentHealth -= dmgAmount;
            if (currentHealth <= 0)
                Die(dmgOrigin);
        } 
    }

    public void IncreaseCurrentHearts(int totalAmount, int fullAmount)
    {
        int full = 0; 

        for (int i = 0; i < totalAmount && currentHearts < maxHearts; i++)
        {
            currentHearts++;
            hearts[currentHearts - 1].GetComponent<Image>().enabled = true;

            if (full < fullAmount)
            {
                full++;
                Heal(1);
            }
        }
    }

    public void DecreaseCurrentHearts(int amount)
    {
        for (int i = 0; i < amount && currentHearts > 0; i++)
        {
            if (currentHealth == currentHearts)
                currentHealth--;

            hearts[currentHearts - 1].GetComponent<Image>().enabled = false;
            currentHearts--;
            
            if (currentHearts <= 0)
                Die(DamageOrigin.Item);
        }
    }

    private void Die(DamageOrigin dmgOrigin)
    {
        animator.SetBool("dead", true);
        GameManager.instance.GameOver();
    }

    private void SetUIHearts()
    {
        foreach (Transform t in heartPanel)
        {
            Destroy(t.gameObject);
        }

        hearts = new GameObject[maxHearts];

        if (currentHealth > currentHearts)
            currentHealth = currentHearts;

        for (int i = 0; i < maxHearts; i++)
        {
            GameObject uiHeart = Instantiate(heartPrefab, heartPanel.transform.position, Quaternion.identity);
            uiHeart.transform.SetParent(heartPanel);
            uiHeart.transform.localScale = new Vector3(1, 1, 1);
            hearts[i] = uiHeart;

            if (i >= currentHealth)
            {
                hearts[i].GetComponent<Animator>().SetBool("empty", true);
            }
            else
            {
                hearts[i].GetComponent<Animator>().SetBool("empty", false);
            }

            if (i < currentHearts)
            {
                hearts[i].GetComponent<Image>().enabled = true;
            }
            else
            {
                hearts[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    private void DecreaseHealthAnimation()
    {
        hearts[currentHealth - 1].GetComponent<Animator>().SetTrigger("loseHeart");
        hearts[currentHealth - 1].GetComponent<Animator>().SetBool("empty", true);
    }

    private void IncreaseHealthAnimation()
    {
        hearts[currentHealth - 1].GetComponent<Animator>().SetTrigger("findHeart");
        hearts[currentHealth - 1].GetComponent<Animator>().SetBool("empty", false);
    }
}
