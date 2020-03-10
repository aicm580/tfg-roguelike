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
        GenerateEnemies();
    }


    Vector3 RandomPosition(int i)
    {
        int randomIndex = Random.Range(0, mapGenerator.rooms[i].emptyPositions.Count);

        Vector3 randomPosition = new Vector3(mapGenerator.rooms[i].emptyPositions[randomIndex].x, mapGenerator.rooms[i].emptyPositions[randomIndex].y, 0f);
        mapGenerator.rooms[i].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos de la lista

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }


    void GenerateEnemies()
    {
        //Vaciamos la lista de enemigos
        enemies.Clear();

        for (int i = 0; i < mapGenerator.rooms.Length; i++)
        {
            CreateEnemiesInRoom(i);
            
        }
    }

    private void CreateEnemiesInRoom(int i)
    {
        int nEnemies = mapGenerator.rooms[i].nEnemies;
        Debug.Log("Nº enemigos a generar en la sala " + i + ": " + nEnemies);

        int enemiesCreated = 0;

        while (enemiesCreated < nEnemies)
        {
            int randomEnemy = Random.Range(0, enemiesPrefabs.Count); //el resultado máximo será enemiesPrefabs.Count-1
            int friends = enemiesPrefabs[randomEnemy].GetComponent<Enemy>().friends.Randomize; 
            Biome enemyBiome = enemiesPrefabs[randomEnemy].GetComponent<Enemy>().biome;

            //Si no se puede crear el enemigo junto con su nº de amigos calculado
            if (enemiesCreated + friends + 1 > nEnemies) 
            {
                //Si ni siquiera poniendo el nº de amigos al mínimo cumplimos con el nEnemies calculado
                if (enemiesCreated + enemiesPrefabs[randomEnemy].GetComponent<Enemy>().friends.minVal + 1 > nEnemies)
                {
                    //Pasamos a la próxima iteración sin haber creado ningún enemigo
                    continue;
                }
                else
                {
                    //Cambiamos el nº de amigos al menor posible
                    friends = enemiesPrefabs[randomEnemy].GetComponent<Enemy>().friends.minVal; 
                }
            }

            for (int j = 0; j < friends + 1; j++)
            {
                Vector3 position;
                if (j == 0)
                {
                    position = RandomPosition(i);
                }
                else
                {
                    position = RandomPosition(i);
                }
                
                GameObject enemy = Instantiate(enemiesPrefabs[randomEnemy], position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().pos = new Vector2(position.x, position.y);
                enemy.transform.parent = enemiesHolder.transform;
                enemies.Add(enemy);
                enemiesCreated++;
            }
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
