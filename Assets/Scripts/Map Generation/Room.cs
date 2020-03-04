using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos; //Coordenada X del tile inferior izquierdo de la sala

    public int[] yPos;
    public int[] columnHeight;

    public int roomWidth;
    private int roomHeight;

    public Direction enteringCorridor; //dirección del pasillo que lleva a esta sala


    //Función usada para la creación del primer cuarto. No tiene el parámetro del pasillo, porque la primera sala no tiene pasillo que lleve hasta ella. 
    public void SetupRoom (IntRange widthRange, IntRange heightRange)
    {
        //Establecemos una altura y una anchura aleatorias
        roomWidth = widthRange.Randomize;
        roomHeight = heightRange.Randomize;

        columnHeight = new int[roomWidth];
        yPos = new int[roomWidth];

        xPos = Random.Range(2, 30);

        for (int i = 0; i < roomWidth - 3; i = i + 4)
        {
            yPos[i] = Mathf.RoundToInt(100 / 2f - roomHeight / 2f) + Random.Range(-1, 3);
            yPos[i+1] = yPos[i];
            yPos[i+2] = yPos[i];
            yPos[i+3] = yPos[i];
            
            columnHeight[i] = roomHeight + Random.Range(-2, 4);
            columnHeight[i+1] = columnHeight[i];
            columnHeight[i+2] = columnHeight[i];
            columnHeight[i+3] = columnHeight[i];
        }
    }

    //Hacemos overload a la función SetupRoom, añadiendo el parámetro del pasillo
    public void SetupRoom (IntRange widthRange, IntRange heightRange, Corridor corridor)
    {
        //Establecemos una altura y una anchura aleatorias
        roomWidth = widthRange.Randomize;
        roomHeight = heightRange.Randomize;

        columnHeight = new int[roomWidth];
        yPos = new int[roomWidth];

        enteringCorridor = corridor.direction;


        if (corridor.direction == Direction.East || corridor.direction == Direction.West)
        {

            if (corridor.direction == Direction.East)
            {
                xPos = corridor.EndPositionX - 1;
            }
            else
            {
                xPos = corridor.EndPositionX - roomWidth;
            }
        }
        else if (corridor.direction == Direction.North || corridor.direction == Direction.South)
        {
            xPos = Random.Range(corridor.EndPositionX - roomWidth + 2, corridor.EndPositionX - 2);
        }


        for (int i = 0; i < roomWidth; i++)
        {
            if (corridor.direction == Direction.East || corridor.direction == Direction.West)
            {
                yPos[i] = Random.Range(corridor.EndPositionY - roomHeight, corridor.EndPositionY);
            }
            else if (corridor.direction == Direction.North)
            {
                if(i == corridor.EndPositionX - xPos)
                {
                    yPos[i] = corridor.EndPositionY;
                }
                else if (i % 4 == 0) //si i es igual a 0, 4, 8, 12,...
                {
                    yPos[i] = corridor.EndPositionY +  Random.Range(-1, 3);
                }
                else{
                    yPos[i] = yPos[i-1];
                }
            }
            else
            {
                yPos[i] = corridor.EndPositionY - roomHeight + 1;
               // yPos[i+1] = yPos[i] + Random.Range(-1, 3);
            }

            
           
            columnHeight[i] = roomHeight + Random.Range(-2, 4);
             /*
            columnHeight[i+1] = columnHeight[i];
            columnHeight[i+2] = columnHeight[i];
            columnHeight[i+3] = columnHeight[i];
            */
        }
    }
}
