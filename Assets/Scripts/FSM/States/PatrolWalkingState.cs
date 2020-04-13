using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWalkingState : State
{
    public PatrolWalkingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void UpdateState()
    {
        if (enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            
            
        }
    }
}
