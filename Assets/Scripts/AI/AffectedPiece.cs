using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AffectedPiece 
{
  
  public Piece piece;

  public Tile from;

  public Tile to;

  public virtual void Undo(){
    piece.Tile.Content = null;
    piece.Tile = from;
    from.Content = piece;
    piece.gameObject.SetActive(true);
  }

  
}

public class AffectedKingRook:AffectedPiece{
public bool wasMoved;
    public override void Undo()
    {
        base.Undo();
        piece.wasMoved = wasMoved;
    }

}

public class AffectedPawn:AffectedPiece{
public bool resetMovement;

public bool wasMoved;
    public override void Undo()
    {
        base.Undo();
        piece.wasMoved = wasMoved;
        if(resetMovement)
        {
          Pawn pawn = piece as Pawn;
          piece.Movement = pawn.savedMovement;
        }
    }

}