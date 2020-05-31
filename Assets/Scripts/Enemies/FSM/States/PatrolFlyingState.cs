using UnityEngine;

public class PatrolFlyingState : PatrolState
{
    public PatrolFlyingState(GameObject enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = wallsLayer | enemiesLayer;
    }

    public override void UpdateState()
    {
        //Comprobamos si el enemigo divisa al jugador
        if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, playerLayer) && GameManager.instance.enemiesActive)
            enemyBehavior.fsm.EnterNextState();

        if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.1f)
        {
            int attempts = 0;
            do
            {
                Vector2 dir = direction;
                direction = ChangePatrolDirection(dir);
                rayOrigin = ChangePatrolRayOrigin(direction);
                rays = enemyBehavior.GetOtherRays(rayOrigin.position);

                hit = Physics2D.Raycast(rayOrigin.position, direction, 1.2f, masks);
                Debug.DrawRay(rayOrigin.position, direction * 1.2f, Color.yellow);

                attempts++;

                if (attempts > 15)
                {
                    direction = Vector2.zero;
                    break;
                }

            } while (hit);

            destination = enemy.transform.position + new Vector3(direction.x, direction.y, 0);
        }
    }
}
