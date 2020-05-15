using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BreakableGenerator : MonoBehaviour
{
    private MapGenerator mapGenerator;

    [HideInInspector]
    public GameObject breakablesHolder;

    public List<GameObject> breakablesPrefabs;
    
    private float defaultMinRandom = 0.89f;
    public float breakableMinRandom;

    private int level;
    private string lvlName;

    public void InitRandom()
    {
        breakableMinRandom = defaultMinRandom;
    }

    private void SetupBreakables(int lvl)
    {
        mapGenerator = GetComponent<MapGenerator>();

        breakablesHolder = GameObject.Find("BreakablesHolder");
        if (breakablesHolder != null)
        {
            foreach (Transform child in breakablesHolder.transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            breakablesHolder = new GameObject("BreakablesHolder");
        }

        //Si en la última partida el último nivel cargado fue este, no cargamos de nuevo los prefabs.
        //Si no fue este, cargamos los prefabs del nivel.
        if (level != lvl)
        {
            level = lvl;
            LoadLevelBreakablesPrefabs();
        }
    }

    //El contenido del array de prefabs de los enemigos dependerá del nivel
    private void LoadLevelBreakablesPrefabs()
    {
        breakablesPrefabs.Clear();
        lvlName = "Level" + level;
        breakablesPrefabs = Resources.LoadAll<GameObject>(lvlName + "/Breakables").ToList();
    }

    public void GenerateBreakables(int lvl)
    {
        SetupBreakables(lvl);

        for (int i = 0; i < mapGenerator.rooms.Length - 1; i++) //la última sala no debe tener objetos rompibles
        {
            CreateBreakablesInRoom(i);
        }
    }

    private void CreateBreakablesInRoom(int i)
    {
        int nBreakables = 0;
        int breakablesCreated = 0;

        float random = Random.Range(0f, 1f);

        if (random > breakableMinRandom && random <= 0.97f)
            nBreakables = 1;
        else if (random > 0.97f)
            nBreakables = 2;

        while (breakablesCreated < nBreakables)
        {
            GameObject breakablePrefab;
            int randomEnemy = Random.Range(0, breakablesPrefabs.Count);
            breakablePrefab = breakablesPrefabs[randomEnemy];


            Vector3 randomPos = RandomPosition(i);

            GameObject breakable = Instantiate(breakablePrefab, randomPos, Quaternion.identity) as GameObject;
            breakable.transform.parent = breakablesHolder.transform;
            breakablesCreated++;
        }
    }

    private Vector3 RandomPosition(int room)
    {
        int randomIndex = Random.Range(0, mapGenerator.rooms[room].emptyPositions.Count);
        Vector3 randomPosition = mapGenerator.rooms[room].emptyPositions[randomIndex];
        mapGenerator.rooms[room].emptyPositions.RemoveAt(randomIndex); //ya no se trata de una posición vacía, así que la eliminamos

        return randomPosition; //devolvemos la posición en la que colocar un nuevo elemento
    }
}
