using UnityEngine;
using UnityEngine.UI;

public enum DamageOrigin
{
    Player, NormalEnemy, Boss, Obstacle, Item,
}

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    protected int initHealth;
    [SerializeField]
    protected int initHearts = 3; //nº de corazones iniciales (llenos o no)
    [SerializeField]
    protected int maxHearts = 10; //nº de corazones máximo que puede recolectar
    [SerializeField]
    protected Image[] hearts;

    private int currentHealth;
    private int currentHearts; //nº de corazones actuales (llenos o no)
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetPlayerHealth();
    }

    public void SetPlayerHealth()
    {
        currentHealth = initHealth;
        currentHearts = initHearts;
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
            DecreaseHealthAnimation();
            currentHealth -= dmgAmount;
            if (currentHealth <= 0)
                Die(dmgOrigin);
        } 
    }

    public void IncreaseCurrentHearts(int totalAmount, int fullAmount)
    {
        int full = 0; 

        for (int i = 0; i < totalAmount; i++)
        {
            if (currentHearts < maxHearts)
            {
                currentHearts++;
                hearts[currentHearts - 1].enabled = true;

                if (full < fullAmount)
                {
                    IncreaseHealthAnimation();
                    full++;
                }
            }
            else
            {
                break;
            }
        }
    }

    public void DecreaseCurrentHearts(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (currentHearts > 0)
            {
                currentHearts--;
                hearts[currentHearts - 1].enabled = false;
            }
            else
            {
                break;
            }
        }
    }

    private void Die(DamageOrigin dmgOrigin)
    {
        animator.SetBool("dead", true);
        GameManager.instance.GameOver();
    }

    private void SetUIHearts()
    {
        if (currentHealth > currentHearts)
            currentHealth = initHearts;

        for (int i = 0; i < hearts.Length; i++)
        {
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
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void DecreaseHealthAnimation()
    {
        hearts[currentHealth - 1].GetComponent<Animator>().SetTrigger("loseHeart");
        hearts[currentHealth - 1].GetComponent<Animator>().SetBool("empty", true);
        Debug.Log("YOU LOST A LIFE");
    }

    private void IncreaseHealthAnimation()
    {
        hearts[currentHealth - 1].GetComponent<Animator>().SetTrigger("findHeart");
        hearts[currentHealth - 1].GetComponent<Animator>().SetBool("empty", false);
        Debug.Log("YOU WON A LIFE");
    }
}
