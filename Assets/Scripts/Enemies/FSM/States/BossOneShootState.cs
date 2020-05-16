using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneShootState : State
{
    public BossOneShootState(GameObject enemy, StateType state) : base(enemy, state) { }

    private Enemy enemyBehavior;
    private CharacterShooting characterShooting;
    private float timer;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        characterShooting = enemy.GetComponent<CharacterShooting>();
        timer = 0;
    }

    public override void UpdateState()
    {
        if (timer < characterShooting.shootDelay)
        {
            timer += Time.deltaTime;
        }
        else
        {
            animator.SetTrigger("split");
            characterShooting.Shoot(enemy.transform.position, Vector2.zero, Quaternion.identity, enemyBehavior.dmgOriginType, enemyBehavior.enemyName);
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
