using System.Collections;
using UnityEngine;

public class ChildGenerator : MonoBehaviour
{
    public GameObject childPrefab;
    public GameObject childOriginPrefab;
    private GameObject enemiesHolder;
    public int childsPerRound;
    public float timeBetweenRounds;

    public void Start()
    {
        enemiesHolder = GameObject.Find("EnemiesHolder");
    }

    public void GenerateChild(Vector2 pos)
    {
        GameObject childOrigin = Instantiate(childOriginPrefab, pos, Quaternion.identity);
        Destroy(childOrigin, 1f);
        StartCoroutine(InstantiateChild(pos));
    }

    private IEnumerator InstantiateChild(Vector2 pos)
    {
        yield return new WaitForSeconds(0.8f);
        GameObject child = Instantiate(childPrefab, pos, Quaternion.identity);
        child.transform.SetParent(enemiesHolder.transform);
    }
}
