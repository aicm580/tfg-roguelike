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
        disappearTimer = 0.4f;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0.5f) * Time.deltaTime;
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3.5f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            text.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
