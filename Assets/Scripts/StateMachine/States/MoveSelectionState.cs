using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("MoveSelectionState");
        List<AvailableMove> moves = Board.Instance.selectedPiece.Movement.GetValidMoves();
        Highlights.Instance.SelectTiles(moves);
        Board.Instance.TileClicked += OnHighlightClicked;
    }
    
    public override void Exit()
    {
        Highlights.Instance.DeSelectTiles();
        Board.Instance.TileClicked -= OnHighlightClicked;
    }

    private void OnHighlightClicked(object sender, object args)
    {
        var highlight = sender as HighlightClick;
        if(highlight == null) return;
      
        Board.Instance.selectedMove = highlight.move;
        Machine.ChangeTo<PieceMovementState>();
    }
    void ReturnClicked(object sender, object args){
        Machine.ChangeTo<PieceSelectionState>();
    }
}
