using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Image barShadows;

    public void SetHealth(int health, int maxHealth)
    {
        slider.value = health;
        barShadows.fillAmount = (float)health / (float)maxHealth - 0.018f;
    }

    public void SetMaxHealth (int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void EnableHealthBar()
    {
        gameObject.SetActive(true);
    }

    public void DisableHealthBar()
    {
        gameObject.SetActive(false);
    }
}
