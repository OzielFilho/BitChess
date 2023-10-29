using UnityEngine;


public class PieceSelectionState : State
{
    private AudioController audioController;
    
    public override void Enter()
    {
        Board.Instance.TileClicked += PieceClicked;
    }

    public override void Exit()
    {
        Board.Instance.TileClicked -= PieceClicked;
    }

    private void PieceClicked(object sender, object args)
    {
        var piece = sender as Piece;
        var player = args as Player;

        if (Machine.currentlyPlayer == player)
        {
            audioController = GetComponent<AudioController>();
            audioController.Play(this);
            Debug.Log(piece + " was clicked");
            Board.Instance.selectedPiece = piece;
            Machine.ChangeTo<MoveSelectionState>();
        }
    }

}
