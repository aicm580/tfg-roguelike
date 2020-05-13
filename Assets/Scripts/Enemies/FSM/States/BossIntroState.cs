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
        if (enemyBehavior.NeedChangeState(enemyBehavior.detectionRange, masks))
        {
            animator.SetTrigger("appear");
            enemy.GetComponent<BossHealth>().healthBar.EnableHealthBar();
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }
    }
}
