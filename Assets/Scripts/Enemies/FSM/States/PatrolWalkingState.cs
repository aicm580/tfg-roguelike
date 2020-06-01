using UnityEngine;

public class PatrolWalkingState : PatrolState
{
    public PatrolWalkingState(GameObject enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit, hit1, hit2;
   
    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = blockingLayer | wallsLayer | enemiesLayer | waterLayer;
        destination = null;
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            //Comprobamos si el enemigo divisa al jugador
            if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, wallsLayer | playerLayer))
                enemyBehavior.fsm.EnterNextState();

            if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.1f)
            {
                int attempts = 0;
                do
                {
                    FindNewDirection();

                    hit = Physics2D.Raycast(rayOrigin.position, direction, 1.3f, masks);
                    Debug.DrawRay(rayOrigin.position, direction * 1.3f, Color.yellow);
                    hit1 = Physics2D.Raycast(rays[0], direction, 1.3f, masks);
                    Debug.DrawRay(rays[0], direction * 1.3f, Color.yellow);
                    hit2 = Physics2D.Raycast(rays[1], direction, 1.3f, masks);
                    Debug.DrawRay(rays[1], direction * 1.3f, Color.yellow);

                    attempts++;

                    if (attempts > 15)
                    {
                        direction = Vector2.zero;
                        break;
                    }

                } while (hit || hit1 || hit2);

                destination = enemy.transform.position + new Vector3(direction.x * 0.95f, direction.y * 0.95f, 0);
            }
        }
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive &&
            GameManager.instance.enemiesActive)
        {
            enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
            animator.SetBool("isMoving", direction.sqrMagnitude > 0 ? true : false);
            enemyBehavior.characterMovement.Move(direction, 1);
        }
        else
        {
            animator.SetBool("isMoving", false); //si la habilidad especial está activada, el enemigo no debe moverse
        }
    }
}
