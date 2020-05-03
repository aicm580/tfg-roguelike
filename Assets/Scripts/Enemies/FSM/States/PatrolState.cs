using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : State
{
    public PatrolState(Enemy enemy, StateType state) : base(enemy, state) { }

    Vector2 initialPos;
    Vector2 direction;
    Transform rayOrigin;
    Vector2[] rays;
    RaycastHit2D hit, hit1, hit2;
    int masks;
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");

    public Vector2 ChangePatrolDirection(Vector2 direction)
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

    public Transform ChangePatrolRayOrigin(Vector2 direction)
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
}
