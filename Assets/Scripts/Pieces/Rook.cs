public class Rook : Piece
{
    void Awake()
    {
        Movement = new RookMovement();
    }

     public override AffectedPiece CreateAffected()
    {

        AffectedKingRook aff = new AffectedKingRook();
        aff.wasMoved = wasMoved;
        return aff;
    }
}
