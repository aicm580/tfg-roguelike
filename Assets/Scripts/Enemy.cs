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

    //private Animator anim;


    void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Si el enemigo colisiona con el jugador y este enemigo hace daño al ser tocado
        if (collision.gameObject.tag == "Player" && touchDamage == true)
        {
            gameManager.LoseLife();
        }
    }
}
