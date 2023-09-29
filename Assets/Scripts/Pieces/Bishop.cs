public class Bishop : Piece
{
    void Awake()
    {
        Movement = new BishopMovement();
    }
}
