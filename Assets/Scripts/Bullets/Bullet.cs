using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType bulletType;
    public int bulletDamage = 5; //daño que inflige
    public float bulletSpeed = 5.0f; //velocidad de la bala
    public float bulletLifetime = 0.9f; //tiempo de vida de la bala
    public Color explosionColor;

    [HideInInspector]
    public DamageOrigin bulletOwnerType;
    [HideInInspector]
    public string bulletOwnerName;

    [HideInInspector]
    public Vector3 direction;
  
    private Rigidbody2D rb;
    private GameObject bulletExplosion;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(direction.x * bulletSpeed, direction.y * bulletSpeed);
        bulletExplosion = BulletAssets.instance.explosion;
        Invoke("DestroyBullet", bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bulletOwnerType == DamageOrigin.Player)
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
        else if (bulletOwnerType == DamageOrigin.NormalEnemy && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.NormalEnemy, bulletOwnerName);
        }
        //Si la bala de un Boss choca con el jugador
        else if (bulletOwnerType == DamageOrigin.Boss && collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(1, DamageOrigin.Boss, bulletOwnerName);
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        bulletExplosion.GetComponent<SpriteRenderer>().color = explosionColor;
        GameObject explosion = Instantiate(bulletExplosion, transform.position + (direction * 0.15f), Quaternion.identity);
        float destroyTime = explosion.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(explosion, destroyTime); //esperamos para destruir la explosion
        Destroy(gameObject);
    }
}