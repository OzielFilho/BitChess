public class King : Piece
{
    protected override void Start()
    {
        base.Start();
        movement = new KingMovement(maxTeam);
    }

    public override AffectedPiece CreateAffected()
    {
        return new AffectedKingRook
        {
            wasMoved = wasMoved
        };
    }
}