using UnityEngine;

public class Tile
{
    public Vector2Int position;
    public Piece content;
    
    public Tile(Vector2Int position)
    {
        this.position = position;
    }
}
