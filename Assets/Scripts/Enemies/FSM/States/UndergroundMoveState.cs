using UnityEngine;

public class UndergroundMoveState : State
{
    public UndergroundMoveState(GameObject enemy, StateType state) : base(enemy, state) { }

    private BossHealth bossHealth;
    private EnemiesGenerator enemiesGenerator;
    private MapGenerator mapGenerator;
    private BoxCollider2D boxCol;
    private float timer;
    private float delay = 1.2f;

    public override void OnStateEnter()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        enemiesGenerator = gameManager.GetComponent<EnemiesGenerator>();
        mapGenerator = gameManager.GetComponent<MapGenerator>();
        bossHealth = enemy.GetComponent<BossHealth>();
        boxCol = enemy.GetComponent<BoxCollider2D>();
        timer = 0;
    }

    public override void UpdateState()
    {
        if (bossHealth.currentHealth <= bossHealth.maxHealth - bossHealth.maxHealth / 3)
        {
            enemy.GetComponent<FiniteStateMachine>().EnterNextState();
        }

        if (timer >= delay)
        {
            enemy.transform.position = FindNewPosition();
            enemy.GetComponent<FiniteStateMachine>().EnterPreviousState(); 
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
            newPos = enemiesGenerator.RandomPositionWithOverlap(mapGenerator.rooms.Length - 1, boxCol.size);
        }
        while (Vector2.Distance(enemy.transform.position, newPos) < 0.5f);
            
        return newPos;
    }
}
