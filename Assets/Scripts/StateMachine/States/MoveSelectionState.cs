using UnityEngine;

public class MoveSelectionState : State
{
    private AudioController audioController;
    
    public override void Enter()
    {
        Debug.Log("MoveSelectionState");
        var moves = Board.instance.selectedPiece.movement.GetValidMoves();
        Highlights.instance.SelectTiles(moves);
        
        SetColliders(true);

        InputController.instance.tileClicked+= OnHighlightClicked;
        InputController.instance.returnClicked+= ReturnClicked;
        InputController.instance.tileClicked+= PieceClicked;
    }

    public override void Exit()
    {
        Highlights.instance.DeSelectTiles();
        InputController.instance.tileClicked-= OnHighlightClicked;
        InputController.instance.returnClicked-= ReturnClicked;
        InputController.instance.tileClicked-= PieceClicked;
        SetColliders(false);
    }

    private void OnHighlightClicked(object sender, object args)
    {
        var highlight = sender as HighlightClick;
        if (highlight == null) return;

        Board.instance.selectedMove = highlight.move;
        machine.ChangeTo<PieceMovementState>();
    }

    void ReturnClicked(object sender, object args)
    {
        machine.ChangeTo<PieceSelectionState>();
    }
    
    private void PieceClicked(object sender, object args)
    {
        var piece = sender as Piece;
        var player = args as Player;

        if (machine.currentlyPlayer == player)
        {
            audioController = GetComponent<AudioController>();
            audioController.Play(this);
            Debug.Log(piece + " was clicked");
            Board.instance.selectedPiece = piece;
            Exit();
            Enter();
        }
    }
}