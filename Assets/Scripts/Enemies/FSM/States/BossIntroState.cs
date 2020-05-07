using UnityEngine;

public class BossIntroState : State
{
    public BossIntroState(GameObject enemy, StateType state) : base(enemy, state) { }

    private Enemy enemyBehavior;

    public override void OnStateEnter()
    {
        masks = wallsLayer | playerLayer;
        enemyBehavior = enemy.GetComponent<Enemy>();
    }

    public override void UpdateState()
    {
        if (enemyBehavior.NeedChangeState(10, masks))
        {
            animator.SetTrigger("intro");
            enemy.GetComponent<BossHealth>().healthBar.EnableHealthBar();
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
