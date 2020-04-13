using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolFlyingState", menuName = "FSM/States/PatrolFlying", order = 3)]
public class PatrolFlyingState : AbstractFSMState
{
    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.Patrol;
    }

    public override bool EnterState()
    {
        enteredState = false;

        if (base.EnterState())
        {
            enteredState = true;
        }

        return enteredState;
    }

    public override void UpdateState()
    {
        if (enteredState)
        {
            Debug.Log("Updating patrol FLYING state");
            if (!enemy.GetComponentInChildren<Renderer>().isVisible)
            {
                finiteStateMachine.EnterState(FSMStateType.Idle);
            }
        }
    }
}
