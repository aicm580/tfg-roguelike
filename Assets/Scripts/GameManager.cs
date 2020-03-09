using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator mapGenerator;

    private GameObject enemiesHolder;

    private int level = 1;
    private string lvlName;

    private List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> enemiesPrefabs;
    
    
    void Start()
    {
        //Generamos el mapa del nivel
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.SetupMap();

        LoadLevelEnemiesArray();
        enemiesHolder = new GameObject("EnemiesHolder");
        CreateEnemies();
    }


    Vector3 RandomPosition(int i)
    {
        int randomIndex = Random.Range(0, mapGenerator.rooms[i].emptyPositions.Count);

        Vector3 randomPosition = new Vector3(mapGenerator.rooms[i].emptyPositions[randomIndex].x, mapGenerator.rooms[i].emptyPositions[randomIndex].y, 0f);
        mapGenerator.rooms[i].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos de la lista

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }


    void CreateEnemies()
    {
        //Vaciamos la lista de enemigos
        enemies.Clear();

        for (int i = 0; i < mapGenerator.rooms.Length; i++)
        {
            Debug.Log("Nº enemigos a generar en la sala " + i + ": " + mapGenerator.rooms[i].nEnemies);
            
            Vector3 position = RandomPosition(i);

            int randomEnemy = Random.Range(0, enemiesPrefabs.Count); //el máximo del Random.Range en enteros es exclusivo; el resultado máximo será enemiesPrefabs.Count-1

            GameObject enemy = Instantiate(enemiesPrefabs[randomEnemy], position, Quaternion.identity) as GameObject;
            enemy.GetComponent<Enemy>().pos = new Vector2(position.x, position.y);

            enemy.transform.parent = enemiesHolder.transform;

            enemies.Add(enemy);
        }
    }
    

    //El contenido del array de prefabs de los enemigos dependerá del nivel
    void LoadLevelEnemiesArray()
    {
        enemiesPrefabs.Clear();
        lvlName = "Level" + level;
        enemiesPrefabs = Resources.LoadAll<GameObject>(lvlName + "/Prefabs/Enemies").ToList();
    }
    
}
