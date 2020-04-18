using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Move
{
    Right, Left, Up, Down,
}

public class FollowWalkingState : State
{
    Vector2 direction;
    Vector2 colDirection;
    Vector2? destination;
    
    int masks;
    int blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
    int detectionLayer = 1 << LayerMask.NameToLayer("DetectionLayer");
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");
    bool hit, hit1, hit2;
    bool rightHit, rightTopHit, rightBottomHit;
    bool leftHit, leftTopHit, leftBottomHit;
    bool topHit, topRightHit, topLeftHit;
    bool bottomHit, bottomRightHit, bottomLeftHit;
    Move lastMove;

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
            enemy.fsm.EnterNextState();
        }
        //Si no está en el rango de ataque, el enemigo debe acercarse al jugador
        if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.1f)
        {
            hit = Physics2D.Raycast(enemy.rayOrigin, enemy.playerDirection, 0.8f, masks);
            hit1 = Physics2D.Raycast(enemy.rayOrigin1, enemy.playerDirection, 0.8f, masks);
            hit2 = Physics2D.Raycast(enemy.rayOrigin2, enemy.playerDirection, 0.8f, masks);
            rightHit = Physics2D.Raycast(enemy.rightRayOrigin.position, Vector2.right, 0.2f, masks);
            leftHit = Physics2D.Raycast(enemy.leftRayOrigin.position, Vector2.left, 0.2f, masks);
            topHit = Physics2D.Raycast(enemy.topRayOrigin.position, Vector2.up, 0.2f, masks);
            bottomHit = Physics2D.Raycast(enemy.bottomRayOrigin.position, Vector2.down, 0.2f, masks);

            if (!hit && !hit1 && !hit2 && !rightHit && !leftHit && !topHit && !bottomHit)
            {
                direction = enemy.playerDirection;
            }
            else //algo bloquea el camino más corto hacia el jugador
            {
                direction = Vector2.zero;
               
                rightTopHit = Physics2D.Raycast(enemy.rightTopOrigin, Vector2.right, 0.8f, masks);
                rightBottomHit = Physics2D.Raycast(enemy.rightBottomOrigin, Vector2.right, 0.8f, masks);
                
                leftTopHit = Physics2D.Raycast(enemy.leftTopOrigin, Vector2.left, 0.8f, masks);
                leftBottomHit = Physics2D.Raycast(enemy.leftBottomOrigin, Vector2.left, 0.8f, masks);
               
                topRightHit = Physics2D.Raycast(enemy.topRightOrigin, Vector2.up, 0.8f, masks);
                topLeftHit = Physics2D.Raycast(enemy.topLeftOrigin, Vector2.up, 0.8f, masks);
                
                bottomRightHit = Physics2D.Raycast(enemy.bottomRightOrigin, Vector2.down, 0.8f, masks);
                bottomLeftHit = Physics2D.Raycast(enemy.bottomLeftOrigin, Vector2.down, 0.8f, masks);

                float enemyPosX = enemy.transform.position.x;
                float enemyPosY = enemy.transform.position.y;
                float targetPosX = enemy.target.position.x;
                float targetPosY = enemy.target.position.y;
                
                if (targetPosX >= enemyPosX && !rightHit && !rightTopHit && !rightBottomHit && lastMove != Move.Left)
                {
                    direction = Vector2.right;
                    lastMove = Move.Right;    
                }
                else if (targetPosY > enemyPosY && !topHit && !topRightHit && !topLeftHit && lastMove != Move.Down)
                {
                    direction = Vector2.up;
                    lastMove = Move.Up;
                }
                else if (targetPosX < enemyPosX && !leftHit && !leftTopHit && !leftBottomHit && lastMove != Move.Right)
                {
                    direction = Vector2.left;
                    lastMove = Move.Left;
                }
                else if (targetPosY < enemyPosY && !bottomHit && !bottomRightHit && !bottomLeftHit && lastMove != Move.Up)
                {
                    direction = Vector2.down;
                    lastMove = Move.Down;
                }
                
                if (direction == Vector2.zero)
                {
                    if (!leftHit && !leftTopHit && !leftBottomHit && lastMove != Move.Right)
                    {
                        direction = Vector2.left;
                        lastMove = Move.Left;
                    }
                    else if (!topHit && !topRightHit && !topLeftHit && lastMove != Move.Down)
                    {
                        direction = Vector2.up;
                        lastMove = Move.Up;
                    }
                    else if (!rightHit && !rightTopHit && !rightBottomHit && lastMove != Move.Left)
                    {
                        direction = Vector2.right;
                        lastMove = Move.Right;
                    }
                    else if (!bottomHit && !bottomRightHit && !bottomLeftHit && lastMove != Move.Up)
                    {
                        direction = Vector2.down;
                        lastMove = Move.Down;
                    }     
                }
            }
            destination = enemy.transform.position + new Vector3(direction.x * 0.65f, direction.y * 0.65f, 0);
        }

        if (Vector2.Distance(enemy.target.position, enemy.transform.position) >= enemy.giveUpRange)
        {
            enemy.fsm.EnterPreviousState();
        }
    }
    
    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, enemy.followSpeed);
    }

    public override void OnStateExit()
    {
        destination = null;
    }
}
