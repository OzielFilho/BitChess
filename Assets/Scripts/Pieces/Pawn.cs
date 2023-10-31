public class Pawn : Piece
{
    public Movement savedMovement;
    public Movement queenMovement;
    public Movement knightMovement;

    protected override void Start()
    {
        base.Start();
        movement = savedMovement = new PawnMovement(maxTeam);
        queenMovement = new QueenMovement(maxTeam);
        knightMovement = new KnightMovement(maxTeam);
    }

    public override AffectedPiece CreateAffected()
    {
        return new AffectedPawn
        {
            wasMoved = wasMoved
        };
    }
}