public class Queen : Piece
{
    void Awake()
    {
        Movement = new QueenMovement();
    }
}
