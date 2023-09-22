using UnityEngine;

public class MoveSelectionState : State
{
    public override void Enter()
    {
        Debug.Log("MoveSelectionState");
        var moves = Board.Instance.selectedPiece.Movement.GetValidMoves();
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
        
        var tileClicked = highlight.Tile;

        Debug.Log(tileClicked.Position);
        Board.Instance.selectedHighlight = highlight;
        Machine.ChangeTo<PieceMovementState>();
    }
}
