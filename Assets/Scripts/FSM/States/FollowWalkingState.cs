using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowWalkingState : State
{
    Vector2 direction;
    int masks;
    int blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
    int detectionLayer = 1 << LayerMask.NameToLayer("DetectionLayer");
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");
    bool rightHit;
    bool leftHit;
    bool topHit;
    bool bottomHit;

    public FollowWalkingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        masks = blockingLayer | detectionLayer | enemiesLayer;
        animator.SetBool("isFollowing", true);
    }

    public override void UpdateState()
    {
        //Si el jugador está en el rango de ataque del enemigo y nada se interpone entre ellos, se pasa al estado de ataque
        if (enemy.NeedChangeState(enemy.attackRange, masks))
        {
            // enemy.fsm.EnterNextState();
            Debug.Log("Ataquemos");
        }
        //Si no está en el rango de ataque, el enemigo debe acercarse al jugador
        else
        {
            direction = Vector2.zero;
            rightHit = Physics2D.Raycast(enemy.rightRayOrigin.position, Vector2.right, 0.6f, masks);
            leftHit = Physics2D.Raycast(enemy.leftRayOrigin.position, Vector2.left, 0.6f, masks);
            topHit = Physics2D.Raycast(enemy.topRayOrigin.position, Vector2.up, 0.6f, masks);
            bottomHit = Physics2D.Raycast(enemy.bottomRayOrigin.position, Vector2.down, 0.6f, masks);

            if (Mathf.Round(enemy.playerDirection.x) > 0 && !rightHit)
            {
                direction = Vector2.right;
            }
            //Si lo anterior no ha resultado, probamos a ir a la dirección y del player
            else if (Mathf.Round(enemy.playerDirection.y) > 0 && !topHit)
            {
                //Debug.Log("Miramos de movernos en vertical");
                direction = Vector2.up;
            }
            else if (Mathf.Round(enemy.playerDirection.x) < 0 && !leftHit)
            {
                direction = Vector2.left;
            }
            else if (Mathf.Round(enemy.playerDirection.y) < 0 && !bottomHit)
            {
                direction = Vector2.down;
            }
            //Si no se puede ir en dirección al player, se mueve en cualquier otra dirección disponible
            else
            {
                if (!topHit)
                    direction = Vector2.up;
                else if (!rightHit)
                    direction = Vector2.right;
                else if (!bottomHit)
                    direction = Vector2.down;
                else
                    direction = Vector2.left;
            }
        }
    }
    
    public override void FixedUpdateState()
    {
        Debug.Log(direction);
        enemy.characterMovement.Move(direction, enemy.followSpeed);
    }
}
