using UnityEngine;

public class GenerateChildsState : State
{
    public GenerateChildsState(GameObject enemy, StateType state) : base(enemy, state) { }

    private ChildGenerator childGenerator;
    private MapGenerator mapGenerator;

    private float timer;
    private bool roundActive = true;
    private Vector2 randomPos;

    public override void OnStateEnter()
    {
        mapGenerator = GameObject.Find("GameManager").GetComponent<MapGenerator>();
        childGenerator = enemy.GetComponent<ChildGenerator>();
        masks = blockingLayer | enemiesLayer | playerLayer;
    }

    public override void UpdateState()
    {
        if (roundActive)
        {
            for (int i = 0; i < childGenerator.childsPerRound; i++)
            {
                randomPos = RandomPosition(mapGenerator.rooms.Length - 1);
                childGenerator.GenerateChild(randomPos);
            }

            roundActive = false;
            timer = 0;
        }
        else if (timer < childGenerator.timeBetweenRounds)
        {
            timer += Time.deltaTime;
        }
        else
        {
            roundActive = true;
        }
    }


    private Vector3 RandomPosition(int room)
    {
        int randomIndex; 
        Vector3 randomPosition;
        Collider2D col;

        do
        {
            randomIndex = Random.Range(0, mapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = mapGenerator.rooms[room].emptyPositions[randomIndex];

            col = Physics2D.OverlapBox(randomPosition, new Vector2(0.9f, 0.9f), 0, masks);

        } while (col != null);
        
        return randomPosition; //devolvemos la posición en la que colocar un nuevo enemigo
    }
}
