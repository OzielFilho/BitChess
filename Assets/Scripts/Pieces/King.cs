public class King : Piece
{
    void Awake()
    {
        Movement = new KingMovement();
    }

    public override AffectedPiece CreateAffected()
    {

        AffectedKingRook aff = new AffectedKingRook();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
