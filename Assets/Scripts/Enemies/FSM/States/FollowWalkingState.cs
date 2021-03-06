﻿using UnityEngine;

public class FollowWalkingState : FollowState
{
    public FollowWalkingState(GameObject enemy, StateType state) : base(enemy, state) { }

    int nextStateMask;
    
    bool rightTopHit, rightBottomHit;
    bool leftTopHit, leftBottomHit;
    bool topRightHit, topLeftHit;
    bool bottomRightHit, bottomLeftHit;
    
    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = blockingLayer | wallsLayer | enemiesLayer | waterLayer | playerLayer;
        nextStateMask = blockingLayer | wallsLayer | enemiesLayer | playerLayer;
        destination = null;
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            //Si el jugador está en el rango de ataque del enemigo y nada se interpone entre ellos, se pasa al estado de ataque
            if (enemyBehavior.NeedChangeState(enemyBehavior.attackRange, nextStateMask))
                enemyBehavior.fsm.EnterNextState();

            CheckGiveUp();

            //Si no está en el rango de ataque, el enemigo debe acercarse al jugador
            if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.1f)
            {
                SetUpFollowRays();

                if (!hit && !hit1 && !hit2 && !rightHit && !leftHit && !topHit && !bottomHit)
                {
                    direction = enemyBehavior.playerDirection;
                }
                else //algo bloquea el camino más corto hacia el jugador
                {
                    direction = Vector2.zero;

                    rightTopHit = Physics2D.Raycast(enemyBehavior.rightTopOrigin, Vector2.right, 0.8f, masks);
                    rightBottomHit = Physics2D.Raycast(enemyBehavior.rightBottomOrigin, Vector2.right, 0.8f, masks);

                    leftTopHit = Physics2D.Raycast(enemyBehavior.leftTopOrigin, Vector2.left, 0.8f, masks);
                    leftBottomHit = Physics2D.Raycast(enemyBehavior.leftBottomOrigin, Vector2.left, 0.8f, masks);

                    topRightHit = Physics2D.Raycast(enemyBehavior.topRightOrigin, Vector2.up, 0.8f, masks);
                    topLeftHit = Physics2D.Raycast(enemyBehavior.topLeftOrigin, Vector2.up, 0.8f, masks);

                    bottomRightHit = Physics2D.Raycast(enemyBehavior.bottomRightOrigin, Vector2.down, 0.8f, masks);
                    bottomLeftHit = Physics2D.Raycast(enemyBehavior.bottomLeftOrigin, Vector2.down, 0.8f, masks);

                    float enemyPosX = enemy.transform.position.x;
                    float enemyPosY = enemy.transform.position.y;
                    float targetPosX = enemyBehavior.target.position.x;
                    float targetPosY = enemyBehavior.target.position.y;

                    if (targetPosX >= enemyPosX && !rightHit && !rightTopHit && !rightBottomHit && lastMove != Move.Left)
                        SetDirection(Vector2.right);

                    else if (targetPosY > enemyPosY && !topHit && !topRightHit && !topLeftHit && lastMove != Move.Down)
                        SetDirection(Vector2.up);

                    else if (targetPosX < enemyPosX && !leftHit && !leftTopHit && !leftBottomHit && lastMove != Move.Right)
                        SetDirection(Vector2.left);

                    else if (targetPosY < enemyPosY && !bottomHit && !bottomRightHit && !bottomLeftHit && lastMove != Move.Up)
                        SetDirection(Vector2.down);

                    if (direction == Vector2.zero)
                    {
                        if (!leftHit && !leftTopHit && !leftBottomHit && lastMove != Move.Right)
                            SetDirection(Vector2.left);

                        else if (!topHit && !topRightHit && !topLeftHit && lastMove != Move.Down)
                            SetDirection(Vector2.up);
                        
                        else if (!rightHit && !rightTopHit && !rightBottomHit && lastMove != Move.Left)
                            SetDirection(Vector2.right);
                        
                        else if (!bottomHit && !bottomRightHit && !bottomLeftHit && lastMove != Move.Up)
                            SetDirection(Vector2.down);
                    }
                }
                destination = enemy.transform.position + new Vector3(direction.x * 0.375f, direction.y * 0.375f, 0);
            }
        }  
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive &&
            GameManager.instance.enemiesActive)
        {
            animator.SetBool("isMoving", direction.sqrMagnitude > 0 ? true : false);
            enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
            enemyBehavior.characterMovement.Move(direction, enemyBehavior.followSpeed);
        }
        else
        {
            animator.SetBool("isMoving", false); //si la habilidad especial está activada, el enemigo no debe moverse
        }
    }

    public override void OnStateExit()
    {
        destination = null;
    }
}
