using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PieceSelectionState : State
{
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
            Debug.Log(piece + " was clicked");
            Board.Instance.SelectedPiece = piece;
            Machine.ChangeTo<MoveSelectionState>();
        }
    }
}
