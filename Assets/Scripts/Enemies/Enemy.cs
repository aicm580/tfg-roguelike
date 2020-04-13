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

[RequireComponent(typeof(FiniteStateMachine))]
public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;
    public Biome biome;

    public IntRange friends; //rango de número de enemigos igual que éste que le acompañan
    public int calculatedFriends;

    public float detectionRange; //distancia a la que el enemigo ve al jugador
    public float attackRange; //distancia a la que el enemigo empieza a atacar al jugador

    private Transform target;
    private CharacterMovement characterMovement;
    FiniteStateMachine fsm;


    private void Awake()
    {
        characterMovement = this.GetComponent<CharacterMovement>();
        fsm = this.GetComponent<FiniteStateMachine>();
    }

    private void Start()
    {
        target = GameManager.instance.player.transform;
    }

    public bool DetectPlayer()
    {
        Vector2 direction = GetDirectionToPlayer();
        Debug.DrawRay(transform.position, direction * detectionRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, 1 << LayerMask.NameToLayer("DetectionLayer"));

        if (hit)
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public bool FollowPlayerWalking()
    {
        if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            Vector2 direction = GetDirectionToPlayer();
            characterMovement.Move(direction);
            return true;
        }
        return false;
    }

    public Vector2 GetDirectionToPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        return direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Comprobamos si el enemigo colisiona con algo que le haga daño
        if (collision.gameObject.tag == "Bullet")
        {
            //Comprobamos que la bala ha sido lanzada por el jugador
            if (collision.gameObject.GetComponent<Bullet>().bulletOwner == DamageOrigin.Player)
            {
                int damage = collision.gameObject.GetComponent<Bullet>().bulletDamage;
                GetComponent<EnemyHealth>().TakeDamage(damage);
            }
        }
    }
}
