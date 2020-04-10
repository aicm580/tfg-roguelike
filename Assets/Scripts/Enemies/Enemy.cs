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

    public bool finalBoss;

    public float detectionRange; //distancia a la que el enemigo ve al jugador
    public float attackRange; //distancia a la que el enemigo empieza a atacar al jugador
    
   


    public bool DetectPlayer()
    {
        Vector2 direction = (GameManager.instance.player.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, direction * detectionRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, 1 << LayerMask.NameToLayer("DetectionLayer"));

        if (hit)
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
            else
            {
                Debug.Log(hit.collider.name);
            }
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        /*
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
        }*/
    }

   
}
