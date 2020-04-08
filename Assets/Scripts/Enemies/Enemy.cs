using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
    Velociraptor, Dilophosaurus, Meganeura, Olghoïkhorkhoï, 
} 

public enum Biome
{
    Land, Water, 
}

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Biome biome;

    public IntRange friends; //rango de número de enemigos igual que éste que le acompañan
    public int calculatedFriends;

    public int health;
    public bool finalBoss;

    public float detectionRange; //distancia a la que el enemigo ve al jugador
    public float attackRange; //distancia a la que el enemigo empieza a atacar al jugador
    public bool touchDamage; //si es true, significa que al tocarlo el player recibirá daño
    private float touchDelay = 0.3f; //tiempo a esperar para que la colisión vuelva a hacer daño
    private bool canTouch = true;

 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si el enemigo colisiona con el jugador y este enemigo hace daño al ser tocado
        if (collision.gameObject.tag == "Player" && touchDamage == true && canTouch == true)
        {
            GameManager.instance.LoseLife();
            canTouch = false;
            StartCoroutine(TouchDelay());
        }

        //Si una bala lanzada por el jugador toca al enemigo, este recibe daño
        if (collision.gameObject.tag == "Bullet")
        {
            int damage = PlayerController.playerInstance.bulletDamage;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
            Transform damagePopupTransform = Instantiate(GameManager.instance.popupDamageText, pos, Quaternion.identity);
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damage);
            health -= damage;
            GameManager.instance.damageDone += damage;

            if (health <= 0)
            {
                if (finalBoss)
                {
                    GameManager.instance.bossesKilled += 1;
                }
                else
                {
                    GameManager.instance.normalEnemiesKilled += 1;
                }
                GameManager.instance.totalKills += 1;
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TouchDelay()
    {
        yield return new WaitForSeconds(touchDelay);
        canTouch = true;
        Debug.Log(canTouch);
    }

    public bool DetectPlayer()
    {
        Vector2 direction = (PlayerController.playerInstance.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, 1 << LayerMask.NameToLayer("BlockingLayer"));
      
        if (hit != null)
        {
            if (hit.transform.tag == "Player")
            {
                Debug.Log(hit.transform.tag);
                return true;
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
        return false;
    }
}
