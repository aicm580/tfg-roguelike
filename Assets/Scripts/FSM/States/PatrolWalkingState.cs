using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatrolWalkingState", menuName = "FSM/States/PatrolWalking", order = 2)]
public class PatrolWalkingState : AbstractFSMState
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
            Debug.Log("Updating patrol state");
            if (!enemy.GetComponentInChildren<Renderer>().isVisible)
            {
                //finiteStateMachine.EnterState(FSMStateType.Idle);
            }
        }
    }
}
