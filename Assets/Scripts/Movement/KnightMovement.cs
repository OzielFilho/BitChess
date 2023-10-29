using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
     public KnightMovement(){
        value = 300;
    }
    public override List<AvailableMove> GetValidMoves()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
         moves.AddRange(GetLMovement(new Vector2Int(0, 1)));
         moves.AddRange(GetLMovement(new Vector2Int(0, -1)));
         moves.AddRange(GetLMovement(new Vector2Int(1, 0)));
         moves.AddRange(GetLMovement(new Vector2Int(-1, 0)));
        return moves;
    }
    List<AvailableMove> GetLMovement(Vector2Int direction)
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        Tile current = Board.Instance.selectedPiece.Tile;
        Tile temp = GetTile(current.Position + direction * 2);
        if (temp != null)
        {
            moves.AddRange(GetCurvedPart(temp.Position, new Vector2Int(direction.y, direction.x)));
        }
        return moves;
    }
    List<AvailableMove> GetCurvedPart(Vector2Int pos, Vector2Int direction)
    {

        List<AvailableMove> availableMoves = new List<AvailableMove>();
        Tile tile1 = GetTile(pos + direction);
        Tile tile2 = GetTile(pos - direction);
        if (tile1 != null && (tile1.Content == null || IsEnemy(tile1)))
            availableMoves.Add(new AvailableMove(tile1.Position));
        if (tile2 != null && (tile2.Content == null || IsEnemy(tile2)))
            availableMoves.Add(new AvailableMove( tile2.Position));

        return availableMoves;
    }

}