using UnityEngine;

public class PatrolState : State
{
    public PatrolState(GameObject enemy, StateType state) : base(enemy, state) { }

    protected Enemy enemyBehavior;
    
    protected Vector2 direction;
    protected Vector2? destination;
    protected Transform rayOrigin;
    protected Vector2[] rays;
   
    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
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
        Transform rayOrigin = enemyBehavior.rightRayOrigin;

        if (direction.x == 1)
            rayOrigin = enemyBehavior.rightRayOrigin;
        else if (direction.x == -1)
            rayOrigin = enemyBehavior.leftRayOrigin;

        if (direction.y == 1)
            rayOrigin = enemyBehavior.topRayOrigin;
        else if (direction.y == -1)
            rayOrigin = enemyBehavior.bottomRayOrigin;

        return rayOrigin;
    }

    protected void FindNewDirection()
    {
        Vector2 dir = direction;
        direction = ChangePatrolDirection(dir);
        rayOrigin = ChangePatrolRayOrigin(direction);
        rays = enemyBehavior.GetOtherRays(rayOrigin.position);
    }
}
