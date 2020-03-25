using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private GameManager gameManager;

    private GameObject enemiesHolder;

    public List<GameObject> enemies = new List<GameObject>();

    public List<GameObject> enemiesPrefabs;
    public GameObject bossPrefab;

    private int level;
    private string lvlName;


    void SetupEnemies(int lvl)
    {
        mapGenerator = GetComponent<MapGenerator>();
        gameManager = GetComponent<GameManager>();
        
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
        enemiesPrefabs = Resources.LoadAll<GameObject>(lvlName + "/Prefabs/Enemies/BasicEnemies").ToList();
        bossPrefab = Resources.Load<GameObject>(lvlName + "/Prefabs/Enemies/Boss");
    }


    public void GenerateEnemies(int lvl)
    {
        SetupEnemies(lvl);

        for (int i = 0; i < mapGenerator.rooms.Length; i++)
        {
            CreateEnemiesInRoom(i);
        }
    }


    void CreateEnemiesInRoom(int i)
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
             int maxEnemies = mapGenerator.rooms[i].emptyPositions.Count / (mapGenerator.rooms[i].roomWidth - 1 + mapGenerator.rooms[i].roomHeight - 1);
             int minEnemies = (int)System.Math.Floor(maxEnemies / 2f);
             nEnemies = Random.Range(minEnemies, maxEnemies);
             Debug.Log("minEnemies: " + minEnemies + ", maxEnemies: " + maxEnemies);
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
             Vector3 lastPos = new Vector3();

             for (int j = 0; j < friends + 1; j++)
             {
                 Vector3 position;

                 if (j == 0)
                 {
                     position = randomPos;
                 }
                 else
                 {
                     int k = 1;
                     int attempt = 0;
                     do
                     {
                         position.x = lastPos.x + Random.Range(-k, k);
                         position.y = lastPos.y + Random.Range(-k, k);
                         position.z = 0;

                         attempt++;

                         if (attempt >= 5)
                         {
                             k++;
                         }

                     } while (!gameManager.CheckPosition(position, i));

                     mapGenerator.rooms[i].emptyPositions.Remove(position);
                 }

                 lastPos = position;

                 GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
                 enemy.transform.position = position;
                 enemy.transform.parent = enemiesHolder.transform;
                 enemy.GetComponent<Enemy>().calculatedFriends = friends;

                 enemies.Add(enemy);
                 enemiesCreated++;
             }
         }
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
                    || positionToCheck == gameManager.player.transform.position)
                {
                    return false;
                }
            }
        }
        return true;
    }


    Vector3 RandomPosition(int room, int friends)
    {
        int randomIndex;
        Vector3 randomPosition;

        do
        {
            randomIndex = Random.Range(0, mapGenerator.rooms[room].emptyPositions.Count);
            randomPosition = mapGenerator.rooms[room].emptyPositions[randomIndex];
        } while (!NotEnemiesNearby(randomPosition, room));

        mapGenerator.rooms[room].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }
}
