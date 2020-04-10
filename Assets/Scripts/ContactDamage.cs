using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    private float touchDelay = 0.22f; //tiempo a esperar para que la colisión vuelva a hacer daño
    private bool canTouch = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si el gameObject colisiona con el jugador
        if (collision.gameObject.tag == "Player" && canTouch == true)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            canTouch = false;
            StartCoroutine(TouchDelay());
        }
    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(touchDelay);
        canTouch = true;
    }
}
