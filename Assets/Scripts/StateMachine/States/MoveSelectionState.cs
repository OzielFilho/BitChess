using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("MoveSelectionState");
        var moves = Board.Instance.SelectedPiece.Movement.GetValidMoves();
        Highlights.Instance.SelectTiles(moves);
        Board.Instance.TileClicked += OnHighlightClicked;
    }
    
    public override void Exit()
    {
        Highlights.Instance.DeSelectTiles();
        Board.Instance.TileClicked -= OnHighlightClicked;
    }

    void OnHighlightClicked(object sender, object args)
    {
        var highlight = sender as HighlightClick;
        if(highlight == null) return;
        
        var tileClicked = highlight.Tile;

        Debug.Log(tileClicked.Position);
        Board.Instance.SelectedHighlight = highlight;
        Machine.ChangeTo<PieceMovementState>();
    }
}
