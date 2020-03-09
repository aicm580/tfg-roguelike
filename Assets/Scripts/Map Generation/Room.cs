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

    public List <Vector2> emptyPositions = new List<Vector2>();


    //Función usada para la creación del primer cuarto. No tiene el parámetro del pasillo, porque la primera sala no tiene pasillo que lleve hasta ella. 
    public void SetupRoom (IntRange widthRange, IntRange heightRange)
    {
        //Nos aseguramos de que la lista de posiciones no contiene ningún valor
        emptyPositions.Clear();

        //Establecemos una altura y una anchura aleatorias
        roomWidth = widthRange.Randomize;
        roomHeight = heightRange.Randomize;

        columnHeight = new int[roomWidth];
        yPos = new int[roomWidth];

        xPos = Random.Range(2, 30);

        for (int i = 0; i < roomWidth; i++)
        {
            if (i % 4 == 0 && i+1 < roomWidth) //si la i es igual a 0, 4, 8, 12,... y no se trata de la última columna
            {
                yPos[i] = Mathf.RoundToInt(100 / 2f - roomHeight / 2f) + Random.Range(-1, 3);
                columnHeight[i] = roomHeight + Random.Range(-1, 4);
            }
            else
            {
                yPos[i] = yPos[i-1];
                columnHeight[i] = columnHeight[i-1];
            }
        }
    }

    //Hacemos overload a la función SetupRoom, añadiendo el parámetro del pasillo
    public void SetupRoom (IntRange widthRange, IntRange heightRange, Corridor corridor)
    {
        //Nos aseguramos de que la lista de posiciones no contiene ningún valor
        emptyPositions.Clear();
        
        //Establecemos una altura y una anchura aleatorias
        roomWidth = widthRange.Randomize;
        roomHeight = heightRange.Randomize;

        columnHeight = new int[roomWidth];
        yPos = new int[roomWidth];

        enteringCorridor = corridor.direction;

        //Calculamos la xPos en función de la dirección del pasillo
        if (corridor.direction == Direction.East || corridor.direction == Direction.West)
        {
            if (corridor.direction == Direction.East)
            {
                xPos = corridor.EndPositionX - 1;
            }
            else
            {
                xPos = corridor.EndPositionX - roomWidth + 2;
            }
        }
        else if (corridor.direction == Direction.North || corridor.direction == Direction.South)
        {
            xPos = Random.Range(corridor.EndPositionX - roomWidth + 2, corridor.EndPositionX - 2);
        }

        //Calculamos los array yPos y columnHeight en función de la dirección del pasillo
        for (int i = 0; i < roomWidth; i++)
        {
            if (i % 4 == 0 && i+1 < roomWidth)
            {
                columnHeight[i] = roomHeight + Random.Range(-1, 4);
            }
            else
            {
                columnHeight[i] = columnHeight[i-1];
            }

            //DIRECCIÓN ESTE u OESTE
            if (corridor.direction == Direction.East || corridor.direction == Direction.West)
            {
                if(i == 0)
                {
                    yPos[i] = Random.Range(corridor.EndPositionY - roomHeight + 2, corridor.EndPositionY - 2);
                }
                else if(i == roomWidth - 1 && corridor.direction == Direction.West)
                {
                    yPos[i] = yPos[0];
                    yPos[i-1] = yPos[0];
                }
                else if(i % 4 == 0 && i+1 < roomWidth)
                {
                    yPos[i] = yPos[0] + Random.Range(-1, 3);
                }
                else{
                    yPos[i] = yPos[i-1];
                }
                
            }
            //DIRECCIÓN NORTE o SUR
            else if (corridor.direction == Direction.North || corridor.direction == Direction.South)
            {
                if(i == corridor.EndPositionX - xPos) //si se trata de la columna que coincide con el pasillo
                {
                    if(corridor.direction == Direction.North)
                    {
                        yPos[i] = corridor.EndPositionY - 1;
                    }
                    else
                    {
                        yPos[i] = corridor.EndPositionY - roomHeight + 2;
                        columnHeight[i] = roomHeight;
                    }  
                }
                else if (i % 4 == 0 && i+1 < roomWidth) //si i es igual a 0, 4, 8, 12,... y no se trata de la última columna
                {
                    if(corridor.direction == Direction.North)
                    {
                        yPos[i] = corridor.EndPositionY +  Random.Range(-1, 3);
                    }
                    else
                    {
                        yPos[i] = corridor.EndPositionY - roomHeight + 1 + Random.Range(-1, 3);
                    }
                }
                else{
                    yPos[i] = yPos[i-1];
                }
            }
        }
    }
}
