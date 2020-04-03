using UnityEngine;

public enum TileType
{
    TopFloor, Wall, RoomFloor, SmallObstacle //indicamos que se trata de un Small Obstacle, por si en el futuro queremos añadir BigObstacles (ej:árbol)
} 

class Tile
{
    public TileType tileType;
    public Vector3Int pos = new Vector3Int();

    public Tile(TileType tiletype, Vector3Int v)
    {
        tileType = tiletype;
        pos = v;
    }
}
