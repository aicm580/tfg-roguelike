using UnityEngine;

public class ChildGenerator : MonoBehaviour
{
    public GameObject childPrefab;
    private GameObject enemiesHolder;
    public int childsPerRound;
    public float timeBetweenRounds;

    public void Start()
    {
        enemiesHolder = GameObject.Find("EnemiesHolder");
    }

    public void GenerateChild(Vector2 pos)
    {
        GameObject child = Instantiate(childPrefab, pos, Quaternion.identity);
        child.transform.SetParent(enemiesHolder.transform);
    }
}
