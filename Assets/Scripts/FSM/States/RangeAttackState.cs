using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : State
{
    private CharacterShooting characterShooting;
    private Vector2 direction;
    private Vector3 bulletOrigin = new Vector3();

    public RangeAttackState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        characterShooting = enemy.GetComponent<CharacterShooting>();
    }

    public override void UpdateState()
    {
        if (Vector2.Distance(animator.transform.position, enemy.target.position) <= enemy.attackRange)
        {
            direction = enemy.GetDirectionToPlayer();
            bulletOrigin = enemy.transform.position + (Vector3)(direction * 0.75f);
            characterShooting.Shoot(bulletOrigin, direction, Quaternion.identity, DamageOrigin.NormalEnemy);
        }
        else
        {
            animator.SetBool("isAttacking", false);
            enemy.fsm.EnterPreviousState();
        }
    }
}
