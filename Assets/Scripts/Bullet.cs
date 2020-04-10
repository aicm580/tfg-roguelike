using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject explosionEffect;
    
    public int bulletDamage = 5; //daño que inflige
    public float bulletSpeed = 5.0f; //velocidad de la bala
    public float bulletLifetime = 0.9f; //tiempo de vida de la bala
    public DamageOrigin bulletOwner;
    [HideInInspector]
    public Vector2 direction;
  
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.x * bulletSpeed, direction.y * bulletSpeed);
        Invoke("DestroyBullet", bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        //GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //Destroy(explosion, 5f); //esperamos 5f para destruir la explosion
        Destroy(gameObject);
    }
}