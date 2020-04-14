using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFlyingState : State
{
    public PatrolFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void UpdateState()
    {


        if (enemy.DetectPlayer())
        {
            animator.SetBool("isFollowing", true);
            enemy.fsm.EnterNextState();
        }
    }
}
