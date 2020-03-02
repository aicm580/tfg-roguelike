using UnityEngine;


public enum Direction
{
    North, East, South, West,
}


public class Corridor
{
    public int startXPos;
    public int startYPos;

    public int corridorLength;

    public Direction direction;

    //Calculamos la posición X final del pasillo según su posición X inicial y su dirección
    public int EndPositionX
    {
        get
        {
            if (direction == Direction.North || direction == Direction.South)
                return startXPos;

            if (direction == Direction.East) //derecha
                return startXPos + corridorLength;

            return startXPos - corridorLength; //izquierda
        }
    }

    //Calculamos la posición Y final del pasillo según su posición Y inicial y su dirección
    public int EndPositionY
    {
        get
        {
            if (direction == Direction.East || direction == Direction.West)
                return startYPos;

            if (direction == Direction.North)
                return startYPos + corridorLength; 

            return startYPos - corridorLength; //si la dirección es South
        }
    }

    public void SetupCorridor (Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
    {
        //la dirección se decidirá de forma aleatoria
        direction = (Direction)Random.Range(0, 4);

        Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4); //por ejemplo, si el pasillo que lleva a la habitación actual tiene dirección 2 (South), su contrario tendrá dirección 0 (North)

        if (!firstCorridor && direction == oppositeDirection)
        {
            int directionInt = (int)direction;
            directionInt++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        corridorLength = length.Randomize; //establecemos una longitud del pasillo aleatoria

        int maxLength = length.maxVal; //la inicializamos con la longitud máxima establecida para los pasillos, pero modificaremos este valor en función de la dirección del pasillo, para evitar salir del tablero
        int random;

        switch (direction)
        {
            case Direction.North: 
                random = Random.Range(2, room.roomWidth - 2);
                startXPos = room.xPos + random;
                startYPos = room.yPos[random] + room.columnHeight[random];
                maxLength = rows - startYPos - roomHeight.minVal;
                break;
            case Direction.South: 
                random = Random.Range(2, room.roomWidth - 2);
                startXPos = room.xPos + random;
                startYPos = room.yPos[random];
                maxLength = startYPos - roomHeight.minVal;
                break;            
            case Direction.East: 
                
                break;
            case Direction.West: 
               
                break;
        }

        corridorLength = Mathf.Clamp(corridorLength, 1, maxLength);
        Debug.Log(startXPos);
    }
}

