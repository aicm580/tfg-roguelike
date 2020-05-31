using UnityEngine;

public class PatrolWalkingState : PatrolState
{
    public PatrolWalkingState(GameObject enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit, hit1, hit2;
   
    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = blockingLayer | wallsLayer | enemiesLayer | waterLayer;
    }

    public override void UpdateState()
    {
        //Comprobamos si el enemigo divisa al jugador
        if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, wallsLayer | playerLayer) && GameManager.instance.enemiesActive)
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

                hit = Physics2D.Raycast(rayOrigin.position, direction, 1.1f, masks);
                Debug.DrawRay(rayOrigin.position, direction * 1.1f, Color.yellow);
                hit1 = Physics2D.Raycast(rays[0], direction, 1.1f, masks);
                Debug.DrawRay(rays[0], direction * 1.1f, Color.yellow);
                hit2 = Physics2D.Raycast(rays[1], direction, 1.1f, masks);
                Debug.DrawRay(rays[1], direction * 1.1f, Color.yellow);

                attempts++;

                if (attempts > 15)
                {
                    direction = Vector2.zero;
                    break;
                }

            } while (hit || hit1 || hit2);
            
            destination = enemy.transform.position + new Vector3(direction.x * 0.9f, direction.y * 0.95f, 0);
        }
    }
}
