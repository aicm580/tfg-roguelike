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
        //Si la bala choca con un enemigo, comprobamos que la bala sea del jugador antes de infligir daño al enemigo
        if (collision.gameObject.tag == "Enemy" && bulletOwner == DamageOrigin.Player)
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
        }
        //Si la bala choca con el jugador, comprobamos que la bala sea de un enemigo antes de infligir daño al jugador
        else if (collision.gameObject.tag == "Player" && bulletOwner == DamageOrigin.NormalEnemy)
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.NormalEnemy);
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        //GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //Destroy(explosion, 5f); //esperamos 5f para destruir la explosion
        Destroy(gameObject);
    }
}