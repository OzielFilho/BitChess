using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Movement
{
    public BishopMovement(bool maxTeam)
    {
        value = 300;
        positionValue = maxTeam ? AIController.instance.squareTable.bishopGold : AIController.instance.squareTable.bishopGreen;
    }
    
    public override List<AvailableMove> GetValidMoves()
    {
        var moves = new List<AvailableMove>();

        UntilBlockedPath(moves, new Vector2Int(1, 1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, -1), true, 99);
        UntilBlockedPath(moves, new Vector2Int(-1, 1), true, 99);
        return moves;
    }
}
