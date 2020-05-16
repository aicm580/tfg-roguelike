using System.Collections;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [HideInInspector]
    public DamageOrigin contactOriginType;
    [HideInInspector]
    public string contactOriginName;
    private float touchDelay = 0.2f; //tiempo a esperar para que la colisión vuelva a hacer daño
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

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(touchDelay);
        canTouch = true;
    }
}
