using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : CharacterHealth
{
    [SerializeField]
    protected int initHearts = 3; //nº de corazones iniciales (llenos o no)
    [SerializeField]
    protected int maxHearts = 10; //nº de corazones máximo que puede recolectar
    [SerializeField]
    protected Image[] hearts;

    private int currentHearts; //nº de corazones actuales (llenos o no)

    protected override void Start()
    {
        base.Start();
        currentHearts = initHearts;
        SetUIHearts();
    }

    public override void TakeDamage(int dmgAmount, DamageOrigin dmgOrigin)
    {
        DecreaseHealthAnimation();
        base.TakeDamage(dmgAmount, dmgOrigin);
    }

    protected override void Die(DamageOrigin dmgOrigin)
    {
        base.Die(dmgOrigin);
        GameManager.instance.GameOver();
    }

    public void Heal(int healAmount)
    {
        IncreaseHealthAnimation();
        currentHealth += healAmount;
        if (currentHealth > maxHearts)
        {
            currentHealth = maxHearts;
        }
    }

    private void SetUIHearts()
    {
        if (currentHealth > currentHearts)
        {
            currentHealth = initHearts;
        }

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

    }
}
