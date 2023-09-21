using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        var direction = GetDirection();

        var limit = 1;
        if(Board.Instance.SelectedPiece.wasMoved)
            limit = 2;
        var moveable = UntilBlockedPath(direction, false, limit);
        moveable.AddRange(GetPawnAttack(direction));
        
        return moveable;
    }

    private Vector2Int GetDirection()
    {
        if (StateMachineController.Instance.currentlyPlayer.transform.name == "BluePieces")
            return new Vector2Int(0, 1);
        return new Vector2Int(0, -1);
    }
    
    List<Tile> GetPawnAttack(Vector2Int direction)
    {
        var pawnAttack = new List<Tile>();
        Tile temp;
        var piece = Board.Instance.SelectedPiece;
        var leftPos = new Vector2Int(piece.Tile.Position.x - 1, piece.Tile.Position.y + direction.y);
        var rightPos = new Vector2Int(piece.Tile.Position.x + 1, piece.Tile.Position.y + direction.y);
        temp = GetTile(leftPos);
        if (temp != null && IsEnemy(temp))
        {
            pawnAttack.Add(temp);
        }
        temp = GetTile(rightPos);
        if (temp != null && IsEnemy(temp))
        {
            pawnAttack.Add(temp);
        }

        return pawnAttack;
    }
}
