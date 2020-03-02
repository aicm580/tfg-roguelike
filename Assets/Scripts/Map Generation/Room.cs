using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos; //Coordenada X del tile inferior izquierdo de la sala
    public int yPos; //Coordenada Y del tile inferior izquierdo de la sala

    public int roomWidth;
    private int roomHeight;

    public int[] roomGrid;

    //Función usada para la creación del primer cuarto. No tiene el parámetro del pasillo, porque la primera sala no tiene pasillo que lleve hasta ella. 
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        roomWidth = widthRange.Randomize; //establecemos una anchura random para la sala
        roomHeight = heightRange.Randomize;

        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);
        yPos = Mathf.RoundToInt(rows / 2f - roomHeight / 2f); //establecemos la posición yPos en función de la primera altura calculada

        roomGrid = new int[roomWidth];
        
        for (int i = 0; i < roomGrid.Length; i++)
        {
            roomGrid[i] = roomHeight + Random.Range(-1, 4);
        }
    }
}
