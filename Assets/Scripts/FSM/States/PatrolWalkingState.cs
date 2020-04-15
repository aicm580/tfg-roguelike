using UnityEngine;

public class PatrolWalkingState : State
{
    Vector2 initialPos;
    Vector2 direction;
    Transform rayOrigin;
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
        rayOrigin = enemy.rayOrigin.transform;
    }

    public override void UpdateState()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, direction, 1, masks);
        Debug.DrawRay(rayOrigin.position, direction, Color.green);

        if (hit || (Vector2.Distance(enemy.transform.position, initialPos) >= 1.5f))
        {
            float random = Random.Range(0f, 1f);
            if (random <= 0.5f)
            {
                if (direction.x == 0)
                {
                    direction.x = 1;
                    rayOrigin.localPosition = new Vector3(0.35f, 0, 0);
                }
                else
                {
                    direction.x *= -1;
                    rayOrigin.localPosition = new Vector3(rayOrigin.localPosition.x * -1, 0, 0);
                }
                direction.y = 0;
            }
            else
            {
                if (direction.y == 0)
                {
                    direction.y = 1;
                    rayOrigin.localPosition = new Vector3(0, 0.35f, 0);
                } else
                {
                    direction.y *= -1;
                    rayOrigin.localPosition = new Vector3(0, rayOrigin.localPosition.y * -1, 0);
                }
                direction.x = 0;
            }
            enemy.SetAnimatorDirection(direction.x, direction.y);
            initialPos = enemy.transform.position;
        }    
    }

    public override void FixedUpdateState()
    {
        enemy.characterMovement.Move(direction, 1);
    }
}
