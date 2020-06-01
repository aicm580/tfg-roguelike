using UnityEngine;

public class PatrolFlyingState : PatrolState
{
    public PatrolFlyingState(GameObject enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = wallsLayer | enemiesLayer;
        destination = null;
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            //Comprobamos si el enemigo divisa al jugador
            if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, playerLayer))
                enemyBehavior.fsm.EnterNextState();

            if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.1f)
            {
                int attempts = 0;
                do
                {
                    FindNewDirection();

                    hit = Physics2D.Raycast(rayOrigin.position, direction, 1.25f, masks);
                    Debug.DrawRay(rayOrigin.position, direction * 1.25f, Color.yellow);

                    attempts++;

                    if (attempts > 15)
                    {
                        direction = Vector2.zero;
                        break;
                    }

                } while (hit);

                destination = enemy.transform.position + new Vector3(direction.x * 0.8f, direction.y * 0.8f, 0);
            }
        }
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive &&
            GameManager.instance.enemiesActive)
        {
            enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
            enemyBehavior.characterMovement.Move(direction, 1);
        }
    }
}
