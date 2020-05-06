using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : State
{
    private CharacterShooting characterShooting;
    private Vector2 direction;
    private Quaternion angle;
    private Vector3 bulletOrigin = new Vector3();

    public RangeAttackState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        characterShooting = enemy.GetComponent<CharacterShooting>();
    }

    public override void UpdateState()
    {
        if (Vector2.Distance(enemy.transform.position, enemy.target.position) <= enemy.attackRange)
        {
            if (characterShooting.canShoot)
            {
                animator.SetBool("isAttacking", true);
                direction = enemy.GetDirectionToPlayer();
                bulletOrigin = enemy.transform.position + (Vector3)(direction * 0.46f);
                characterShooting.Shoot(bulletOrigin, direction, Quaternion.identity, DamageOrigin.NormalEnemy);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            enemy.fsm.EnterPreviousState();
        }
    }

    public override void OnStateExit()
    {
        animator.SetBool("isAttacking", false);
    }
}
