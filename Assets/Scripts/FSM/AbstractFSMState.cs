using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExecutionState
{
    NONE, ACTIVE, COMPLETED, TERMINATED,
}

public abstract class AbstractFSMState : ScriptableObject
{
    public ExecutionState exeState { get; protected set; }

    public virtual void OnEnable()
    {
        exeState = ExecutionState.NONE;
    }

    public virtual bool EnterState()
    {
       exeState = ExecutionState.ACTIVE;
        return true;
    }

    public abstract void UpdateState();

    public virtual bool ExitState()
    {
        exeState = ExecutionState.COMPLETED;
        return true;
    }
}