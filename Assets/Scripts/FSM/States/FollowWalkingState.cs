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
    bool hit;
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
            hit = Physics2D.Raycast(enemy.rayOrigin, enemy.playerDirection, 0.5f, masks);
            if (!hit)
            {
                direction = enemy.playerDirection;
            }
            else //algo bloquea el camino más corto hacia el jugador
            {
                direction = Vector2.zero;
                rightHit = Physics2D.Raycast(enemy.rightRayOrigin.position, Vector2.right, 0.5f, masks);
                leftHit = Physics2D.Raycast(enemy.leftRayOrigin.position, Vector2.left, 0.5f, masks);
                topHit = Physics2D.Raycast(enemy.topRayOrigin.position, Vector2.up, 0.5f, masks);
                bottomHit = Physics2D.Raycast(enemy.bottomRayOrigin.position, Vector2.down, 0.5f, masks);

                float enemyPosX = enemy.transform.position.x;
                float enemyPosY = enemy.transform.position.y;
                float targetPosX = enemy.target.position.x;
                float targetPosY = enemy.target.position.y;


                if (targetPosX >= enemyPosX + 0.5f)
                {
                    Debug.Log("está a la derecha");
                    if (!rightHit)
                        direction = Vector2.right;
                    else if (targetPosY >= enemyPosY && !topHit)
                        direction = Vector2.up;
                    else if (targetPosY < enemyPosY && !bottomHit)
                        direction = Vector2.down;
                }
                else if (targetPosX < enemyPosX && !leftHit)
                {
                    if (!leftHit)
                        direction = Vector2.left;
                    else if (targetPosY >= enemyPosY && !topHit)
                        direction = Vector2.up;
                    else if (targetPosY < enemyPosY && !bottomHit)
                        direction = Vector2.down;
                }
            }
        }
    }
    
    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, enemy.followSpeed);
    }
}
