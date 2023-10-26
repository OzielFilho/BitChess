using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : Movement
{

     public KingMovement(){
        value = 100000;
    }
    public override List<AvailableMove> GetValidMoves()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 1));


        moves.AddRange(Castling());
        return moves;
    }

    List<AvailableMove> Castling()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        if (Board.Instance.selectedPiece.wasMoved)
            return moves;

        Tile temp = CheckRook(new Vector2Int(1, 0));
        if (temp != null)
        {
            moves.Add(new AvailableMove(temp.Position,MoveType.Castling));
        }
        temp = CheckRook(new Vector2Int(-1, 0));
        if (temp != null)
        {
            moves.Add(new AvailableMove(temp.Position,MoveType.Castling));
        }

        return moves;
    }

    Tile CheckRook(Vector2Int direction)
    {
        Rook rook;
        Tile currentTile = GetTile(Board.Instance.selectedPiece.Tile.Position + direction);

        while (currentTile != null)
        {
            if (currentTile.Content != null)
                break;
            currentTile = GetTile(currentTile.Position + direction);
        }
        if (currentTile == null)
            return null;
        rook = currentTile.Content as Rook;
        if (rook == null || rook.wasMoved)
            return null;
        return rook.Tile;
    }
}
