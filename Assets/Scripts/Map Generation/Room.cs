using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos; //Coordenada X del tile inferior izquierdo de la sala

    public int roomWidth;
    private int roomHeight;

    public int[] columnHeight;
    public int[] yPos;

    //Función usada para la creación del primer cuarto. No tiene el parámetro del pasillo, porque la primera sala no tiene pasillo que lleve hasta ella. 
    public void SetupRoom (IntRange widthRange, IntRange heightRange, int columns, int rows)
    {
        roomWidth = widthRange.Randomize; //establecemos una anchura random para la sala
        roomHeight = heightRange.Randomize;

        columnHeight = new int[roomWidth];
        yPos = new int[roomWidth];

        xPos = Mathf.RoundToInt(columns / 2f - roomWidth / 2f);

        for (int i = 0; i < roomWidth - 3; i = i + 4)
        {
            yPos[i] = Mathf.RoundToInt(rows / 2f - roomHeight / 2f) + Random.Range(-1, 3);
            yPos[i+1] = yPos[i];
            yPos[i+2] = yPos[i];
            yPos[i+3] = yPos[i];
            
            columnHeight[i] = roomHeight + Random.Range(-2, 4);
            columnHeight[i+1] = columnHeight[i];
            columnHeight[i+2] = columnHeight[i];
            columnHeight[i+3] = columnHeight[i];
        }
    }
}
