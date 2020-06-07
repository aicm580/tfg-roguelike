using UnityEngine;

public class MeleeWalkingAttackState : State
{
    public MeleeWalkingAttackState(GameObject enemy, StateType state) : base(enemy, state) { }

    Enemy enemyBehavior;
    Vector2 direction;
    Vector2? destination;
    float attackSpeed = 0.65f;

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        masks = playerLayer | enemiesLayer | wallsLayer | waterLayer | blockingLayer;
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
                    bool move = true;
                    RaycastHit2D hit1 = Physics2D.Raycast(enemyBehavior.rayOrigin1, enemyBehavior.playerDirection, 0.4f, masks);
                    Debug.DrawRay(enemyBehavior.rayOrigin1, enemyBehavior.playerDirection * 0.4f, Color.gray);
                    RaycastHit2D hit2 = Physics2D.Raycast(enemyBehavior.rayOrigin2, enemyBehavior.playerDirection, 0.4f, masks);
                    if (hit1)
                    {
                        if (hit1.collider.tag != "Player")
                            move = false;
                    }

                    if (hit2)
                    {
                        if (hit2.collider.tag != "Player")
                            move = false;
                    }

                    if (move)
                    {
                        direction = enemyBehavior.playerDirection;
                        enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
                    }
                }

                destination = enemy.transform.position + new Vector3(direction.x * 0.2f, direction.y * 0.2f, 0);
            }
        }
    }

    public override void FixedUpdateState()
    {
        if (!enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive &&
            GameManager.instance.enemiesActive)
        {
            animator.SetBool("isMoving", direction.sqrMagnitude > 0 ? true : false);
            enemyBehavior.characterMovement.Move(direction, enemyBehavior.followSpeed + attackSpeed);
        }
        else
        {
            animator.SetBool("isMoving", false); //si la habilidad especial está activada, el enemigo no debe moverse
        }
    }
}
