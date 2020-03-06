using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private GameObject enemiesHolder;

    private int level = 1;
    private List<GameObject> enemies = new List<GameObject>();

    public GameObject prefab;
    
    
    void Start()
    {
        //Vaciamos la lista de enemigos
        enemies.Clear();

        //Generamos el mapa del nivel
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.SetupMap(level);

        enemiesHolder = new GameObject("EnemiesHolder");

        CreateEnemiesAtRandom();
    }


    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range (0, mapGenerator.emptyPositions.Count);

        Vector3 randomPosition = new Vector3(mapGenerator.emptyPositions[randomIndex].x, mapGenerator.emptyPositions[randomIndex].y, 0f);
        mapGenerator.emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos de la lista

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }


    void CreateEnemiesAtRandom()
    {
        Vector3 position = RandomPosition();

        GameObject enemy = Instantiate(prefab, position, Quaternion.identity) as GameObject;

        enemy.transform.parent = enemiesHolder.transform;

        Debug.Log(enemy.GetComponent<Enemy>().enemyType);

        enemies.Add(enemy);
        Debug.Log("Nº de enemigos creados: " + enemies.Count);
    }
    
}
