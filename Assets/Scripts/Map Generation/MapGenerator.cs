using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public IntRange numRooms = new IntRange (6, 9);
    public IntRange roomWidth = new IntRange (10, 26);
    public IntRange roomHeight = new IntRange (10, 23); 
    public IntRange corridorLength = new IntRange (7, 13);
    public IntRange corridorWidth = new IntRange (5,7);

    public Room[] rooms; //tiene que ser pública, ya que deberá ser accesible desde el GameManager
    private Corridor[] corridors;

    private List <Tile> tiles = new List<Tile>();

    [SerializeField]
    private Tilemap groundMap;
    [SerializeField]
    private TileBase groundTile;
    [SerializeField]
    private Tilemap wallMap;
    [SerializeField]
    private TileBase wallTile;
    [SerializeField]
    private Tilemap obstacleMap;
    [SerializeField]
    private TileBase smallObstacleTile;
    [SerializeField]
    private TileBase waterTile;


    public void SetupMap()
    {
        //Limpiamos los tilemaps
        wallMap.ClearAllTiles();
        groundMap.ClearAllTiles();

        CreateRoomsAndCorridors();
        InstantiateTiles();
    }


    void CreateRoomsAndCorridors()
    {
        tiles.Clear();

        rooms = new Room[numRooms.Randomize]; //creamos el array de cuartos con un tamaño aleatorio
        corridors = new Corridor[rooms.Length - 1]; //creamos el array de pasillos, que será igual al número de cuartos - 1, ya que el primer cuarto no tiene pasillo
        Debug.Log("ROOMS LENGTH: " + rooms.Length);

        //Creamos el primer cuarto y el primer pasillo
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        //Establecemos las características del primer cuarto, que no tiene pasillo
        rooms[0].SetupRoom(roomWidth.minVal, roomHeight.minVal);
        Debug.Log("Room width min val: " + roomWidth.minVal);

        //Establecemos las características del primer pasillo usando el primer cuarto
        int nDirection = Random.Range(0, 4);
        corridors[0].SetupCorridor(rooms[0], corridorLength, corridorWidth, roomWidth, roomHeight, nDirection, true);

        SetTilesForRoom(0);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room();

            int attempt = 0;
            do //cambiaremos las características de la sala actual hasta que esta sala que no se sitúe sobre una sala ya existente
            {
                if (attempt != 0 && attempt % 8 == 0)
                {
                    nDirection = ((int)corridors[i-1].direction + 1) % 4;
                    Debug.Log("NEED TO CHANGE LAST CORRIDOR DIRECTION (" + corridors[i-1].direction + ")");
                    corridors[i-1].SetupCorridor(rooms[i-1], corridorLength, corridorWidth, roomWidth, roomHeight, nDirection, false);
                    
                    if (attempt >= 32 && i >= 2) //nos damos por vencidos y paramos la generación de terreno
                    {
                        System.Array.Resize(ref rooms, i);
                        System.Array.Resize(ref corridors, i - 1);
                        
                        Debug.Log("NUMBER OF ROOMS HAS CHANGED TO " + rooms.Length);
                        break;
                    }
                }

                if (i < rooms.Length)
                {
                    rooms[i].SetupRoom(roomWidth, roomHeight, corridors[i-1]);
                    attempt += 1;
                }

            } while (!PositionAvailable(rooms[i])); 
            
            if (i < corridors.Length)
            {
                nDirection = Random.Range(0, 4);
                corridors[i] = new Corridor();
                corridors[i].SetupCorridor(rooms[i], corridorLength, corridorWidth, roomWidth, roomHeight, nDirection, false);
            }
            
            if (i < rooms.Length)
                SetTilesForRoom(i);
        }

        SetTilesForCorridors();
    }


    bool PositionAvailable(Room currentRoom)
    {
        for (int j = 0; j < currentRoom.roomWidth; j++)
        {
            int xCoord = currentRoom.xPos + j;

            for (int k = 0; k < currentRoom.roomHeight; k++)
            {
                int yCoord = currentRoom.yPos + k;
                Vector3Int pos = new Vector3Int(xCoord, yCoord, 0);

                if(tiles.Exists(x => x.pos == pos)) //Si la posición está ocupada
                {
                    Debug.Log("Need to recalculate room");
                    return false;
                }
            }
        }
        
        Debug.Log("Position available");
        return true;
    }


    void SetTilesForRoom(int i)
    {
        Room currentRoom = rooms[i];

        //Calculamos el nº máximo de pequeños obstáculos que puede haber en la sala
        int maxSmallObstacles = (int)((currentRoom.roomHeight * currentRoom.roomWidth) / (currentRoom.roomHeight / 1.25f + currentRoom.roomWidth / 1.25f));
        int smallObstacles = 0;
        Debug.Log("ROOM " + i + " MaxSmallObstacles: " + maxSmallObstacles);

        for (int j = 0; j < currentRoom.roomWidth; j++)
        {
            int xCoord = currentRoom.xPos + j;

            TileType currentTileType; 

            for (int k = 0; k < currentRoom.roomHeight; k++)
            {
                int yCoord = currentRoom.yPos + k;
                currentTileType = TileType.RoomFloor;

                if (j == 0 || j == currentRoom.roomWidth - 1 || k == 0 || k == currentRoom.roomHeight - 1)
                {
                    //Añadimos paredes a la sala
                    currentTileType = TileType.Wall;
                }
                else
                {
                    //Añadimos piedras a la sala
                    Tile leftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord);
                    Tile bottomTile = tiles.Find(x => x.pos.x == xCoord && x.pos.y == yCoord - 1);
                    Tile bottomLeftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord - 1);
                    Tile topLeftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord + 1);

                    float random = Random.Range(0f, 10f);
                    if (random > 9.65f)
                    {
                        if (bottomTile != null && bottomTile.tileType == TileType.RoomFloor && smallObstacles < maxSmallObstacles)
                        {
                            if ((leftTile != null && leftTile.tileType == TileType.RoomFloor &&
                                bottomLeftTile != null && bottomLeftTile.tileType == TileType.RoomFloor &&
                                topLeftTile != null && topLeftTile.tileType == TileType.RoomFloor)
                                ||
                                (leftTile == null && bottomTile != null)
                                ||
                                (leftTile != null && leftTile.tileType == TileType.RoomFloor && topLeftTile == null)
                            )
                            {
                                currentTileType = TileType.SmallObstacle;
                                smallObstacles++;
                            }
                        }
                    }

                    //Añadimos agua a la sala
                    random = Random.Range(0f, 10f);
                    if (random > 9.3f && random < 9.65f)
                    {
                        //Agua de 2 tiles
                        if (leftTile != null && leftTile.tileType == TileType.RoomFloor)
                        {
                            currentTileType = TileType.Water;
                        }
                        else if (leftTile != null && leftTile.tileType == TileType.Water)
                        {
                            currentTileType = TileType.Water;
                        }
                    }
                    else if (random >= 9.65f)
                    {
                        //Agua de 4 tiles
                    }
                }

                Vector3Int pos = new Vector3Int(xCoord, yCoord, 0);
                Tile newTile = new Tile(currentTileType, pos);
                tiles.Add(newTile);

                //Si se trata de un tile Floor, añadimos su posición a la lista de posiciones vacías de la sala
                if (currentTileType == TileType.RoomFloor)
                {
                    currentRoom.emptyPositions.Add(pos);
                }
            }
        }
    }


    void SetTilesForCorridors()
    {
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];
            
            for (int j = 0; j < currentCorridor.corridorWidth; j++)
            {
                for (int k = 0; k <= currentCorridor.corridorLength; k++)
                {
                    int xCoord = currentCorridor.startXPos;
                    int yCoord = currentCorridor.startYPos;

                    TileType currentTileType = TileType.RoomFloor; 

                    if (currentCorridor.direction == Direction.North || currentCorridor.direction == Direction.South)
                    {
                        if (currentCorridor.direction == Direction.North)
                        {
                            xCoord = currentCorridor.startXPos + j;
                            yCoord += k;
                        }
                        else
                        {
                            xCoord = currentCorridor.startXPos + j;
                            yCoord -= k; 
                        }

                        if (xCoord == currentCorridor.startXPos ||
                            xCoord == currentCorridor.startXPos + currentCorridor.corridorWidth - 1
                            ) 
                        {
                            currentTileType = TileType.Wall;
                        }

                    }
                    else if (currentCorridor.direction == Direction.East || currentCorridor.direction == Direction.West)
                    {
                        if (currentCorridor.direction == Direction.East)
                        {
                            xCoord += k;
                            yCoord = currentCorridor.startYPos + j;
                        }
                        else
                        {
                            xCoord -= k;
                            yCoord = currentCorridor.startYPos + j;
                        }

                        if (yCoord == currentCorridor.startYPos || 
                            yCoord == currentCorridor.startYPos + currentCorridor.corridorWidth - 1
                            )
                        {
                            currentTileType = TileType.Wall;
                        }
                
                    }

                    Vector3Int pos = new Vector3Int(xCoord, yCoord, 0);

                    Tile t = tiles.Find(x => x.pos == pos); //miramos si ya hay un tile guardado con esta posición 
                    if (t != null) //si se ha guardado ya un tile con esta posición
                    {
                        tiles[tiles.IndexOf(t)].tileType = currentTileType;
                    }
                    else
                    {
                        Tile newTile = new Tile(currentTileType, pos);
                        tiles.Add(newTile);
                    }
                 
                }
            }
        }
    }


    void InstantiateTiles()
    {
        foreach (Tile tile in tiles)
        {
            Vector3Int pos = new Vector3Int((int)tile.pos.x, (int)tile.pos.y, 0);
            Vector3Int currentCell = groundMap.WorldToCell(transform.position);
            
            switch (tile.tileType)
            {
                case TileType.RoomFloor:
                    groundMap.SetTile(pos, groundTile);
                    break;

                case TileType.Wall:
                    wallMap.SetTile(pos, wallTile);
                    break;

                case TileType.SmallObstacle:
                    obstacleMap.SetTile(pos, smallObstacleTile);
                    break;

                case TileType.Water:
                    obstacleMap.SetTile(pos, waterTile);
                    break;
            }
        }
    }
}
