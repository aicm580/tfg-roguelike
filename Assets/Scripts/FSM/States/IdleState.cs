using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleState", menuName = "FSM/States/Idle", order = 1)]
public class IdleState : AbstractFSMState
{
    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.Idle;
    }

    public override bool EnterState()
    {
        enteredState = base.EnterState();

        if (enteredState)
        {
            Debug.Log("Entering Idle State");
            Debug.Log(enemy.GetComponentInChildren<Renderer>());
        }
        
        return enteredState;
    }

    public override void UpdateState()
    {
        if (enteredState)
        {
            if (enemy.GetComponentInChildren<Renderer>().isVisible)
            {
                Debug.Log("VISIBLE!!!!");
                enemy.GetComponentInChildren<Animator>().SetBool("isPatrolling", true);
                finiteStateMachine.EnterState(FSMStateType.Patrol);
            }
        }
    }

    public override bool ExitState()
    {
        base.ExitState();
        Debug.Log("Exiting idle state");
        return true;
    }
}
