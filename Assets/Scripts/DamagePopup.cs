using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshPro text;
    private Color textColor;
    private float disappearTimer;
    

    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damage)
    {
        text.SetText(damage.ToString());
        textColor = text.color;
        disappearTimer = 0.5f;
    }

    private void Update()
    {
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
