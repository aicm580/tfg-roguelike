using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Move
{
    Right, Left, Up, Down,
}

public class FollowState : State
{
    public FollowState(Enemy enemy, StateType state) : base(enemy, state) { }

    protected Vector2 direction;
    protected Vector2? destination;

    protected bool hit, hit1, hit2;
    protected bool rightHit, leftHit, topHit, bottomHit;
    protected Move lastMove;

    protected void SetUpFollowRays()
    {
        hit = Physics2D.Raycast(enemy.rayOrigin, enemy.playerDirection, 0.8f, masks);
        hit1 = Physics2D.Raycast(enemy.rayOrigin1, enemy.playerDirection, 0.8f, masks);
        hit2 = Physics2D.Raycast(enemy.rayOrigin2, enemy.playerDirection, 0.8f, masks);
        rightHit = Physics2D.Raycast(enemy.rightRayOrigin.position, Vector2.right, 0.2f, masks);
        leftHit = Physics2D.Raycast(enemy.leftRayOrigin.position, Vector2.left, 0.2f, masks);
        topHit = Physics2D.Raycast(enemy.topRayOrigin.position, Vector2.up, 0.2f, masks);
        bottomHit = Physics2D.Raycast(enemy.bottomRayOrigin.position, Vector2.down, 0.2f, masks);
    }

    protected void SetDirection(Vector2 vector)
    {
        direction = vector;

        if (direction == Vector2.up)
            lastMove = Move.Up;
        else if (direction == Vector2.down)
            lastMove = Move.Down;
        else if (direction == Vector2.right)
            lastMove = Move.Right;
        else if (direction == Vector2.left)
            lastMove = Move.Left;
    }

    protected void CheckGiveUp()
    {
        if (Vector2.Distance(enemy.target.position, enemy.transform.position) >= enemy.giveUpRange)
            enemy.fsm.EnterPreviousState();
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, enemy.followSpeed);
    }

    public override void OnStateExit()
    {
        destination = null;
    }
}
