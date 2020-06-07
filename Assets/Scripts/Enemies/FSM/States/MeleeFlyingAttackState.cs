using UnityEngine;

public class MeleeFlyingAttackState : State
{
    public MeleeFlyingAttackState(GameObject enemy, StateType state) : base(enemy, state) { }

    Enemy enemyBehavior;
    Vector2 direction;
    Vector2? destination;
    float attackSpeed = 2.1f;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = playerLayer | enemiesLayer | wallsLayer;
        destination = null;
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            if (Vector2.Distance(enemy.transform.position, enemyBehavior.target.position) > enemyBehavior.attackRange)
                enemyBehavior.fsm.EnterPreviousState();

            if (destination.HasValue == false || Vector2.Distance(enemy.transform.position, destination.Value) <= 0.09f)
            {
                direction = Vector2.zero;

                //Si el enemigo está a rango de ataque y nada se interpone entre él y el jugador
                if (enemyBehavior.NeedChangeState(enemyBehavior.attackRange, masks))
                {
                    direction = enemyBehavior.playerDirection;
                    enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
                }
                 
                destination = enemy.transform.position + new Vector3(direction.x * 0.25f, direction.y * 0.25f, 0);
            }
        }
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive &&
            GameManager.instance.enemiesActive) 
            enemyBehavior.characterMovement.Move(direction, enemyBehavior.followSpeed + attackSpeed);
    }
}
