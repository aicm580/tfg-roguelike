using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFlyingState : State
{
    public PatrolFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    private void Awake()
    {
        stateType = StateType.PatrolFlying;
    }

    public override void UpdateState()
    {
        if (enemy.GetComponentInChildren<Renderer>().isVisible)
        {
            
            
        }
    }
}
