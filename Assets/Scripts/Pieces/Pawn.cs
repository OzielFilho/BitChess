public class Pawn : Piece
{
    void Awake()
    {
        Movement = new PawnMovement();
    }
}
