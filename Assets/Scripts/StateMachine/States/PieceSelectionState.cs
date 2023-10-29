using UnityEngine;


public class PieceSelectionState : State
{
    private AudioController audioController;
    public override void Enter()
    {
        InputController.instance.tileClicked += PieceClicked;
        SetColliders(true);
    }

    public override void Exit()
    {
        InputController.instance.tileClicked -= PieceClicked;
        SetColliders(false);
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
            machine.ChangeTo<MoveSelectionState>();
        }
    }

    private void SetColliders(bool state)
    {
        foreach (var b in machine.currentlyPlayer.GetComponentsInChildren<BoxCollider2D>())
        {
            b.enabled = state;
        }
    }
}
