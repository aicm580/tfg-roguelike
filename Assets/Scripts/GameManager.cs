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
    public GameObject bossPrefab;
    
    
    void Start()
    {
        //Generamos el mapa del nivel
        mapGenerator = GetComponent<MapGenerator>();
        mapGenerator.SetupMap();

        LoadLevelEnemiesPrefabs();
        enemiesHolder = new GameObject("EnemiesHolder");
        GenerateEnemies();
    }


    //El contenido del array de prefabs de los enemigos dependerá del nivel
    void LoadLevelEnemiesPrefabs()
    {
        enemiesPrefabs.Clear();
        lvlName = "Level" + level;
        enemiesPrefabs = Resources.LoadAll<GameObject>(lvlName + "/Prefabs/Enemies/BasicEnemies").ToList();
        bossPrefab = Resources.Load<GameObject>(lvlName + "/Prefabs/Enemies/Boss");
    }


    Vector3 RandomPosition(int room, int friends)
    {
        int randomIndex;
        Vector3 randomPosition;

        do
        {
            randomIndex = Random.Range(0, mapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = mapGenerator.rooms[room].emptyPositions[randomIndex];
        } while (!EnoughSpaceForFriends(randomPosition, room, friends) || !NotEnemiesNearby(randomPosition, room));

        mapGenerator.rooms[room].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos de la lista

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }

    
    //Esta función permite comprobar si una posición concreta de una sala concreta está disponible
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
        if (friends > 0)
        {
            int freePos = 0;

            for (int i = -friends; i <= friends; i++)
            {
                for (int j = -friends; j <= friends; j++)
                {
                    Vector3 positionToCheck = new Vector3(randomPosition.x + i, randomPosition.y + j, 0);

                    if (positionToCheck != randomPosition)
                    {
                        if (CheckPosition(positionToCheck, room))
                        {
                            freePos++;

                            //Justo al llegar al nº de sitios necesarios, paramos el bucle
                            if (freePos >= friends) 
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        else
        {
            return true;
        }
        
    }


    //Esta función examina que no haya enemigos muy cerca de donde creamos el nuevo enemigo
    private bool NotEnemiesNearby(Vector3 randomPosition, int room)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                Vector3 positionToCheck = new Vector3(randomPosition.x + i, randomPosition.y + j, 0);

                if (positionToCheck != randomPosition && enemies.Exists(x => x.transform.position == positionToCheck))
                {
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
        int nEnemies;
        if (i == 0 && level == 1) //si se trata de la primera sala del primer nivel
        {
            nEnemies = 2;
        }    
        else if (i == mapGenerator.rooms.Length - 1) //si se trata de la última sala, solo debe tener 1 enemigo final
        {
            nEnemies = 1; 
        }
        else //si no se trata de la primera sala del primer nivel ni de la última sala de cualquier nivel       
        {
            int maxEnemies = (int)System.Math.Ceiling((mapGenerator.rooms[i].roomWidth + mapGenerator.rooms[i].roomHeight) / 3f);
            nEnemies = Random.Range(3, maxEnemies);
        }

        mapGenerator.rooms[i].nEnemies = nEnemies; //guardamos el nº de enemigos en la info de la sala
        Debug.Log("Nº enemigos a generar en la sala " + i + ": " + nEnemies);

        int enemiesCreated = 0;

        while (enemiesCreated < nEnemies)
        {
            int friends;
            GameObject enemyPrefab;

            //Trataremos diferente la generación de enemigos en la última sala que en el resto de salas,
            //puesto que en esta sala solo se encontrará 1 enemigo, el jefe final, que no tendrá amigos nunca
            if (i != mapGenerator.rooms.Length - 1)
            {
                int randomEnemy = Random.Range(0, enemiesPrefabs.Count); //el resultado máximo será enemiesPrefabs.Count-1
                enemyPrefab = enemiesPrefabs[randomEnemy];
                friends = enemyPrefab.GetComponent<Enemy>().friends.Randomize;
                Biome enemyBiome = enemiesPrefabs[randomEnemy].GetComponent<Enemy>().biome;


                //Si no se puede crear el enemigo junto con su nº de amigos calculado
                if (enemiesCreated + friends + 1 > nEnemies)
                {
                    //Si ni siquiera poniendo el nº de amigos al mínimo cumplimos con el nEnemies calculado
                    if (enemiesCreated + enemyPrefab.GetComponent<Enemy>().friends.minVal + 1 > nEnemies)
                    {
                        //Pasamos a la próxima iteración sin haber creado ningún enemigo.
                        //Este continue no romperá el juego, puesto que en cada nivel habrá como mínimo 1 enemigo con un mínimo de 0 amigos
                        continue;
                    }
                    else
                    {
                        //Cambiamos el nº de amigos al menor posible
                        friends = enemyPrefab.GetComponent<Enemy>().friends.minVal;
                    }
                }
            }
            else
            {
                friends = bossPrefab.GetComponent<Enemy>().friends.Randomize;
                enemyPrefab = bossPrefab;
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
            
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
                enemy.transform.position = position;
                enemy.transform.parent = enemiesHolder.transform;
                enemy.GetComponent<Enemy>().calculatedFriends = friends;
            
                enemies.Add(enemy);
                enemiesCreated++;
            }
        }        
    }   
}
