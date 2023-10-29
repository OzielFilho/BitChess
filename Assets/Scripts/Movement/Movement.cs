using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public int value;
    public abstract List<AvailableMove> GetValidMoves();
    public Dictionary<Vector2Int, int> positionValue;

    protected bool IsEnemy(Tile tile)
    {
        return tile.content != null && tile.content.transform.parent != Board.instance.selectedPiece.transform.parent;
    }

    protected Tile GetTile(Vector2Int position)
    {
        Board.instance.tiles.TryGetValue(position, out var tile);
        return tile;
    }

    protected void UntilBlockedPath(List<AvailableMove> moves, Vector2Int direction, bool includeBlocked, int limit)
    {
        var current = Board.instance.selectedPiece.tile;
        var currentCount = moves.Count;
        while (current != null && moves.Count < limit + currentCount)
        {
            if (Board.instance.tiles.TryGetValue(current.position + direction, out current))
            {
                if (current.content == null)
                {
                    moves.Add(new AvailableMove(current.position));
                }
                else if (IsEnemy(current))
                {
                    if (includeBlocked)
                        moves.Insert(0, new AvailableMove(current.position));
                    return;
                }
                else
                {
                    //era um aliado
                    return;
                }
            }
        }
    }
}