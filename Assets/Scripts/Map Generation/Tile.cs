﻿using UnityEngine;

public enum TileType
{
    Wall, Floor, Corridor, 
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