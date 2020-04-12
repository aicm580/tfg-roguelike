using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "FSM/States/Idle", order = 1)]
public class IdleState : AbstractFSMState
{
    public override bool EnterState()
    {
        base.EnterState();
        Debug.Log("Entering Idle State");
        return true;
    }

    public override void UpdateState()
    {
        Debug.Log("Updating idle state");
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting idle state");
        return true;
    }
}
