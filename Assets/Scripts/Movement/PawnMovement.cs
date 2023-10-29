using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    Vector2Int direction;
    int promotionHeight;

    public PawnMovement(bool maxTeam)
    {
        if (maxTeam)
        {
            direction = new Vector2Int(0, 1);
            promotionHeight = 7;
            positionValue = AIController.instance.squareTable.pawnGold;
        }
        else
        {
            direction = new Vector2Int(0, -1);
            promotionHeight = 0;
            positionValue = AIController.instance.squareTable.pawnGreen;
        }

        value = 100;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moveable = new List<AvailableMove>();
        var moves = new List<AvailableMove>();
        GetPawnAttack(moveable);

        if (!Board.instance.selectedPiece.wasMoved)
        {
            UntilBlockedPath(moves, direction, false, 2);
            if (moves.Count == 2)
                moves[1] = new AvailableMove(moves[1].pos, MoveType.PawnDoubleMove);
        }
        else
        {
            UntilBlockedPath(moves, direction, false, 1);
            if (moves.Count > 0)
                moves[0] = CheckPromotion(moves[0]);
        }

        moveable.AddRange(moves);

        return moveable;
    }

    private void GetPawnAttack(List<AvailableMove> pawnAttack)
    {
        var piece = Board.instance.selectedPiece;
        var leftPos = new Vector2Int(piece.tile.position.x - 1, piece.tile.position.y + direction.y);
        var rightPos = new Vector2Int(piece.tile.position.x + 1, piece.tile.position.y + direction.y);

        GetPawnAttack(GetTile(leftPos), pawnAttack);
        GetPawnAttack(GetTile(rightPos), pawnAttack);
    }

    private void GetPawnAttack(Tile tile, IList<AvailableMove> pawnAttack)
    {
        if (tile == null)
            return;

        if (IsEnemy(tile))
            pawnAttack.Add(new AvailableMove(tile.position, MoveType.Normal));

        else if (PieceMovementState.enPassantFlag.moveType == MoveType.EnPassant && PieceMovementState.enPassantFlag.pos == tile.position)
            pawnAttack.Add(new AvailableMove(tile.position, MoveType.EnPassant));
    }

    private AvailableMove CheckPromotion(AvailableMove availableMove)
    {
        return availableMove.pos.y != promotionHeight ? availableMove : new AvailableMove(availableMove.pos, MoveType.Promotion);
    }
}