public class Rook : Piece
{
    protected override void Start()
    {
        base.Start();
        movement = new RookMovement(maxTeam);
    }

    public override AffectedPiece CreateAffected()
    {
        return new AffectedKingRook
        {
            wasMoved = wasMoved
        };
    }
}