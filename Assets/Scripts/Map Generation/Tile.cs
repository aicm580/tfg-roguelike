using UnityEngine;

public enum TileType
{
    Surroundings, RoomTopWall, RoomBottomWall, RoomSideWall, RoomFloor, CorridorFloor, CorridorWall, 
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
