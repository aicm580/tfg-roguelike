using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackState : State
{
    float attackSpeed = 3.8f;
    bool isAttacking;
    Vector2 direction;

    public MeleeAttackState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        Vector2 initialPosition = enemy.transform.position;
        isAttacking = true;
    }

    public override void UpdateState()
    {
        if (isAttacking)
        {
            direction = enemy.GetDirectionToPlayer();
            if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= 0.3f)
                isAttacking = false;
        }
        else
        {
            direction *= -1;
        }
           
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= enemy.attackRange)
        {
            
        }
        else
        {
            animator.SetBool("isAttacking", false);
            enemy.fsm.EnterPreviousState();
            Debug.Log("enter previous");
        }
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, attackSpeed);
    }
}
