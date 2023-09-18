using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMovement : Movement
{
    public override List<Tile> GetValidMoves()
    {
        var temp = new List<Vector2Int>();
        var direction = GetDirection();
        temp.Add(Board.Instance.SelectedPiece.Tile.Position + direction);
        var exists = ValidateExists(temp);
        var moveable = UntilBlockedPath(exists);
        moveable.AddRange(GetPawnAttack(direction));
        
        return moveable;
    }

    private Vector2Int GetDirection()
    {
        if (StateMachineController.Instance.currentlyPlayer.transform.name == "BluePieces")
            return new Vector2Int(0, 1);
        return new Vector2Int(0, -1);
    }

    private List<Tile> ValidateExists(List<Vector2Int> positions)
    {
        var rtv = new List<Tile>();
        foreach (var position in positions)
        {
            Tile tile;
            if (Board.Instance.Tiles.TryGetValue(position, out tile))
            {
                rtv.Add(tile);
            }
        }

        return rtv;
    }
    
    private List<Tile> UntilBlockedPath(List<Tile> tiles)
    {
        var valid = new List<Tile>();
        foreach (var tile in tiles)
        {
            if (tile.Content == null)
            {
                valid.Add(tile);
            }
        }

        return valid;
    }

    bool IsEnemy(Vector2Int position, out Tile temp)
    {
        if (Board.Instance.Tiles.TryGetValue(position, out temp))
        {
            if (temp != null && temp.Content != null)
            {
                if(temp.Content.transform.parent != Board.Instance.SelectedPiece.transform.parent)
                    return true;
            }
        }

        return false;
    }

    List<Tile> GetPawnAttack(Vector2Int direction)
    {
        var pawnAttack = new List<Tile>();
        var piece = Board.Instance.SelectedPiece;
        var leftPos = new Vector2Int(piece.Tile.Position.x - 1, piece.Tile.Position.y + direction.y);
        var rightPos = new Vector2Int(piece.Tile.Position.x + 1, piece.Tile.Position.y + direction.y);
        if (IsEnemy(leftPos, out var temp))
        {
            pawnAttack.Add(temp);
        }
        if (IsEnemy(rightPos, out temp))
        {
            pawnAttack.Add(temp);
        }

        return pawnAttack;
    }
}
