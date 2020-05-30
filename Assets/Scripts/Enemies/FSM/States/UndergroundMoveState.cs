using UnityEngine;

public class UndergroundMoveState : State
{
    public UndergroundMoveState(GameObject enemy, StateType state) : base(enemy, state) { }

    private BossHealth bossHealth;
    private EnemiesGenerator enemiesGenerator;
    private BoxCollider2D boxCol;
    private float timer;
    private float delay = 1.1f;
    private bool disappear = false;

    public override void OnStateEnter()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        enemiesGenerator = gameManager.GetComponent<EnemiesGenerator>();
        bossHealth = enemy.GetComponent<BossHealth>();
        boxCol = enemy.GetComponent<BoxCollider2D>();
        timer = 0;
        disappear = false;
    }

    public override void UpdateState()
    {
        if (timer >= delay * 2)
        {
            enemy.transform.position = FindNewPosition();
            animator.SetTrigger("appear");

            if (bossHealth.currentHealth <= bossHealth.maxHealth - bossHealth.maxHealth / 2f)
                enemy.GetComponent<FiniteStateMachine>().EnterNextState();
            else
                enemy.GetComponent<FiniteStateMachine>().EnterPreviousState(); 
        }
        else if (timer >= delay && !disappear)
        {
            animator.SetTrigger("disappear");
            disappear = true;
        }
        else
        {
            timer += Time.deltaTime; 
        }
    }
    
    private Vector2 FindNewPosition()
    {
        Vector2 newPos;
        do
        {
            newPos = enemiesGenerator.RandomPositionWithOverlap(MapGenerator.rooms.Length - 1, boxCol.size + new Vector2(0.5f, 0.5f));
        }
        while (Vector2.Distance(enemy.transform.position, newPos) < 0.5f);
            
        return newPos;
    }
}
