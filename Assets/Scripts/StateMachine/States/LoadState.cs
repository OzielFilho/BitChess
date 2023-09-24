using System.Collections.Generic;
using System.Threading.Tasks;

public class LoadState : State
{
    public override async void Enter()
    {
        await Board.Instance.Load();
        await LoadAllPiecesAsync();
        Machine.currentlyPlayer = Machine.player2;
        Machine.ChangeTo<TurnBeginState>();
    }
    
    private async Task LoadAllPiecesAsync()
    {
        LoadTeamPieces(Board.Instance.bluePieces);
        LoadTeamPieces(Board.Instance.whitePieces);
        await Task.Delay(100);
    }
    
    private void LoadTeamPieces(List<Piece> pieces)
    {
        foreach (var piece in pieces)
        {
            Board.Instance.AddPiece(piece.transform.parent.name, piece);
        }
    }
}
