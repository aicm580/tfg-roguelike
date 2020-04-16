using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowFlyingState : State
{
    Vector2 direction;

    public FollowFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        animator.SetBool("isFollowing", true);
    }

    public override void UpdateState()
    {
        //Si el enemigo no está suficientemente cerca del jugador como para atacarlo, se acerca volando (sin tener en cuenta obstáculos)
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) > enemy.attackRange)
        {
            Vector2 direction = enemy.GetDirectionToPlayer();
        }
        //Si está suficientemente cerca, lo ataca
        else
        {
            enemy.fsm.EnterNextState();
        }
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, 1.9f);
    }
}
