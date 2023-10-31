public class Knight : Piece
{
    protected override void Start()
    {
        base.Start();
        movement = new KnightMovement(maxTeam);
    }
}