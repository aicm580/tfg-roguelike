using UnityEngine;

public class PatrolWalkingState : PatrolState
{
    public PatrolWalkingState(GameObject enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit, hit1, hit2;
   
    public override void OnStateEnter()
    {
        masks = blockingLayer | wallsLayer | enemiesLayer | waterLayer;
        InitPatrol();
    }

    public override void UpdateState()
    {
        hit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
        hit1 = Physics2D.Raycast(rays[0], direction, 1, masks);
        hit2 = Physics2D.Raycast(rays[1], direction, 1, masks);

        if (hit || hit1 || hit2 || (Vector2.Distance(enemy.transform.position, initialPos) >= 1.5f))
        {
            Vector2 dir = direction;
            direction = ChangePatrolDirection(dir);
            rayOrigin = ChangePatrolRayOrigin(direction);
            initialPos = enemy.transform.position;

            //Antes de cambiar la dirección del Animator, comprobamos que la nueva dirección sea válida
            RaycastHit2D newHit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
            if (!newHit)
                enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
        }
        rays = enemyBehavior.GetOtherRays(rayOrigin.position);

        //Comprobamos si el enemigo divisa al jugador
        if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, wallsLayer | playerLayer) && GameManager.instance.enemiesActive)
            enemyBehavior.fsm.EnterNextState();
    }
}
