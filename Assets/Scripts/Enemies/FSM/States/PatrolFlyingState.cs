using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFlyingState : PatrolState
{
    public PatrolFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    Vector2 initialPos;
    Vector2 direction;
    Transform rayOrigin;
    Vector2[] rays;
    RaycastHit2D hit, hit1, hit2;
    int masks;
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");

    public override void OnStateEnter()
    {
        masks = enemiesLayer;
        initialPos = enemy.transform.position;
        direction = new Vector2(1, 0);
        rayOrigin = enemy.rightRayOrigin;
        rays = enemy.GetOtherRays(rayOrigin.position);
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
        if (enemy.NeedChangeState(enemy.detectionRange, 1 << LayerMask.NameToLayer("DetectionLayer")))
        {
            enemy.fsm.EnterNextState();
        }
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, 1);
    }
}
