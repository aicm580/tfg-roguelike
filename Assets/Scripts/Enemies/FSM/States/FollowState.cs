using UnityEngine;

public enum Move
{
    Right, Left, Up, Down,
}

public class FollowState : State
{
    public FollowState(GameObject enemy, StateType state) : base(enemy, state) { }

    protected Enemy enemyBehavior;
    protected Vector2 direction;
    protected Vector2? destination;

    protected bool hit, hit1, hit2;
    protected bool rightHit, leftHit, topHit, bottomHit;
    protected Move lastMove;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
    }

    protected void SetUpFollowRays()
    {
        hit = Physics2D.Raycast(enemyBehavior.rayOrigin, enemyBehavior.playerDirection, 0.8f, masks);
        hit1 = Physics2D.Raycast(enemyBehavior.rayOrigin1, enemyBehavior.playerDirection, 0.8f, masks);
        hit2 = Physics2D.Raycast(enemyBehavior.rayOrigin2, enemyBehavior.playerDirection, 0.8f, masks);
        rightHit = Physics2D.Raycast(enemyBehavior.rightRayOrigin.position, Vector2.right, 0.2f, masks);
        leftHit = Physics2D.Raycast(enemyBehavior.leftRayOrigin.position, Vector2.left, 0.2f, masks);
        topHit = Physics2D.Raycast(enemyBehavior.topRayOrigin.position, Vector2.up, 0.2f, masks);
        bottomHit = Physics2D.Raycast(enemyBehavior.bottomRayOrigin.position, Vector2.down, 0.2f, masks);
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
        if (Vector2.Distance(enemyBehavior.target.position, enemy.transform.position) >= enemyBehavior.giveUpRange)
            enemyBehavior.fsm.EnterPreviousState();
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive)
        {
            enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
            enemyBehavior.characterMovement.Move(direction, enemyBehavior.followSpeed);
        }
    }

    public override void OnStateExit()
    {
        destination = null;
    }
}
