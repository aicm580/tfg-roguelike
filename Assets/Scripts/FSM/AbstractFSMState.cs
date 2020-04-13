using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExecutionState
{
    NONE, ACTIVE, COMPLETED, TERMINATED,
}

public enum FSMStateType
{
    Idle, Patrol, Follow, Attack,  
}

public abstract class AbstractFSMState : ScriptableObject
{
    protected Enemy enemy;
    protected FiniteStateMachine finiteStateMachine;

    public ExecutionState exeState { get; protected set; }
    public FSMStateType stateType { get; protected set; }
    public bool enteredState { get; protected set; }

    public virtual void OnEnable()
    {
        exeState = ExecutionState.NONE;
    }

    public virtual bool EnterState()
    {
        bool success = true;
        success = (enemy != null);

        exeState = ExecutionState.ACTIVE;
        
        return success;
    }

    public abstract void UpdateState();

    public virtual bool ExitState()
    {
        exeState = ExecutionState.COMPLETED;
        return true;
    }

    public virtual void SetExecutingFSM(FiniteStateMachine fsm)
    {
        if (fsm != null)
        {
            finiteStateMachine = fsm;
        }
    }

    public virtual void SetExecutingEnemy(Enemy currentEnemy)
    {
        if (currentEnemy != null)
        {
            enemy = currentEnemy;
        }
    }
}