using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    private MapGenerator mapGenerator;

    [HideInInspector]
    public GameObject enemiesHolder;
    [HideInInspector]
    public List<GameObject> enemies = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> enemiesPrefabs;
    [HideInInspector]
    public GameObject bossPrefab;

    private int level;
    private string lvlName;

    private bool possible;

    void SetupEnemies(int lvl)
    {
        mapGenerator = GetComponent<MapGenerator>();
        
        if (GameObject.Find("EnemiesHolder"))
        {
            enemiesHolder = GameObject.Find("EnemiesHolder");
            foreach (Transform child in enemiesHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            enemiesHolder = new GameObject("EnemiesHolder");
        }

        //Vaciamos la lista de enemigos
        enemies.Clear();

        //Si en la última partida el último nivel cargado fue este, no cargamos de nuevo los prefabs.
        //Si no fue este, cargamos los prefabs del nivel.
        if (level != lvl) 
        {
            level = lvl;
            LoadLevelEnemiesPrefabs();
        }
    }

    //El contenido del array de prefabs de los enemigos dependerá del nivel
    void LoadLevelEnemiesPrefabs()
    {
        enemiesPrefabs.Clear();
        lvlName = "Level" + level;
        enemiesPrefabs = Resources.LoadAll<GameObject>(lvlName + "/Enemies/BasicEnemies").ToList();
        bossPrefab = Resources.Load<GameObject>(lvlName + "/Enemies/Boss");
    }

    public void GenerateEnemies(int lvl)
    {
        SetupEnemies(lvl);

        for (int i = 0; i < MapGenerator.rooms.Length; i++)
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
         else if (i == MapGenerator.rooms.Length - 1) //si se trata de la última sala, solo debe tener 1 enemigo final
         {
             nEnemies = 1;
         }
         else //si no se trata de la primera sala del primer nivel ni de la última sala de cualquier nivel       
         {
             int maxEnemies = (int)(MapGenerator.rooms[i].emptyPositions.Count / ((MapGenerator.rooms[i].roomWidth - 2 + MapGenerator.rooms[i].roomHeight - 2) / 2f));
             int minEnemies = (int)Mathf.Round(maxEnemies / 1.7f);
             nEnemies = Random.Range(minEnemies, maxEnemies);
             //Debug.Log("minEnemies: " + minEnemies + ", maxEnemies: " + maxEnemies);
             //Debug.Log("Nº enemigos a generar en la sala " + i + ": " + nEnemies);
         }

         MapGenerator.rooms[i].nEnemies = nEnemies; //guardamos el nº de enemigos en la info de la sala

         int enemiesCreated = 0;

         while (enemiesCreated < nEnemies)
         {
             int friends = 0;
             possible = true; 
             Biome enemyBiome;
             Vector3 position;

             //Trataremos diferente la generación de enemigos en la última sala que en el resto de salas,
             //puesto que en esta sala solo se encontrará 1 enemigo, el jefe final, que no tendrá amigos nunca
             if (i != MapGenerator.rooms.Length - 1)
             {
                int randomEnemy = Random.Range(0, enemiesPrefabs.Count); //el resultado máximo será enemiesPrefabs.Count-1
                GameObject enemyPrefab = enemiesPrefabs[randomEnemy];
                friends = enemyPrefab.GetComponent<Enemy>().friends.Randomize;
                enemyBiome = enemyPrefab.GetComponent<Enemy>().biome;

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
                Vector3 randomPos = RandomPosition(i);

                if (!possible) //si ya no es posible colocar enemigos en una posición random, paramos de crearlos
                {
                    break;
                }
                   
                Vector3 lastPos = new Vector3();
                
                for (int j = 0; j < friends + 1; j++)
                {
                    if (j == 0)
                    {
                        position = randomPos;
                    }
                    else
                    {
                        int k = 1;
                        int attempt = 0;
                        bool breakLoop = false; 
                        do
                        {
                            position.x = lastPos.x + Random.Range(-k, k);
                            position.y = lastPos.y + Random.Range(-k, k);
                            position.z = 0;

                            attempt++;

                            if (attempt >= 12)
                            {
                                k++;
                                attempt = 0;
                            }

                            if (k > 3)
                            {
                                breakLoop = true;
                                break;
                            }

                        } while (!mapGenerator.CheckPosition(position, i));

                        if (breakLoop)
                        {
                            Debug.Log("Enemigos a generar en la sala " + i + ": " + nEnemies);
                            Debug.Log("Nº amigos a generar: " + friends);
                            friends = j - 1;
                            Debug.Log("Nº de amigos cambiado a " + friends);
                            break;
                        }
                         
                        MapGenerator.rooms[i].emptyPositions.Remove(position);
                    }

                    lastPos = position;
                    InstantiateEnemy(enemyPrefab, position, false, friends);
                    enemiesCreated++;
                }
             }
             else
             {
                BoxCollider2D boxCol = bossPrefab.GetComponent<BoxCollider2D>();
                position = RandomPositionWithOverlap(i, boxCol.size);
                MapGenerator.rooms[i].emptyPositions.Remove(position);
                InstantiateEnemy(bossPrefab, position, true, friends);
                enemiesCreated++;
             }
         }
    }

    private void InstantiateEnemy(GameObject prefab, Vector3 position, bool isBoss, int friends)
    {
        GameObject enemy = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        enemy.transform.parent = enemiesHolder.transform;
        enemy.GetComponent<Enemy>().calculatedFriends = friends;

        if (!isBoss)
            enemies.Add(enemy);
    }

    //Esta función examina que no haya enemigos muy cerca de donde creamos el nuevo enemigo, y que tampoco el player esté muy cerca
    private bool NotEnemiesNearby(Vector3 randomPosition, int room)
    {
        for (int i = -2; i <= 2; i++)
        {
            for (int j = -2; j <= 2; j++)
            {
                Vector3 positionToCheck = new Vector3(randomPosition.x + i, randomPosition.y + j, 0);

                if (positionToCheck != randomPosition && enemies.Exists(x => x.transform.position == positionToCheck)
                    || positionToCheck == GameManager.instance.playerTransform.position)
                {
                    return false;
                }
            }
        }
        return true;
    }

    Vector3 RandomPosition(int room)
    {
        int attempt = 0;
        int randomIndex;
        Vector3 randomPosition;

        do
        {
            randomIndex = Random.Range(0, MapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = MapGenerator.rooms[room].emptyPositions[randomIndex];

            attempt++;
            if (attempt > 50)
            {
                possible = false;
                break;
            }
        } while (!NotEnemiesNearby(randomPosition, room));

        MapGenerator.rooms[room].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }

    public Vector3 RandomPositionWithOverlap(int room, Vector2 boxSize)
    {
        int randomIndex;
        Vector3 randomPosition;
        Collider2D col;

        do
        {
            randomIndex = Random.Range(0, MapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = MapGenerator.rooms[room].emptyPositions[randomIndex];

            col = Physics2D.OverlapBox(randomPosition, new Vector2(boxSize.x, boxSize.y), 0);

        } while (col != null);

        return randomPosition; //devolvemos la posición en la que colocar un nuevo enemigo
    }
}
