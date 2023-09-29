public class Rook : Piece
{
    void Awake()
    {
        Movement = new RookMovement();
    }
}
