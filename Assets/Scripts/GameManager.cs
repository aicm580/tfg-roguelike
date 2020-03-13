﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapGenerator mapGenerator;
    private EnemiesGenerator enemiesGenerator;

    private int level = 1;

    public GameObject player;
    
    
    void Start()
    {
        mapGenerator = GetComponent<MapGenerator>();
        enemiesGenerator = GetComponent<EnemiesGenerator>();

        InitLevel();
    }


    void InitLevel()
    {
        //Generamos el mapa del nivel
        mapGenerator.SetupMap();
        //Indicamos la posición inicial del jugador
        player.transform.position = InitPlayerPosition();
        //Generamos los enemigos del nivel
        enemiesGenerator.GenerateEnemies(level);
    }


    Vector3 InitPlayerPosition()
    {
        int randomIndex = Random.Range(0, (int)System.Math.Ceiling(mapGenerator.rooms[0].emptyPositions.Count / 2.5f));
        Vector3 randomPosition = mapGenerator.rooms[0].emptyPositions[randomIndex];

        mapGenerator.rooms[0].emptyPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    
    //Esta función permite comprobar si una posición concreta de una sala concreta está disponible
    public bool CheckPosition(Vector3 positionToCheck, int room)
    {
        if (mapGenerator.rooms[room].emptyPositions.Contains(positionToCheck))
        {
            return true;
        }

        return false;
    }

}
