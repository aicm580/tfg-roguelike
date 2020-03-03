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

    public int randomX;

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

    public void SetupCorridor (Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, bool firstCorridor)
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
        
        Debug.Log(direction);

        corridorLength = length.Randomize; //establecemos una longitud del pasillo aleatoria

        switch (direction)
        {
            case Direction.North: 
                randomX = Random.Range(2, room.roomWidth - 2);
                startXPos = room.xPos + randomX;
                startYPos = room.yPos[randomX] + room.columnHeight[randomX] - 1;
                break;
            case Direction.South: 
                randomX = Random.Range(2, room.roomWidth - 2);
                startXPos = room.xPos + randomX;
                startYPos = room.yPos[randomX];
                break;            
            case Direction.East: 
                startXPos = room.xPos + room.roomWidth - 1;
                startYPos = Random.Range(room.yPos[room.roomWidth-1] + 2, room.yPos[room.roomWidth-1] + room.columnHeight[room.roomWidth-1] - 2);
                break;
            case Direction.West: 
                startXPos = room.xPos;
                startYPos = Random.Range(room.yPos[0] + 2, room.yPos[0] + room.columnHeight[0] - 2);
                break;
        }
    }
}

