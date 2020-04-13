using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [HideInInspector]
    public int i = 0; 

    Enemy enemy;
    State currentState;

    [SerializeField]
    List<StateType> validStates;

    List<State> states = new List<State>();
    List<State> enemyStates = new List<State>();

    public void Awake()
    {
        enemy = this.GetComponent<Enemy>();
        currentState = null;
        states.Add(new IdleState(enemy, StateType.Idle));
        states.Add(new PatrolWalkingState(enemy, StateType.PatrolWalking));
        states.Add(new PatrolFlyingState(enemy, StateType.PatrolFlying));

        foreach (State state in states)
        {
            if (validStates.Contains(state.stateType))
            {
                enemyStates.Add(state);
            }
        }
        Debug.Log(enemyStates.Count);
    }

    public void Start()
    {
        EnterState(i);
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }


    #region STATE MANAGEMENT
    public void EnterState(int i)
    {
        State nextState = enemyStates[i];
        if (nextState == null)
        {
            return;
        }

        if (currentState != null)
        {
            currentState.OnStateExit();
        }

        currentState = nextState;
        currentState.OnStateEnter();
    }
    #endregion
}
