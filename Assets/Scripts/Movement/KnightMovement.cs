using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
    public KnightMovement(bool maxTeam)
    {
        value = 300;
        positionValue = maxTeam ? AIController.instance.squareTable.knightGold : AIController.instance.squareTable.knightGreen;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        moves.AddRange(GetLMovement(new Vector2Int(0, 1)));
        moves.AddRange(GetLMovement(new Vector2Int(0, -1)));
        moves.AddRange(GetLMovement(new Vector2Int(1, 0)));
        moves.AddRange(GetLMovement(new Vector2Int(-1, 0)));
        return moves;
    }

    List<AvailableMove> GetLMovement(Vector2Int direction)
    {
        var moves = new List<AvailableMove>();
        var current = Board.instance.selectedPiece.tile;
        var temp = GetTile(current.position + direction * 2);
        if (temp != null)
        {
            moves.AddRange(GetCurvedPart(temp.position, new Vector2Int(direction.y, direction.x)));
        }

        return moves;
    }

    private IEnumerable<AvailableMove> GetCurvedPart(Vector2Int pos, Vector2Int direction)
    {
        var availableMoves = new List<AvailableMove>();
        var tile1 = GetTile(pos + direction);
        var tile2 = GetTile(pos - direction);
        
        if (tile1 != null && (tile1.content == null || IsEnemy(tile1)))
            availableMoves.Add(new AvailableMove(tile1.position));
        
        if (tile2 != null && (tile2.content == null || IsEnemy(tile2)))
            availableMoves.Add(new AvailableMove(tile2.position));

        return availableMoves;
    }
}