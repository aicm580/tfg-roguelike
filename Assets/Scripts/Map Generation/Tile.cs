using UnityEngine;

public enum TileType
{
    TopFloor, TopWall, BottomWall, LeftWall, RightWall, RoomFloor, CorridorWall, 
} 

class Tile
{
    public TileType tileType;
    public Vector2 pos = new Vector2();

    public Tile(TileType tiletype, Vector2 v)
    {
        tileType = tiletype;
        pos = v;
    }
}
