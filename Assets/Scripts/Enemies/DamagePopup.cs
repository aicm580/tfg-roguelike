using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private static int sortingOrder; 

    private TextMeshPro text;
    private Color textColor;
    private float disappearTimer;
    private const float disappearTime = 0.5f;
    

    private void Awake()
    {
        text = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(int damage)
    {
        text.SetText(damage.ToString());
        textColor = text.color;
        disappearTimer = 0.5f;

        //Hacemos que los nuevos popups aparezcan encima de los anteriores
        sortingOrder++;
        text.sortingOrder = sortingOrder;
    }

    private void Update()
    {
        transform.position += new Vector3(0, 0.6f) * Time.deltaTime;

        float scaleAmount = 1f; 

        if (disappearTimer > disappearTime * 0.5f) //si estamos en la primera mitad del popup
        {
            transform.localScale += Vector3.one * scaleAmount * Time.deltaTime;
        }
        else //segunda mitad del popup
        {
            transform.localScale -= Vector3.one * scaleAmount/2 * Time.deltaTime;
        }

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
