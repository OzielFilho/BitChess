using UnityEngine;

public struct AvailableMove
{
    public AvailableMove(Vector2Int rcvPos, MoveType rcvMoveType)
    {
        pos = rcvPos;
        moveType = rcvMoveType;
    }

    public AvailableMove(Vector2Int rcvPos)
    {
        pos = rcvPos;
        moveType = MoveType.Normal;
    }

    public Vector2Int pos;
    public MoveType moveType;
}