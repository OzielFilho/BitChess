using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement
{
    public abstract List<Tile> GetValidMoves();

    protected bool IsEnemy(Tile tile)
    {
        return tile.Content != null && tile.Content.transform.parent != Board.Instance.SelectedPiece.transform.parent;
    }

    protected Tile GetTile(Vector2Int position)
    {
        Board.Instance.Tiles.TryGetValue(position, out var tile);
        return tile;
    }

    protected List<Tile> UntilBlockedPath(Vector2Int direction, bool includeBlocked, int limit)
    {
        var moves = new List<Tile>();
        var current = Board.Instance.SelectedPiece.Tile;
        while (current != null && moves.Count < limit)
        {
            if (Board.Instance.Tiles.TryGetValue(current.Position + direction, out current))
            {
                if (current.Content == null)
                {
                    moves.Add(current);
                } 
                else if (IsEnemy(current))
                {
                    if(includeBlocked)
                    {
                        moves.Add(current);
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
