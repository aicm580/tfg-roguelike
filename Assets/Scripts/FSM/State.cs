using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle, PatrolWalking, PatrolFlying, FollowWalking, FollowFlying, RangeAttack, MeleeAttack,
}

public abstract class State
{
    protected Enemy enemy;
    protected Animator animator;
    public StateType stateType;

    public State(Enemy enemy, StateType stateType)
    {
        this.enemy = enemy;
        animator = enemy.GetComponentInChildren<Animator>();
        this.stateType = stateType;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
   
    public abstract void UpdateState();  //hacer un método abstracto obliga a las clases derivadas a implementarlo
}
