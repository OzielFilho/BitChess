using UnityEngine;

public class Tile
{
    public Vector2Int Position;
    public Piece Content;
   

    public Tile(Vector2Int position)
    {
        Position = position;
    }
}
