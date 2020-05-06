using UnityEngine;

public class PatrolFlyingState : PatrolState
{
    public PatrolFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    RaycastHit2D hit;

    public override void OnStateEnter()
    {
        masks = enemiesLayer;
        InitPatrol();
    }

    public override void UpdateState()
    {
        hit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
        Debug.DrawRay(rayOrigin.position, direction * 1, Color.blue);

        if (hit || (Vector2.Distance(enemy.transform.position, initialPos) >= 1.5f))
        {
            Vector2 dir = direction;
            direction = ChangePatrolDirection(dir);
            rayOrigin = ChangePatrolRayOrigin(direction);
            initialPos = enemy.transform.position;

            //Antes de cambiar la dirección del Animator, comprobamos que la nueva dirección sea válida
            RaycastHit2D newHit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
            if (!newHit)
                enemy.SetAnimatorDirection(direction.x, direction.y);
        }

        //Comprobamos si el enemigo divisa al jugador
        if (enemy.NeedChangeState(enemy.detectionRange, playerLayer) && GameManager.instance.enemiesActive)
            enemy.fsm.EnterNextState();
    }
}
