using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackState : State
{
    public RangeAttackState(GameObject enemy, StateType state) : base(enemy, state) { }

    private CharacterShooting characterShooting;
    private Enemy enemyBehavior;
    private Vector2 direction;
    private Quaternion angle;
    private Vector3 bulletOrigin = new Vector3();

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        characterShooting = enemy.GetComponent<CharacterShooting>();
    }

    public override void UpdateState()
    {
        if (Vector2.Distance(enemy.transform.position, enemyBehavior.target.position) <= enemyBehavior.attackRange)
        {
            if (characterShooting.canShoot)
            {
                animator.SetBool("isAttacking", true);
                direction = enemyBehavior.GetDirectionToPlayer();
                bulletOrigin = enemy.transform.position + (Vector3)(direction * 0.46f);
                characterShooting.Shoot(bulletOrigin, direction, Quaternion.identity, enemyBehavior.dmgOriginType, enemyBehavior.enemyName);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            enemyBehavior.fsm.EnterPreviousState();
        }
    }

    public override void OnStateExit()
    {
        animator.SetBool("isAttacking", false);
    }
}
