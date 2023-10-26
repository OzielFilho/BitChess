using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RookMovement : Movement
{

     public RookMovement(){
        value = 500;
    }
    public override List<AvailableMove> GetValidMoves()
    {
        List<AvailableMove> moves = new List<AvailableMove>();
        moves.AddRange(UntilBlockedPath(new Vector2Int(1, 0), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(-1, 0), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, 1), true, 99));
        moves.AddRange(UntilBlockedPath(new Vector2Int(0, -1), true, 99));

        return moves;
    }
}