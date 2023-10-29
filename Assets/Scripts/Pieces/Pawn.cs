using UnityEngine;

public class Pawn : Piece
{

    public Movement savedMovement;

    public Movement queenMovement = new QueenMovement();

    public Movement knightMovement = new KnightMovement();
    protected override void Start()
    {
        base.Start();
        Movement = savedMovement = new PawnMovement(maxTeam);
    }

     public override AffectedPiece CreateAffected()
    {

        AffectedPawn aff = new AffectedPawn();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
