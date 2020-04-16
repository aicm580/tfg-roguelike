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
        states.Add(new FollowWalkingState(enemy, StateType.FollowWalking));
        states.Add(new FollowFlyingState(enemy, StateType.FollowFlying));
        states.Add(new RangeAttackState(enemy, StateType.RangeAttack));
        states.Add(new MeleeAttackState(enemy, StateType.MeleeAttack));

        foreach (State state in states)
        {
            if (validStates.Contains(state.stateType))
            {
                enemyStates.Add(state);
            }
        }
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

    public void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdateState();
        }
    }

    #region STATE MANAGEMENT

    private void EnterState(int i)
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

    public void EnterNextState()
    {
        if (i < enemyStates.Count)
        {
            i++;
        }
        EnterState(i);
    }

    public void EnterPreviousState()
    {
        if (i > 0)
        {
            i--;
        }
        EnterState(i);
    }

    #endregion
}
