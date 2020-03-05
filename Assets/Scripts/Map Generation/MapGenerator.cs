using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public IntRange numRooms = new IntRange (5, 8);
    public IntRange roomWidth = new IntRange (10, 30);
    public IntRange roomHeight = new IntRange (8, 30); 
    public IntRange corridorLength = new IntRange (7, 15);
    public IntRange corridorWidth = new IntRange (3,5);

    private Room[] rooms;
    private Corridor[] corridors;

    private List <Tile> tiles = new List<Tile>();
    private List <Vector2> mapPositions = new List<Vector2>();

    private GameObject mapHolder;

    public GameObject[] floorTiles;
    public GameObject[] corridorTiles;
    


    public void SetupMap(int level)
    {
        mapHolder = new GameObject("MapHolder");

        CreateRoomsAndCorridors();
        InstantiateTiles();

     //   InitialiseList();
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
        rooms[0].SetupRoom(roomWidth, roomHeight);
        SetTilesForRoom(rooms[0]);

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
                SetTilesForRoom(rooms[i]);
        }

        SetTilesForCorridors();
    }


    bool PositionAvailable(Room currentRoom)
    {
        for (int j = 0; j < currentRoom.roomWidth; j++)
        {
            int xCoord = currentRoom.xPos + j;

            for (int k = 0; k < currentRoom.columnHeight[j]; k++)
            {
                int yCoord = currentRoom.yPos[j] + k;
                Vector2 pos = new Vector2(xCoord, yCoord);

                if(tiles.Exists(x => x.pos == pos)) //Si la posición no está ocupada
                {
                    Debug.Log("Need to recalculate room");
                    return false;
                }
            }
        }
        
        Debug.Log("Position available");
        return true;
    }


    void SetTilesForRoom(Room currentRoom)
    {        
        for (int j = 0; j < currentRoom.roomWidth; j++)
        {
            int xCoord = currentRoom.xPos + j;

            for (int k = 0; k < currentRoom.columnHeight[j]; k++)
            {
                int yCoord = currentRoom.yPos[j] + k;
                Vector2 pos = new Vector2(xCoord, yCoord);
                Tile newTile = new Tile(TileType.Floor, pos);
                tiles.Add(newTile);
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
                for (int k = 0; k < currentCorridor.corridorLength; k++)
                {
                    int xCoord = currentCorridor.startXPos;
                    int yCoord = currentCorridor.startYPos;

                    switch (currentCorridor.direction)
                    {
                        case Direction.North: 
                            xCoord = currentCorridor.startXPos + j;
                            yCoord += k;
                            break;
                        case Direction.South:
                            xCoord = currentCorridor.startXPos + j;
                            yCoord -= k; 
                            break;
                        case Direction.East:
                            xCoord += k;
                            yCoord = currentCorridor.startYPos + j;
                            break;
                        case Direction.West: 
                            xCoord -= k;
                            yCoord = currentCorridor.startYPos + j;
                            break;
                    }

                    Vector2 pos = new Vector2(xCoord, yCoord);
                    Tile newTile = new Tile(TileType.Corridor, pos);
                    tiles.Add(newTile);
                }

            }
        }
    }


    void InstantiateTiles()
    {
        foreach (Tile tile in tiles)
        {
            switch (tile.tileType)
            {
                case TileType.Floor:
                    InstantiateFromArray(floorTiles, tile.pos.x, tile.pos.y);
                    break;

                case TileType.Corridor: 
                    InstantiateFromArray(corridorTiles, tile.pos.x, tile.pos.y);
                    break;
            }
   
        }
    }


    void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
    {
        int randomIndex = Random.Range(0, prefabs.Length);

        Vector3 position = new Vector3(xCoord, yCoord, 0f);

        GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

        tileInstance.transform.parent = mapHolder.transform;
    }
/*

    void InitialiseList() //CAMBIAR-HO PER SES POSICIONS QUE SIGUIN IGUAL A FLOOR
    {
        mapPositions.Clear();

        for (int x = 0; x < columns - 1; x++)
        {
            for (int y = 0; y < rows - 1; y++)
            {
                mapPositions.Add(new Vector3(x, y, 0f)); 
            }
        }
    }
    */
}
