using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject explosionEffect;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            //GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            //Destroy(explosion, 5f); //esperamos 5f para destruir la explosion
            Destroy(gameObject);
        }
    }
}
