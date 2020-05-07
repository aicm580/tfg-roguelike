﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    Enemy enemyBehavior;
    float attackSpeed = 1.8f;
    bool isAttacking;
    Vector2 direction;

    public MeleeAttackState(GameObject enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        Vector2 initialPosition = enemy.transform.position;
        isAttacking = true;
    }

    public override void UpdateState()
    {
        if (isAttacking)
        {
            direction = enemyBehavior.GetDirectionToPlayer();
            if (Vector2.Distance(enemy.transform.position, enemyBehavior.target.position) <= 0.3f)
                isAttacking = false;
        }
        else
        {
            direction *= -1;
        }
           
        if (Vector2.Distance(enemy.transform.position, enemyBehavior.target.position) <= enemyBehavior.attackRange)
        {
            
        }
        else
        {
            animator.SetBool("isAttacking", false);
            enemyBehavior.fsm.EnterPreviousState();
        }
    }

    public override void FixedUpdateState()
    {
        enemyBehavior.characterMovement.Move(direction, attackSpeed);
    }
}
