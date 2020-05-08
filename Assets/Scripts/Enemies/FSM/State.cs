using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    PatrolWalking, PatrolFlying,
    FollowWalking, FollowFlying,
    RangeAttack, MeleeAttack,

    BossIntroState, BossOneShootState, UndergroundMove, GenerateChilds,
}

public abstract class State
{
    protected GameObject enemy;
    protected Animator animator;
    public StateType stateType;

    protected int masks;
    protected int blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
    protected int wallsLayer = 1 << LayerMask.NameToLayer("WallsLayer");
    protected int waterLayer = 1 << LayerMask.NameToLayer("WaterLayer");
    protected int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");
    protected int playerLayer = 1 << LayerMask.NameToLayer("PlayerLayer");

    public State(GameObject enemy, StateType stateType)
    {
        this.enemy = enemy;
        animator = enemy.GetComponentInChildren<Animator>();
        this.stateType = stateType;
    }

    public virtual void OnStateEnter() { }
    public virtual void OnStateExit() { }
    public virtual void FixedUpdateState() { }

    public virtual void UpdateState() { }  //hacer un método abstracto obliga a las clases derivadas a implementarlo
}
