using UnityEngine;

public class UndergroundMoveState : State
{
    public UndergroundMoveState(GameObject enemy, StateType state) : base(enemy, state) { }

    private BossHealth bossHealth;
    private Transform player;

    public override void OnStateEnter()
    {
        player = GameManager.instance.playerTransform;
        bossHealth = enemy.GetComponent<BossHealth>();
    }

    public override void UpdateState()
    {
        if (bossHealth.currentHealth <= bossHealth.maxHealth - bossHealth.maxHealth / 4)
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();


    }
    /*
    private Vector2 FindNewPosition()
    {
        Vector2 newPos;
        return newPos;
    }*/
}
