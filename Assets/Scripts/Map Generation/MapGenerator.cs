using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum TileType
    {
       Wall, Floor, Corridor, 
    } 

    public IntRange numRooms = new IntRange (4, 8);
    public IntRange roomWidth = new IntRange (10, 30);
    public IntRange roomHeight = new IntRange (10, 30); 
    public IntRange corridorLength = new IntRange (4, 15);

    private int columns = 150;
    private int rows = 150;

    public GameObject[] floorTiles;
    public GameObject[] corridorTiles;

    private GameObject mapHolder;

    private TileType[][] tiles;
    private Room[] rooms;
    private Corridor[] corridors;
    
    private List <Vector3> mapPositions = new List<Vector3>();
   

    public void SetupMap(int level)
    {
        mapHolder = new GameObject("MapHolder");
        
        SetupTilesArray();

        CreateRoomsAndCorridors();
        SetTilesValuesForRooms();
        SetTilesValuesForCorridors();
        InstantiateTiles();

     //   InitialiseList();
    }
    

    void SetupTilesArray()
    {
        tiles = new TileType[columns][];

        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new TileType[rows];
        }
    }


    void CreateRoomsAndCorridors()
    {
     //   rooms = new Room[numRooms.Randomize]; //creamos el array de cuartos con un tamaño aleatorio
        rooms = new Room[2];
        corridors = new Corridor[rooms.Length - 1]; //creamos el array de pasillos, que será igual al número de cuartos - 1, ya que el pimer cuarto no tiene pasillo

        //Creamos el primer cuarto y el primer pasillo
        rooms[0] = new Room();
        corridors[0] = new Corridor();

        //Establecemos las características del primer cuarto, que no tiene pasillo
        rooms[0].SetupRoom(roomWidth, roomHeight);
        //Establecemos las características del primer pasillo usando el primer cuarto
        corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, true);

        for (int i = 1; i < rooms.Length; i++)
        {
            rooms[i] = new Room();
            rooms[i].SetupRoom(roomWidth, roomHeight, corridors[i-1]);
        }
    }


    void SetTilesValuesForRooms()
    {
        for (int i = 0; i < rooms.Length; i++)
        {
            Room currentRoom = rooms[i];
            Debug.Log("Room " + i + ": Width " + currentRoom.roomWidth);
           
            for (int j = 0; j < currentRoom.roomWidth; j++)
            {
                int xCoord = currentRoom.xPos + j;

                for (int k = 0; k < currentRoom.columnHeight[j]; k++)
                {
                    int yCoord = currentRoom.yPos[j] + k;
                    tiles[xCoord][yCoord] = TileType.Floor;
                }
            }
        }
    }


    void SetTilesValuesForCorridors()
    {
        for (int i = 0; i < corridors.Length; i++)
        {
            Corridor currentCorridor = corridors[i];

            for (int j = 0; j < currentCorridor.corridorLength; j++)
            {
                int xCoord = currentCorridor.startXPos;
                int yCoord = currentCorridor.startYPos;

                switch (currentCorridor.direction)
                {
                    case Direction.North: 
                        yCoord += j;
                        break;
                    case Direction.South:
                        yCoord -= j; 
                        break;
                    case Direction.East:
                        xCoord += j;
                        break;
                    case Direction.West: 
                        xCoord -= j;
                        break;
                }

                tiles[xCoord][yCoord] = TileType.Corridor;
            }

        }
    }


    void InstantiateTiles()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                switch (tiles[i][j])
                {
                    case TileType.Floor:
                        InstantiateFromArray(floorTiles, i, j);
                        break;

                    case TileType.Corridor: 
                        InstantiateFromArray(corridorTiles, i, j);
                        break;
                }
                    
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


   /* void InitialiseList() //CAMBIAR-HO PER SES POSICIONS QUE SIGUIN IGUAL A FLOOR
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
