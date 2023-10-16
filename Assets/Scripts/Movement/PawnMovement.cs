using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        Vector2Int direction = GetDirection();
        List<Tile> moveable = GetPawnAttack(direction);
        List<Tile> moves;

        if (!Board.Instance.selectedPiece.wasMoved)
        {
            moves = UntilBlockedPath(direction, false, 2);
            SetNormalMove(moves);
            if (moves.Count == 2)
                moves[1].MoveType = MoveType.PawnDoubleMove;
        }
        else
        {
            moves = UntilBlockedPath(direction, false, 1);
            SetNormalMove(moves);
        }

        moveable.AddRange(moves);

        CheckPromotion(moves);

        return moveable;
    }
    private Vector2Int GetDirection()
    {
        return StateMachineController.Instance.currentlyPlayer.transform.name == "BluePieces"
            ? new Vector2Int(0, 1)
            : new Vector2Int(0, -1);
    }

    List<Tile> GetPawnAttack(Vector2Int direction)
    {
        List<Tile> pawnAttack = new List<Tile>();
        Piece piece = Board.Instance.selectedPiece;
        Vector2Int leftPos = new Vector2Int(piece.Tile.Position.x - 1, piece.Tile.Position.y + direction.y);
        Vector2Int rightPos = new Vector2Int(piece.Tile.Position.x + 1, piece.Tile.Position.y + direction.y);

        GetPawnAttack(GetTile(leftPos), pawnAttack);
        GetPawnAttack(GetTile(rightPos), pawnAttack);

        return pawnAttack;
    }

    void GetPawnAttack(Tile tile, List<Tile> pawnAttack)
    {
        if (tile == null)
            return;
        if (IsEnemy(tile))
        {
            tile.MoveType = MoveType.Normal;
            pawnAttack.Add(tile);
        }
        else if (tile.MoveType == MoveType.EnPassant)
        {
            pawnAttack.Add(tile);
        }
    }

    void CheckPromotion(List<Tile> tiles)
    {
        foreach (Tile t in tiles)
        {
            if (t.Position.y == 0 || t.Position.y == 7)
                t.MoveType = MoveType.Promotion;
        }
    }
}
