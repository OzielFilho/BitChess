using System.Threading.Tasks;
using UnityEngine;


public class PieceMovementState : State
{
    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        var piece = Board.Instance.SelectedPiece;
        piece.transform.position = Board.Instance.SelectedHighlight.transform.position;
        piece.Tile.Content = null;
        piece.Tile = Board.Instance.SelectedHighlight.Tile;
        if (piece.Tile.Content != null)
        {
            var deadPiece = piece.Tile.Content;
            Debug.LogFormat("Peça {0} foi morta", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }
        piece.Tile.Content = piece;

        await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }
}
