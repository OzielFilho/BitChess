public class AffectedPiece
{
    public Piece piece;
    public Tile from;
    public Tile to;

    public virtual void Undo()
    {
        piece.tile.content = null;
        piece.tile = from;
        from.content = piece;
    }
}

public class AffectedEnemy : AffectedPiece
{
    public int index;

    public override void Undo()
    {
        base.Undo();
        piece.gameObject.SetActive(true);
        piece.team.Insert(index, piece);
    }
}

public class AffectedKingRook : AffectedPiece
{
    public bool wasMoved;

    public override void Undo()
    {
        base.Undo();
        piece.wasMoved = wasMoved;
    }
}

public class AffectedPawn : AffectedPiece
{
    public bool resetMovement;
    public bool wasMoved;

    public override void Undo()
    {
        base.Undo();
        piece.wasMoved = wasMoved;
        if (resetMovement)
        {
            var pawn = piece as Pawn;
            piece.movement = pawn!.savedMovement;
        }
    }
}