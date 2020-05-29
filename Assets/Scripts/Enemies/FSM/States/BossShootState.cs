using UnityEngine;

public class BossShootState : State
{
    public BossShootState(GameObject enemy, StateType state) : base(enemy, state) { }

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
            characterShooting.Shoot(enemy.transform.position, Vector2.zero, 0.85f, Quaternion.identity, enemyBehavior.dmgOriginType, enemyBehavior.enemyName);
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
