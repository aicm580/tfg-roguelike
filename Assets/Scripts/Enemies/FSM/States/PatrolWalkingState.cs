using UnityEngine;

public class PatrolWalkingState : State
{
    Vector2 initialPos;
    Vector2 direction;
    Transform rayOrigin;
    Vector2[] rays;
    RaycastHit2D hit, hit1, hit2;
    int masks;
    int blockingLayer = 1 << LayerMask.NameToLayer("BlockingLayer");
    int detectionLayer = 1 << LayerMask.NameToLayer("DetectionLayer");
    int enemiesLayer = 1 << LayerMask.NameToLayer("EnemiesLayer");
   

    public PatrolWalkingState(Enemy enemy, StateType state) : base(enemy, state) { }

    public override void OnStateEnter()
    {
        masks = blockingLayer | detectionLayer | enemiesLayer;
        initialPos = enemy.transform.position;
        direction = new Vector2(1, 0);
        rayOrigin = enemy.rightRayOrigin;
        rays = enemy.GetOtherRays(rayOrigin.position);
    }

    public override void UpdateState()
    {
        hit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
        hit1 = Physics2D.Raycast(rays[0], direction, 1, masks);
        hit2 = Physics2D.Raycast(rays[1], direction, 1, masks);

        if (hit || hit1 || hit2 || (Vector2.Distance(enemy.transform.position, initialPos) >= 1.5f))
        {
            float random = Random.Range(0f, 1f);
            if (random <= 0.5f)
            {
                if (direction.x == 0)
                {
                    direction.x = 1;
                    rayOrigin = enemy.rightRayOrigin;
                    
                }
                else
                {
                    direction.x *= -1;
                    if (direction.x == 1)
                        rayOrigin = enemy.rightRayOrigin;
                    else
                        rayOrigin = enemy.leftRayOrigin;
                }
                direction.y = 0;
            }
            else
            {
                if (direction.y == 0)
                {
                    direction.y = 1;
                    rayOrigin = enemy.topRayOrigin;
                } else
                {
                    direction.y *= -1;
                    if (direction.y == 1)
                        rayOrigin = enemy.topRayOrigin;
                    else
                        rayOrigin = enemy.bottomRayOrigin;
                }
                direction.x = 0;
            }
            
            initialPos = enemy.transform.position;

            //Antes de cambiar la dirección del Animator, comprobamos que la nueva dirección sea válida
            RaycastHit2D newHit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
            if (!newHit)
                enemy.SetAnimatorDirection(direction.x, direction.y);
        }
        rays = enemy.GetOtherRays(rayOrigin.position);

        //Comprobamos si el enemigo divisa al jugador
        if (enemy.NeedChangeState(enemy.detectionRange, 1 << LayerMask.NameToLayer("DetectionLayer")) && GameManager.instance.enemiesActive)
        {
            enemy.fsm.EnterNextState();
        }
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, 1);
    }
}
