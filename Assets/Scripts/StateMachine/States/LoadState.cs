using System.Collections.Generic;
using System.Threading.Tasks;

public class LoadState : State
{
    public override async void Enter()
    {
        await Board.instance.Load();
        await LoadAllPiecesAsync();
        machine.currentlyPlayer = machine.player1;
        machine.ChangeTo<TurnBeginState>();
    }

    private async Task LoadAllPiecesAsync()
    {
        LoadTeamPieces(Board.instance.bluePieces);
        LoadTeamPieces(Board.instance.whitePieces);
        await Task.Delay(100);
    }

    private void LoadTeamPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            Board.instance.AddPiece(piece.transform.parent.name, piece);
        }
    }
}