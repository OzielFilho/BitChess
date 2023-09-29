public class Knight : Piece
{
    void Awake()
    {
        Movement = new KnightMovement();
    }
}
