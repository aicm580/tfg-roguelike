using UnityEngine;

public enum TileType
{
    TopFloor, Wall, RoomFloor, 
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
