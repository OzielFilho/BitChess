using UnityEngine;

public class Tile
{
    public Vector2Int Position;
    public Piece Content;
    public MoveType MoveType;

    public Tile(Vector2Int position)
    {
        Position = position;
    }
}
