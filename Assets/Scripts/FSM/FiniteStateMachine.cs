using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    AbstractFSMState currentState;

    [SerializeField]
    List<AbstractFSMState> validStates;
    Dictionary<FSMStateType, AbstractFSMState> fsmStates;

    public void Awake()
    {
        currentState = null;

        Enemy enemy = GetComponent<Enemy>();

        fsmStates = new Dictionary<FSMStateType, AbstractFSMState>();
        foreach (AbstractFSMState state in validStates)
        {
            state.SetExecutingFSM(this);
            state.SetExecutingEnemy(enemy);
            fsmStates.Add(state.stateType, state);
        }
    }

    public void Start()
    {
        EnterState(FSMStateType.Idle);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }


    #region STATE MANAGEMENT
    public void EnterState(AbstractFSMState nextState)
    {
        if (nextState == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = nextState;
        currentState.EnterState();
    }

    public void EnterState(FSMStateType stateType)
    {
        if (fsmStates.ContainsKey(stateType))
        {
            AbstractFSMState nextState = fsmStates[stateType];
            EnterState(nextState);
        }
    }
    #endregion
}
