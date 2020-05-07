using UnityEngine;

public class ChildGenerator : MonoBehaviour
{
    public GameObject childPrefab;
    public int childsPerRound;
    public float timeBetweenRounds;

    public void GenerateChild(Vector2 pos)
    {
        GameObject child = Instantiate(childPrefab, pos, Quaternion.identity);
        child.transform.SetParent(transform);
    }
}
