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


    Vector3 RandomPosition(int room, int friends)
    {
        int randomIndex;
        Vector3 randomPosition;

        do
        {
            randomIndex = Random.Range(0, mapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = mapGenerator.rooms[room].emptyPositions[randomIndex];
        } while (!EnoughSpaceForFriends(randomPosition, room, friends) && NotEnemiesSurrounding(randomPosition, room));

        mapGenerator.rooms[room].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos de la lista

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }

    
    private bool CheckPosition(Vector3 positionToCheck, int room)
    {
        if (mapGenerator.rooms[room].emptyPositions.Contains(positionToCheck))
        {
            return true;
        }

        return false;
    }


    private bool EnoughSpaceForFriends(Vector3 randomPosition, int room, int friends)
    {
        int freePos = 0;

        for (int i = -3; i <= 3; i++)
        {
            for (int j = -3; j <= 3; j++)
            {
                Vector3 positionToCheck = new Vector3(randomPosition.x + i, randomPosition.y + j, 0);

                if (positionToCheck != randomPosition && CheckPosition(positionToCheck, room))
                {
                    freePos++;

                    if (freePos >= friends)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }


    private bool NotEnemiesSurrounding(Vector3 randomPosition, int room)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                Vector3 positionToCheck = new Vector3(randomPosition.x + i, randomPosition.y + j, 0);

                if (positionToCheck != randomPosition)
                {
                    if (!CheckPosition(positionToCheck, room))
                        Debug.Log("Enemies cannot be that close. Recalculating enemy position...");
                        return false;
                }
            }
        }

        return true;
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
                    //Pasamos a la próxima iteración sin haber creado ningún enemigo.
                    //Este continue no romperá el juego, puesto que en cada nivel habrá como mínimo 1 enemigo con un mínimo de 0 amigos
                    continue;
                }
                else
                {
                    //Cambiamos el nº de amigos al menor posible
                    friends = enemiesPrefabs[randomEnemy].GetComponent<Enemy>().friends.minVal; 
                }
            }

            Vector3 randomPos = RandomPosition(i, friends);

            for (int j = 0; j < friends + 1; j++)
            {
                Vector3 position;
                if (j == 0)
                {
                    position = randomPos;
                }
                else
                {
                    do
                    {
                        position.x = randomPos.x + Random.Range(-j, j);
                        position.y = randomPos.y + Random.Range(-j, j);
                        position.z = 0;

                    } while (!CheckPosition(position, i));

                    mapGenerator.rooms[i].emptyPositions.Remove(position);
                }
                
                GameObject enemy = Instantiate(enemiesPrefabs[randomEnemy], position, Quaternion.identity) as GameObject;
                enemy.GetComponent<Enemy>().pos = new Vector2(position.x, position.y);
                enemy.GetComponent<Enemy>().calculatedFriends = friends;
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
