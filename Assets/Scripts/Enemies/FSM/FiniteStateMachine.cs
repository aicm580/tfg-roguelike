using System.Collections.Generic;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [HideInInspector]
    public int i = 0; 

    GameObject enemy;
    State currentState;

    [SerializeField]
    List<StateType> validStates;

    List<State> states = new List<State>();
    List<State> enemyStates = new List<State>();

    public void Awake()
    {
        enemy = this.gameObject;
        currentState = null;
        states.Add(new IdleState(enemy, StateType.Idle));
        states.Add(new PatrolWalkingState(enemy, StateType.PatrolWalking));
        states.Add(new PatrolFlyingState(enemy, StateType.PatrolFlying));
        states.Add(new FollowWalkingState(enemy, StateType.FollowWalking));
        states.Add(new FollowFlyingState(enemy, StateType.FollowFlying));
        states.Add(new RangeAttackState(enemy, StateType.RangeAttack));
        states.Add(new MeleeFlyingAttackState(enemy, StateType.MeleeFlyingAttack));
        states.Add(new MeleeWalkingAttackState(enemy, StateType.MeleeWalkingAttack));

        states.Add(new BossIntroState(enemy, StateType.BossIntroState));
        states.Add(new BossShootState(enemy, StateType.BossShootState));
        states.Add(new UndergroundMoveState(enemy, StateType.UndergroundMove));
        states.Add(new GenerateChildsState(enemy, StateType.GenerateChilds));
    }

    public void Start()
    {
        foreach (StateType s in validStates)
        {
            State state = states.Find(x => x.stateType == s);
            if (state != null)
                enemyStates.Add(state);
        }

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
