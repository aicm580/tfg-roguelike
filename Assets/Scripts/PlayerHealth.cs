using UnityEngine;

public class PlayerHealth : CharacterHealth
{
    [SerializeField]
    protected int initHearts = 3; //nº de corazones iniciales (llenos o no)
    [SerializeField]
    protected int maxHearts = 10; //nº de corazones máximo que puede recolectar

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHearts)
        {
            currentHealth = maxHearts;
        }
    }
}
