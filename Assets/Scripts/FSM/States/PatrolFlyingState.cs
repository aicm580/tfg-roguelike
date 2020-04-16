using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFlyingState : State
{
    public PatrolFlyingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void UpdateState()
    {
        /*
        //Comprobamos si el enemigo divisa al jugador
        if (enemy.NeedChangeState(enemy.detectionRange, 1 << LayerMask.NameToLayer("DetectionLayer")))
        {
            enemy.fsm.EnterNextState();
        }
        */
    }
}
