﻿using UnityEngine;

public enum Biome
{
    Land, Water, 
}

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public DamageOrigin dmgOriginType;
    public Biome biome;

    public IntRange friends; //rango de número de enemigos igual que éste que le acompañan
    public int calculatedFriends;

    public float detectionRange; //distancia a la que el enemigo ve al jugador
    public float attackRange; //distancia a la que el enemigo empieza a atacar al jugador
    public float giveUpRange; //distancia a la que el enemigo deja de perseguir al jugador
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
    public Vector2 playerDirection, rayOrigin, rayOrigin1, rayOrigin2;
    [HideInInspector]
    public Vector2 rightTopOrigin, rightBottomOrigin, leftTopOrigin, leftBottomOrigin, bottomRightOrigin, bottomLeftOrigin, topRightOrigin, topLeftOrigin;

    private Animator animator;
    

    private void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        fsm = GetComponent<FiniteStateMachine>();
        animator = GetComponentInChildren<Animator>();

        rightRayOrigin = new GameObject("RightRayOrigin").transform;
        rightRayOrigin.position = transform.position + new Vector3(0.35f, 0, 0);
        rightRayOrigin.SetParent(transform);

        leftRayOrigin = new GameObject("LeftRayOrigin").transform;
        leftRayOrigin.position = transform.position + new Vector3(-0.35f, 0, 0);
        leftRayOrigin.SetParent(transform);

        topRayOrigin = new GameObject("TopRayOrigin").transform;
        topRayOrigin.position = transform.position + new Vector3(0, 0.38f, 0);
        topRayOrigin.SetParent(transform);

        bottomRayOrigin = new GameObject("BottomRayOrigin").transform;
        bottomRayOrigin.position = transform.position + new Vector3(0, -0.38f, 0);
        bottomRayOrigin.SetParent(transform);

        target = GameManager.instance.playerTransform;
    }

    private void Start()
    {
        ContactDamage contactDmg = GetComponent<ContactDamage>();
        if (contactDmg != null)
        {
            contactDmg.contactOriginType = dmgOriginType;
            contactDmg.contactOriginName = enemyName;
        }
    }

    public bool NeedChangeState(float range, int mask)
    {
        playerDirection = GetDirectionToPlayer();
        GetRayOrigin(target.position);
        Debug.DrawRay(rayOrigin, playerDirection * range, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, playerDirection, range, mask);

        if (hit)
        {
            if (hit.collider.tag == "Player")
                return true;
        }
        return false;
    }

    public Vector2 GetDirectionToPlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        return direction;
    }

    public void GetRayOrigin(Vector2 target)
    {
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

        Vector2[] rays = GetOtherRays(rayOrigin);
        rayOrigin1 = rays[0];
        rayOrigin2 = rays[1];
    }

    public Vector2[] GetOtherRays(Vector3 ray)
    {
        Vector2[] rays = new Vector2[2];
        
        rightTopOrigin = new Vector2(rightRayOrigin.position.x, rightRayOrigin.position.y + 0.3f);
        rightBottomOrigin = new Vector2(rightRayOrigin.position.x, rightRayOrigin.position.y - 0.3f);
        leftTopOrigin = new Vector2(leftRayOrigin.position.x, leftRayOrigin.position.y + 0.3f);
        leftBottomOrigin = new Vector2(leftRayOrigin.position.x, leftRayOrigin.position.y - 0.3f);
        bottomRightOrigin = new Vector2(bottomRayOrigin.position.x + 0.3f, bottomRayOrigin.position.y);
        bottomLeftOrigin = new Vector2(bottomRayOrigin.position.x - 0.3f, bottomRayOrigin.position.y);
        topRightOrigin = new Vector2(topRayOrigin.position.x + 0.3f, topRayOrigin.position.y);
        topLeftOrigin = new Vector2(topRayOrigin.position.x - 0.3f, topRayOrigin.position.y);

        if (ray == rightRayOrigin.position)
        {
            rays[0] = rightTopOrigin;
            rays[1] = rightBottomOrigin;
        }
        else if (ray == leftRayOrigin.position)
        {
            rays[0] = leftTopOrigin;
            rays[1] = leftBottomOrigin;
        }
        else if (ray == topRayOrigin.position)
        {
            rays[0] = topRightOrigin;
            rays[1] = topLeftOrigin;
        }
        else if (ray == bottomRayOrigin.position)
        {
            rays[0] = bottomRightOrigin;
            rays[1] = bottomLeftOrigin;
        }

        return rays;
    }

    public void SetAnimatorDirection(float x, float y)
    {
        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
    }
}
