public class Bishop : Piece
{
    protected override void Start(){
        base.Start();
        movement = new BishopMovement(maxTeam);
    }
}
