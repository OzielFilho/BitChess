using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenMovement : Movement
{
    public QueenMovement(bool maxTeam)
    {
        value = 900;
        positionValue = maxTeam ? AIController.instance.squareTable.queenGold : AIController.instance.squareTable.queenGreen;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 99);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 99);

        UntilBlockedPath(moves, new Vector2Int(1, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, 99);
        
        return moves;
    }
}