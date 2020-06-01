using UnityEngine;

public class RangeAttackState : State
{
    public RangeAttackState(GameObject enemy, StateType state) : base(enemy, state) { }

    private CharacterShooting characterShooting;
    private Enemy enemyBehavior;
    private Vector2 direction;
    private Vector3 bulletOrigin = new Vector3();

    public override void OnStateEnter()
    {
        enemyBehavior = enemy.GetComponent<Enemy>();
        characterShooting = enemy.GetComponent<CharacterShooting>();
    }

    public override void UpdateState()
    {
        if (GameManager.instance.enemiesActive)
        {
            float distance = Vector2.Distance(enemy.transform.position, enemyBehavior.target.position);
            if (distance <= enemyBehavior.attackRange && characterShooting.canShoot &&
                !enemyBehavior.target.GetComponent<PlayerInputController>().abilityActive)
            {
                direction = enemyBehavior.GetDirectionToPlayer();
                enemyBehavior.SetAnimatorDirection(direction.x, direction.y);
                bulletOrigin = enemy.transform.position;
                characterShooting.Shoot(bulletOrigin, direction, 0.65f, Quaternion.identity, enemyBehavior.dmgOriginType, enemyBehavior.enemyName);
            }
            else if (distance > enemyBehavior.attackRange)
            {
                enemyBehavior.fsm.EnterPreviousState();
            }
        }
    }
}
