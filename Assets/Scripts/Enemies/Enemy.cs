using System.Collections;
using System.Collections.Generic;
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

    public float detectionRange; //distancia a la que el enemigo ve al jugador
    public float attackRange; //distancia a la que el enemigo empieza a atacar al jugador
    public float followSpeed; //actuará como parámetro multiplier del método CharacterMovement.Move

    [HideInInspector]
    public Transform rightRayOrigin, leftRayOrigin, topRayOrigin, bottomRayOrigin;

    [HideInInspector]
    public FiniteStateMachine fsm;
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public CharacterMovement characterMovement;
    [HideInInspector]
    public Vector2 playerDirection, rayOrigin;

    private Animator animator;
    

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        fsm = GetComponent<FiniteStateMachine>();
        animator = GetComponentInChildren<Animator>();

        rightRayOrigin = new GameObject("RightRayOrigin").transform;
        rightRayOrigin.position = transform.position + new Vector3(0.41f, 0, 0);
        rightRayOrigin.SetParent(transform);
        leftRayOrigin = new GameObject("LeftRayOrigin").transform;
        leftRayOrigin.position = transform.position + new Vector3(-0.41f, 0, 0);
        leftRayOrigin.SetParent(transform);
        topRayOrigin = new GameObject("TopRayOrigin").transform;
        topRayOrigin.position = transform.position + new Vector3(0, 0.41f, 0);
        topRayOrigin.SetParent(transform);
        bottomRayOrigin = new GameObject("BottomRayOrigin").transform;
        bottomRayOrigin.position = transform.position + new Vector3(0, -0.41f, 0);
        bottomRayOrigin.SetParent(transform);
    }

    private void Start()
    {
        target = GameManager.instance.player.transform;
    }

    public bool NeedChangeState(float range, int mask)
    {
        playerDirection = GetDirectionToPlayer();
        rayOrigin = GetRayOrigin(target.position);
        Debug.DrawRay(rayOrigin, playerDirection * range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, playerDirection, range, mask);

        if (hit)
        {
            if (hit.collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    public Vector2 GetDirectionToPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        return direction;
    }

    private Vector2 GetRayOrigin(Vector2 target)
    {
        Vector2 rayOrigin;
        float rightDistance = Vector2.Distance(rightRayOrigin.position, target);
        float leftDistance = Vector2.Distance(leftRayOrigin.position, target);
        float topDistance = Vector2.Distance(topRayOrigin.position, target);
        float bottomDistance = Vector2.Distance(bottomRayOrigin.position, target);
        float min = Mathf.Min(rightDistance, leftDistance, topDistance, bottomDistance);

        if (min == rightDistance)
            rayOrigin = rightRayOrigin.position;
        else if (min == leftDistance)
            rayOrigin = leftRayOrigin.position;
        else if (min == topDistance)
            rayOrigin = topRayOrigin.position;
        else
            rayOrigin = bottomRayOrigin.position;
        
        return rayOrigin;
    }

    public void SetAnimatorDirection(float x, float y)
    {
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
    }

    public bool MoveTowardsPlayer()
    {
        if (Vector2.Distance(transform.position, target.position) > attackRange)
        {
            Vector2 direction = GetDirectionToPlayer();
            characterMovement.Move(direction, 1.8f);
            return true;
        }
        return false;
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

        Debug.Log("COLLISION");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Debug.Log(collision.gameObject.name);
        }
    }
}
