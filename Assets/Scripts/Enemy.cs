using System.Collections;
using UnityEngine;

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
    private GameManager gameManager; 

    public EnemyType enemyType;
    public Biome biome;

    public IntRange friends; //rango de número de enemigos igual que éste que le acompañan
    public int calculatedFriends;

    public int health;

    public bool touchDamage; //si es true, significa que al tocarlo el player recibirá daño
    private float touchDelay = 0.4f; //tiempo a esperar para que la colisión vuelva a hacer daño
    private bool canTouch = true;

    //private Animator anim;


    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //anim = GetComponent<Animator>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si el enemigo colisiona con el jugador y este enemigo hace daño al ser tocado
        if (collision.gameObject.tag == "Player" && touchDamage == true && canTouch == true)
        {
            gameManager.LoseLife();
            canTouch = false;
            StartCoroutine(TouchDelay());
        }

        //Si una bala lanzada por el jugador toca al enemigo, este recibe daño
        if (collision.gameObject.tag == "Bullet")
        {
            int damage = gameManager.playerController.bulletDamage;
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.4f);
            Transform damagePopupTransform = Instantiate(gameManager.popupDamageText, pos, Quaternion.identity);
            DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();
            damagePopup.Setup(damage);
            health -= damage;

            if(health <= 0)
            {
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
}
