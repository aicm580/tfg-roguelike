using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject explosionEffect;
    public BulletType bulletType;
    public int bulletDamage = 5; //daño que inflige
    public float bulletSpeed = 5.0f; //velocidad de la bala
    public float bulletLifetime = 0.9f; //tiempo de vida de la bala
    public DamageOrigin bulletOwner;
    [HideInInspector]
    public Vector3 direction;
  
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.x * bulletSpeed, direction.y * bulletSpeed);
        Invoke("DestroyBullet", bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bulletOwner == DamageOrigin.Player)
        {
            //Si la bala choca con un enemigo normal
            if (collision.gameObject.tag == "Enemy")
                collision.gameObject.GetComponent<NormalEnemyHealth>().TakeDamage(bulletDamage);
            //Si la bala choca con un Boss
            else if (collision.gameObject.tag == "Boss")
                collision.gameObject.GetComponent<BossHealth>().TakeDamage(bulletDamage);
            //Si la bala choca con un objeto rompible
            else if (collision.gameObject.tag == "Breakable Object")
                collision.gameObject.GetComponent<BreakableObject>().TakeDamage(bulletDamage);
        }
        //Si la bala de un enemigo normal choca con el jugador
        else if (bulletOwner == DamageOrigin.NormalEnemy && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.NormalEnemy);
        }
        //Si la bala de un Boss choca con el jugador
        else if (bulletOwner == DamageOrigin.Boss && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.Boss);
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