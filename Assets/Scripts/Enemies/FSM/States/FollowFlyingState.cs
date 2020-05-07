using UnityEngine;

public class FollowFlyingState : FollowState
{
    public FollowFlyingState(GameObject enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = playerLayer | enemiesLayer;
        animator.SetBool("isFollowing", true);
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            //Si el jugador está en el rango de ataque del enemigo y nada se interpone entre ellos, se pasa al estado de ataque
            if (enemyBehavior.NeedChangeState(enemyBehavior.attackRange, masks))
                enemyBehavior.fsm.EnterNextState();

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

                    float enemyPosX = enemy.transform.position.x;
                    float enemyPosY = enemy.transform.position.y;
                    float targetPosX = enemyBehavior.target.position.x;
                    float targetPosY = enemyBehavior.target.position.y;

                    if (targetPosX >= enemyPosX && !rightHit && lastMove != Move.Left)
                        SetDirection(Vector2.right);
                    
                    else if (targetPosY > enemyPosY && !topHit && lastMove != Move.Down)
                        SetDirection(Vector2.up);
                    
                    else if (targetPosX < enemyPosX && !leftHit && lastMove != Move.Right)
                        SetDirection(Vector2.left);
                    
                    else if (targetPosY < enemyPosY && !bottomHit && lastMove != Move.Up)
                        SetDirection(Vector2.down);

                    if (direction == Vector2.zero)
                    {
                        if (!leftHit && lastMove != Move.Right)
                            SetDirection(Vector2.left);
                        
                        else if (!topHit && lastMove != Move.Down)
                            SetDirection(Vector2.up);
                        
                        else if (!rightHit && lastMove != Move.Left)
                            SetDirection(Vector2.right);
                        
                        else if (!bottomHit && lastMove != Move.Up)
                            SetDirection(Vector2.down);
                    }
                }
                destination = enemy.transform.position + new Vector3(direction.x * 0.65f, direction.y * 0.65f, 0);
            }
            CheckGiveUp();
        }
    }
}
