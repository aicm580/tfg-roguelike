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
    public int corridorWidth;

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

    public void SetupCorridor (Room room, IntRange length, IntRange width, IntRange roomWidth, IntRange roomHeight, int nDirection, bool firstCorridor)
    {
        //la dirección se decidirá de forma aleatoria
        direction = (Direction)nDirection;

        Direction oppositeDirection = (Direction)(((int)room.enteringCorridor + 2) % 4); //por ejemplo, si el pasillo que lleva a la habitación actual tiene dirección 2 (South), su contrario tendrá dirección 0 (North)

        if (!firstCorridor && direction == oppositeDirection)
        {
            int directionInt = (int)direction;
            directionInt++;
            directionInt = directionInt % 4;
            direction = (Direction)directionInt;
        }

        //Debug.Log(direction);

        corridorLength = length.Randomize; //establecemos una longitud del pasillo aleatoria
        corridorWidth = width.Randomize; //establecemos una anchura del pasillo aleatoria

        switch (direction)
        {
            case Direction.North: 
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - corridorWidth);
                startYPos = room.yPos + room.roomHeight - 1;
                break;
            case Direction.South:
                startXPos = Random.Range(room.xPos, room.xPos + room.roomWidth - corridorWidth);
                startYPos = room.yPos;
                break;            
            case Direction.East: 
                startXPos = room.xPos + room.roomWidth - 1;
                startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight - corridorWidth - 3);
                break;
            case Direction.West: 
                startXPos = room.xPos;
                startYPos = Random.Range(room.yPos + corridorWidth, room.yPos + room.roomHeight - corridorWidth - 3);
                break;
        }
    }
}

