public class King : Piece
{
    void Awake()
    {
        Movement = new KingMovement();
    }
}
