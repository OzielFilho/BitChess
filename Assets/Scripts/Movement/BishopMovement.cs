using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BishopMovement : Movement
{
    public BishopMovement(){
        value = 300;
    }
    public override List<AvailableMove> GetValidMoves()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, -1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 1), true, 99));
        return moves;
    }
}
