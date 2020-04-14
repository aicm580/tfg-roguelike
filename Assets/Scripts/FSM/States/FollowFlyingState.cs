using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowFlyingState : State
{
    private CharacterMovement characterMovement;


    public FollowFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        characterMovement = enemy.GetComponent<CharacterMovement>();
    }

    public override void UpdateState()
    {
        //Si el enemigo no está suficientemente cerca del jugador como para atacarlo, se acerca volando (sin tener en cuenta obstáculos)
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) > enemy.attackRange)
        {
            Vector2 direction = enemy.GetDirectionToPlayer();
            characterMovement.Move(direction);
        }
        //Si está suficientemente cerca, lo ataca
        else
        {
            animator.SetBool("isAttacking", true);
            enemy.fsm.EnterNextState();
        }
    }   
}
