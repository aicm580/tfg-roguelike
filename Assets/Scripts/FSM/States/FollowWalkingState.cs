using System;
using System.Collections.Generic;
using UnityEngine;

public class FollowWalkingState : State
{
    public FollowWalkingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void UpdateState()
    {

        
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= enemy.attackRange)
        {
            animator.SetBool("isAttacking", true);
            enemy.fsm.EnterNextState();
        }
    }

    public override void FixedUpdateState()
    {
        
    }


}
