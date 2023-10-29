using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public int value;
    public abstract List<AvailableMove> GetValidMoves();

    protected bool IsEnemy(Tile tile)
    {
        return tile.Content != null && tile.Content.transform.parent != Board.Instance.selectedPiece.transform.parent;
    }

    protected Tile GetTile(Vector2Int position)
    {
        Board.Instance.Tiles.TryGetValue(position, out var tile);
        return tile;
    }

    protected List<AvailableMove> UntilBlockedPath(Vector2Int direction, bool includeBlocked, int limit)
    {
        var moves = new List<AvailableMove>();
        var current = Board.Instance.selectedPiece.Tile;
        while (current != null && moves.Count < limit)
        {
            if (Board.Instance.Tiles.TryGetValue(current.Position + direction, out current))
            {
                if (current.Content == null)
                {
                    moves.Add(new AvailableMove( current.Position));
                }
                else if (IsEnemy(current))
                {
                    if (includeBlocked)
                    {
                        moves.Add(new AvailableMove(current.Position));
                    }

                    return moves;
                }
                else
                {
                    return moves;
                }
            }
        }

        return moves;
    }


}
