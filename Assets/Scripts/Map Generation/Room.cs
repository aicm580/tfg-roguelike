using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int xPos; //Coordenada X del tile inferior izquierdo de la sala
    public int yPos; //Coordenada Y del tile inferior izquierdo de la sala

    public int roomWidth;
    public int roomHeight;

    public Direction enteringCorridor; //dirección del pasillo que lleva a esta sala

    public List <Vector3> emptyPositions = new List<Vector3>(); //posiciones en las que se podrán colocar enemigos o elementos

    public int nEnemies = 1; //nº de enemigos que podrán generarse en la sala. Se inicializa a 1 para evitar problemas
    


    //Función usada para la creación del primer cuarto. No tiene el parámetro del pasillo, porque la primera sala no tiene pasillo que lleve hasta ella. 
    public void SetupRoom (int width, int height)
    {
        //Nos aseguramos de que la lista de posiciones no contiene ningún valor
        emptyPositions.Clear();

        //Establecemos una altura y una anchura
        roomWidth = width;
        roomHeight = height;

        xPos = Random.Range(2, 30);
        yPos = Random.Range(roomHeight, roomHeight + 15);
    }

    //Hacemos overload a la función SetupRoom, añadiendo el parámetro del pasillo
    public void SetupRoom (IntRange widthRange, IntRange heightRange, Corridor corridor)
    {
        //Nos aseguramos de que la lista de posiciones no contiene ningún valor
        emptyPositions.Clear();

        //Establecemos una altura y una anchura aleatorias
        roomWidth = widthRange.Randomize;
        roomHeight = heightRange.Randomize;

        enteringCorridor = corridor.direction;


        //DIRECCIÓN ESTE - xPos
        if (corridor.direction == Direction.East)
        {
            xPos = corridor.EndPositionX;
        }
        //DIRECCIÓN OESTE - xPos
        else if (corridor.direction == Direction.West)
        {
            xPos = corridor.EndPositionX - roomWidth + 1;
        }
        //DIRECCIÓN NORTE O SUR - xPos
        else if (corridor.direction == Direction.North || corridor.direction == Direction.South)
        {
            xPos = Random.Range(corridor.EndPositionX + corridor.corridorWidth - roomWidth, corridor.EndPositionX);
        }


        //DIRECCIÓN ESTE O OESTE - yPos
        if (corridor.direction == Direction.East || corridor.direction == Direction.West)
        {
            yPos = Random.Range(corridor.EndPositionY + corridor.corridorWidth - roomHeight, corridor.EndPositionY);
        }
        //DIRECCIÓN NORTE - yPos
        else if (corridor.direction == Direction.North)
        {
            yPos = corridor.EndPositionY;
        }
        //DIRECCIÓN SUR - yPos
        else if (corridor.direction == Direction.South)
        {
            yPos = corridor.EndPositionY - roomHeight + 1;
        }
    }
}
