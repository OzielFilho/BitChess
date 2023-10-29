using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : Movement
{
    public RookMovement(bool maxTeam)
    {
        value = 500;
        positionValue = maxTeam ? AIController.instance.squareTable.rookGold : AIController.instance.squareTable.rookGreen;
    }

    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();
        UntilBlockedPath(moves, new Vector2Int(1, 0), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 0), true, 99);

        UntilBlockedPath(moves, new Vector2Int(0, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(0, -1), true, 99);
        return moves;
    }
}