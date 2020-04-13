using System;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public IdleState(Enemy enemy, StateType state) : base(enemy, state) { }

    private void Awake()
    {
        stateType = StateType.Idle;
    }

    public override void UpdateState()
    {
        if (enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            animator.SetBool("isPatrolling", true);
           // enemy.fsm.EnterState(StateType.Patrol);
        }
    }
}
