using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public IntRange numRooms = new IntRange (6, 9);
    public IntRange roomWidth = new IntRange (11, 19);
    public IntRange roomHeight = new IntRange (10, 18); 
    public IntRange corridorLength = new IntRange (7, 9);
    public IntRange corridorWidth = new IntRange (5,7);

    public Room[] rooms; //tiene que ser pública, ya que deberá ser accesible desde el GameManager
    private Corridor[] corridors;

    [HideInInspector]
    public List<Tile> tiles = new List<Tile>();
    private List<Vector3Int> positions = new List<Vector3Int>();

    [SerializeField]
    private Tilemap groundMap, wallMap, obstacleMap, waterMap;
    [SerializeField]
    private TileBase groundTile, wallTile, smallObstacleTile, waterTile;


    public void SetupMap()
    {
        //Limpiamos los tilemaps
        wallMap.ClearAllTiles();
        groundMap.ClearAllTiles();
        obstacleMap.ClearAllTiles();
        waterMap.ClearAllTiles();

        CreateRoomsAndCorridors();
        InstantiateTiles();
    }
   
    void CreateRoomsAndCorridors()
    {
        positions.Clear();
        tiles.Clear();
        
        rooms = new Room[numRooms.Randomize]; //creamos el array de cuartos con un tamaño aleatorio
        corridors = new Corridor[rooms.Length - 1]; //creamos el array de pasillos, que será igual al número de cuartos - 1, ya que el primer cuarto no tiene pasillo
        Debug.Log("ROOMS LENGTH: " + rooms.Length);

        //Creamos el primer cuarto y el primer pasillo
        rooms[0] = new Room();
        corridors[0] = new Corridor();
        //Establecemos las características del primer cuarto, que no tiene pasillo
        rooms[0].SetupRoom(roomWidth.minVal, roomHeight.minVal);
        SetPositionsForRoom(0);
        //Establecemos las características del primer pasillo usando el primer cuarto
        int nDirection = Random.Range(0, 4);
        corridors[0].SetupCorridor(rooms[0], corridorLength, corridorWidth, roomWidth, roomHeight, nDirection, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room();

            int attempt = 0;
            do //cambiaremos las características de la sala actual hasta que esta sala que no se sitúe sobre una sala ya existente
            {
                if (attempt != 0 && attempt % 8 == 0)
                {
                    //Cambiamos la dirección del último corredor
                    nDirection = ((int)corridors[i-1].direction + 1) % 4;
                    corridors[i-1].SetupCorridor(rooms[i-1], corridorLength, corridorWidth, roomWidth, roomHeight, nDirection, false);
                    
                    if (attempt >= 32 && i >= 2) //nos damos por vencidos y paramos la generación de terreno
                    {
                        //Cambiamos el número de salas
                        System.Array.Resize(ref rooms, i);
                        System.Array.Resize(ref corridors, i - 1);
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
                SetPositionsForRoom(i);
        }
        SetTilesForRooms();
        SetTilesForCorridors();
        SetTilesForOuterWalls();
    }

    void SetPositionsForRoom(int i)
    {
        Room currentRoom = rooms[i];

        for (int j = 0; j < currentRoom.roomWidth; j++)
        {
            int xCoord = currentRoom.xPos + j;

            for (int k = 0; k < currentRoom.roomHeight; k++)
            {
                int yCoord = currentRoom.yPos + k;
                Vector3Int pos = new Vector3Int(xCoord, yCoord, 0);
                positions.Add(pos);
            }
        }
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
                
                //Si la posición está ocupada
                if (positions.Exists(x => x == pos)) 
                {
                    Debug.Log("Need to recalculate room");
                    return false;
                }
            }
        }
        return true;
    }

    void RemoveFromEmptyList(Room currentRoom, Vector3Int pos)
    {
        if (currentRoom.emptyPositions.Contains(pos))
        {
            currentRoom.emptyPositions.Remove(pos);
        }
    }

    void SetTilesForRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];

            //Calculamos el nº máximo de pequeños obstáculos que puede haber en la sala
            int maxSmallObstacles = (int)((currentRoom.roomHeight * currentRoom.roomWidth) / (currentRoom.roomHeight / 1.25f + currentRoom.roomWidth / 1.25f));
            int smallObstacles = 0;
            int water = 0;
            int maxWater = 2;

            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                TileType currentTileType;

                for (int k = 0; k < currentRoom.roomHeight; k++)
                {
                    int yCoord = currentRoom.yPos + k;
                    currentTileType = TileType.RoomFloor;
                    float random = Random.Range(0f, 1f);

                    if (j == 0 || j == currentRoom.roomWidth - 1 || k == 0 || k == currentRoom.roomHeight - 1)
                    {
                        currentTileType = TileType.Wall; //Añadimos paredes a la sala
                    }
                    else if ((j == 1 || j == currentRoom.roomWidth - 2) && k >= 2)
                    {
                        bool addVerticalWall = false;

                        if (random > 0.85f && i != 0 && i != rooms.Length - 1)
                        {
                            if ((j == 1 && k >= 2 && currentRoom.enteringCorridor != Direction.East && corridors[i].direction != Direction.West) ||
                                (j == currentRoom.roomHeight - 2 && k >= 2 && currentRoom.enteringCorridor != Direction.West && corridors[i].direction != Direction.East))
                            {
                                addVerticalWall = true;
                            }
                        }
                        else if (random > 0.85f && i != 0)
                        {
                            if ((j == 1 && k >= 2 && currentRoom.enteringCorridor != Direction.East) ||
                                (j == currentRoom.roomHeight - 2 && k >= 2 && currentRoom.enteringCorridor != Direction.West))
                            {
                                addVerticalWall = true;
                            }
                        }

                        if (addVerticalWall)
                        {
                            currentTileType = TileType.Wall;
                            Tile bottomTile = tiles.Find(x => x.pos.x == xCoord && x.pos.y == yCoord - 1);
                            tiles[tiles.IndexOf(bottomTile)].tileType = currentTileType;
                            tiles[tiles.IndexOf(bottomTile) - 1].tileType = currentTileType;
                            RemoveFromEmptyList(currentRoom, bottomTile.pos);
                            RemoveFromEmptyList(currentRoom, tiles[tiles.IndexOf(bottomTile) - 1].pos);
                        }
                    }
                    else if ((k == 1 || k == currentRoom.roomHeight - 2) && j >= 2)
                    {
                        bool addHorizontalWall = false;

                        if (random > 0.85f && i != 0 && i != rooms.Length - 1)
                        {
                            if ((k == 1 && currentRoom.enteringCorridor != Direction.North && corridors[i].direction != Direction.South) ||
                                (k == currentRoom.roomHeight - 2 && currentRoom.enteringCorridor != Direction.South && corridors[i].direction != Direction.North))
                            {
                                addHorizontalWall = true;
                            }
                        }
                        else if (random > 0.85f && i != 0)
                        {
                            if ((k == 1 && currentRoom.enteringCorridor != Direction.North) ||
                                (k == currentRoom.roomHeight - 2 && currentRoom.enteringCorridor != Direction.South))
                            {
                                addHorizontalWall = true;
                            }
                        }

                        if (addHorizontalWall)
                        {
                            currentTileType = TileType.Wall;
                            Tile leftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord);
                            tiles[tiles.IndexOf(leftTile)].tileType = currentTileType;
                            Tile leftLeftTile = tiles.Find(x => x.pos.x == xCoord - 2 && x.pos.y == yCoord);
                            tiles[tiles.IndexOf(leftLeftTile)].tileType = currentTileType;
                            RemoveFromEmptyList(currentRoom, leftTile.pos);
                            RemoveFromEmptyList(currentRoom, leftLeftTile.pos);
                        }
                    }
                    else if (j >= 2 && k >= 2 && j < currentRoom.roomWidth - 2 && k < currentRoom.roomHeight - 2)
                    {
                        //Añadimos piedras a la sala
                        Tile leftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord);
                        Tile leftLeftTile = tiles.Find(x => x.pos.x == xCoord - 2 && x.pos.y == yCoord);
                        Tile bottomTile = tiles.Find(x => x.pos.x == xCoord && x.pos.y == yCoord - 1);
                        Tile bottomBottomTile = tiles.Find(x => x.pos.x == xCoord && x.pos.y == yCoord - 2);
                        Tile bottomLeftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord - 1);
                        Tile topLeftTile = tiles.Find(x => x.pos.x == xCoord - 1 && x.pos.y == yCoord + 1);

                        if (random > 0.965f)
                        {
                            if (smallObstacles < maxSmallObstacles &&
                                leftTile.tileType != TileType.SmallObstacle &&
                                bottomTile.tileType != TileType.SmallObstacle &&
                                bottomLeftTile.tileType != TileType.SmallObstacle &&
                                topLeftTile.tileType != TileType.SmallObstacle)
                            {
                                currentTileType = TileType.SmallObstacle;
                                smallObstacles++;
                            }
                        }

                        //Añadimos agua a la sala
                        if (water < maxWater &&
                            leftTile.tileType == TileType.RoomFloor &&
                            bottomLeftTile.tileType != TileType.Water &&
                            topLeftTile.tileType != TileType.Water &&
                            bottomTile.tileType == TileType.RoomFloor &&
                            ((leftLeftTile != null &&
                            leftLeftTile.tileType != TileType.Water) ||
                            leftLeftTile == null) &&
                            ((bottomBottomTile != null &&
                            bottomBottomTile.tileType != TileType.Water) ||
                            bottomBottomTile == null))
                        {
                            if (random > 0.97f)
                            {
                                currentTileType = TileType.Water;
                                water++;

                                if (random > 0.97f && random < 0.99f)
                                {
                                    //Agua de 2 tiles
                                    tiles[tiles.IndexOf(leftTile)].tileType = currentTileType;
                                    water++;
                                    //Si la posición del leftTile pertenecía a la lista de emptyPositions, la eliminamos de esta
                                    RemoveFromEmptyList(currentRoom, leftTile.pos);
                                }
                                else if (random >= 0.99f)
                                {
                                    tiles[tiles.IndexOf(leftTile)].tileType = currentTileType;
                                    tiles[tiles.IndexOf(bottomLeftTile)].tileType = currentTileType;
                                    tiles[tiles.IndexOf(bottomTile)].tileType = currentTileType;
                                    water++;

                                    RemoveFromEmptyList(currentRoom, leftTile.pos);
                                    RemoveFromEmptyList(currentRoom, bottomLeftTile.pos);
                                    RemoveFromEmptyList(currentRoom, bottomTile.pos);
                                }
                            }
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

    void SetTilesForOuterWalls()
    {
        TileType currentTileType = TileType.OuterWall;

        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            
            for (int j = -8; j < currentRoom.roomWidth + 8; j++)
            {
                int xCoord = currentRoom.xPos + j;

                for (int k = -7; k < currentRoom.roomHeight + 7; k++)
                {
                    //si se trata del interior de la sala, no lo revisamos
                    if (j >= 0 && j < currentRoom.roomWidth && k >= 0 && k < currentRoom.roomHeight) 
                        continue;

                    int yCoord = currentRoom.yPos + k;
                    Vector3Int pos = new Vector3Int(xCoord, yCoord, 0);

                    if (!tiles.Exists(x => x.pos == pos)) //Si la posición no está ocupada
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

                case TileType.OuterWall:
                    wallMap.SetTile(pos, wallTile);
                    break;

                case TileType.SmallObstacle:
                    obstacleMap.SetTile(pos, smallObstacleTile);
                    break;

                case TileType.Water:
                    waterMap.SetTile(pos, waterTile);
                    break;
            }
        }
    }

    //Esta función permite comprobar si una posición concreta de una sala concreta está disponible
    public bool CheckPosition(Vector3 positionToCheck, int room)
    {
        if (rooms[room].emptyPositions.Contains(positionToCheck))
        {
            return true;
        }
        return false;
    }
}
