using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board Instance;
    public Dictionary<Vector2Int, Tile> Tiles = new ();
    public List<Piece> BluePieces = new ();
    public List<Piece> WhitePieces = new ();

    void Awake()
    {
        Instance = this;
        CreateBoard();
    }

    public void AddPiece(string team, Piece piece)
    {
        var piecePosition = piece.transform.position;
        var position = new Vector2Int((int)piecePosition.x, (int)piecePosition.y);
        piece.Tile = Tiles[position];
        piece.Tile.Content = piece;

        if (team == "BluePieces")
        {
            BluePieces.Add(piece);
        }
        else
        {
            WhitePieces.Add(piece);
        }
    }

    private void CreateBoard()
    {
        for(var i = 0; i<8; i++)
        {
            for(var j = 0; j<8; j++)
            {
                CreateTile(i, j);
            }
        }
    }
    
    void CreateTile(int i, int j)
    {
        var position = new Vector2Int(i, j);
        var tile = new Tile(position);
        Tiles.Add(tile.Position, tile);
    }
}
