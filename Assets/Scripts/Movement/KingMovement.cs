using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        List<Tile> moves = new List<Tile>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 1));

        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 1));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 1));

        SetNormalMove(moves);

        moves.AddRange(Castling());
        return moves;
    }

    List<Tile> Castling()
    {
        List<Tile> moves = new List<Tile>();
        if (Board.Instance.selectedPiece.wasMoved)
            return moves;

        Tile temp = CheckRook(new Vector2Int(1, 0));
        if (temp != null)
        {
            temp.MoveType = MoveType.Castling;
            moves.Add(temp);
        }
        temp = CheckRook(new Vector2Int(-1, 0));
        if (temp != null)
        {
            temp.MoveType = MoveType.Castling;
            moves.Add(temp);
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
