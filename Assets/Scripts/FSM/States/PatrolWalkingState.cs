using System;
using System.Collections.Generic;
using UnityEngine;

public class PatrolWalkingState : State
{
    Vector2 initialPos;
    Vector2 direction = new Vector2(1, 0);
    Transform rayOrigin;

    int blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
    int detectionLayer = 1 << LayerMask.NameToLayer("DetectionLayer");
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");
    int masks;

    public PatrolWalkingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        masks = blockingLayer | detectionLayer | enemiesLayer;
        initialPos = enemy.transform.position;
        rayOrigin = enemy.rayOrigin.transform;
        Debug.Log(rayOrigin.localPosition);
    }

    public override void UpdateState()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
        Debug.DrawRay(rayOrigin.position, direction, Color.green);

        if (hit || (Vector2.Distance(enemy.transform.position, initialPos) >= 3f))
        {
            direction *= -1;
            rayOrigin.localPosition *= -1;
            initialPos = enemy.transform.position;
        }    
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction);
    }
}
