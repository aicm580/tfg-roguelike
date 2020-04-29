using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    float attackSpeed = 3.5f;

    public MeleeAttackState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        Vector2 initialPosition = enemy.transform.position;
    }

    public override void UpdateState()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) > enemy.attackRange)
        {
            animator.SetBool("isAttacking", false);
            enemy.fsm.EnterPreviousState();
        }
        else
        {
            
        }
    }
}
