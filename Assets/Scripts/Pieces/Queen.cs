public class Queen : Piece
{
    protected override void Start()
    {
        base.Start();
        movement = new QueenMovement(maxTeam);
    }
}