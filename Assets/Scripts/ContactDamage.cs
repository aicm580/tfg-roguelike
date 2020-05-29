using System.Collections;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [HideInInspector]
    public DamageOrigin contactOriginType;
    [HideInInspector]
    public string contactOriginName;
    private float touchDelay = 0.35f; //tiempo a esperar para que la colisión vuelva a hacer daño
    private float collisionStayTimer;
    private bool canTouch = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si el gameObject colisiona con el jugador
        if (collision.gameObject.tag == "Player" && canTouch == true)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, contactOriginType, contactOriginName);
            canTouch = false;
            StartCoroutine(TouchDelay());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Si el jugador se mantiene en la colisión con el enemigo, recibirá daño cada segundo
            if (collisionStayTimer >= 1)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, contactOriginType, contactOriginName);
                collisionStayTimer = 0;
            }
            else
            {
                collisionStayTimer += Time.deltaTime;
            }
        }   
    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(touchDelay);
        canTouch = true;
    }
}
