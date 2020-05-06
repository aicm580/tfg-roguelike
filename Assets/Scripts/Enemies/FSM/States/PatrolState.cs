using UnityEngine;

public class PatrolState : State
{
    public PatrolState(Enemy enemy, StateType state) : base(enemy, state) { }

    protected Vector2 initialPos;
    protected Vector2 direction;
    protected Transform rayOrigin;
    protected Vector2[] rays;
   
    protected void InitPatrol()
    {
        initialPos = enemy.transform.position;
        direction = new Vector2(1, 0);
        rayOrigin = enemy.rightRayOrigin;
        rays = enemy.GetOtherRays(rayOrigin.position);
    }

    protected Vector2 ChangePatrolDirection(Vector2 direction)
    {
        float random = Random.Range(0f, 1f);
        if (random <= 0.5f)
        {
            if (direction.x == 0)
                direction.x = 1;
            else
                direction.x *= -1;

            direction.y = 0;
        }
        else
        {
            if (direction.y == 0)
                direction.y = 1;
            else
                direction.y *= -1;

            direction.x = 0;
        }

        return direction;
    }

    protected Transform ChangePatrolRayOrigin(Vector2 direction)
    {
        Transform rayOrigin = enemy.rightRayOrigin;

        if (direction.x == 1)
            rayOrigin = enemy.rightRayOrigin;
        else if (direction.x == -1)
            rayOrigin = enemy.leftRayOrigin;

        if (direction.y == 1)
            rayOrigin = enemy.topRayOrigin;
        else if (direction.y == -1)
            rayOrigin = enemy.bottomRayOrigin;

        return rayOrigin;
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, 1);
    }
}
