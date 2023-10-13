using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightMovement : Movement
{
     public KnightMovement(){
        value = 3;
    }
    public override List<Tile> GetValidMoves(){
        List<Tile> moves = new List<Tile>();
         moves.AddRange(GetLMovement(new Vector2Int(0, 1)));
         moves.AddRange(GetLMovement(new Vector2Int(0, -1)));
         moves.AddRange(GetLMovement(new Vector2Int(1, 0)));
         moves.AddRange(GetLMovement(new Vector2Int(-1, 0)));
        return moves;
    }
    List<Tile> GetLMovement(Vector2Int direction){
            List<Tile> moves = new List<Tile>();
            Tile current = Board.Instance.selectedPiece.Tile;
            Tile temp = GetTile(current.Position+direction*2);
            if(temp != null)
            {
                moves.AddRange(GetCurvedPart(temp.Position, new Vector2Int(direction.y,direction.x)));
            }
            return moves;
    }
    List<Tile> GetCurvedPart(Vector2Int pos, Vector2Int direction){
    
        List<Tile> tiles = new List<Tile>();
        Tile tile1 = GetTile(pos+direction);
        Tile tile2 = GetTile(pos-direction);
        if(tile1 != null && (tile1.Content == null || IsEnemy(tile1)))
            tiles.Add(tile1);
        if(tile2 != null && (tile2.Content == null || IsEnemy(tile2)))
            tiles.Add(tile2);

            return tiles;   
}

}