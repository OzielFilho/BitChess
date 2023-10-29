using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMovement : Movement
{
    public KingMovement(bool maxTeam)
    {
        value = 100000;
        positionValue = maxTeam ? AIController.instance.squareTable.kingGold : AIController.instance.squareTable.kingGreen;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 1);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 1);

        UntilBlockedPath(moves, new Vector2Int(1, 1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 1);
        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, 1);

        Castling(moves);
        return moves;
    }

    private void Castling(List<AvailableMove> moves)
    {
        if (Board.instance.selectedPiece.wasMoved)
            return;

        var temp = CheckRook(new Vector2Int(1, 0));
        if (temp != null)
        {
            moves.Add(new AvailableMove(temp.position, MoveType.Castling));
        }

        temp = CheckRook(new Vector2Int(-1, 0));
        if (temp != null)
        {
            moves.Add(new AvailableMove(temp.position, MoveType.Castling));
        }
    }

    private Tile CheckRook(Vector2Int direction)
    {
        Rook rook;
        var currentTile = GetTile(Board.instance.selectedPiece.tile.position + direction);

        while (currentTile != null)
        {
            if (currentTile.content != null)
                break;
            currentTile = GetTile(currentTile.position + direction);
        }

        if (currentTile == null)
            return null;
        rook = currentTile.content as Rook;
        if (rook == null || rook.wasMoved)
            return null;
        return rook.tile;
    }
}