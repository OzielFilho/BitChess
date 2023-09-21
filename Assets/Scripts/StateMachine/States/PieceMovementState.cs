using System.Threading.Tasks;
using UnityEngine;


public class PieceMovementState : State
{
    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        var piece = Board.Instance.SelectedPiece;
        piece.Tile.Content = null;
        piece.Tile = Board.Instance.SelectedHighlight.Tile;
        
        if (piece.Tile.Content != null)
        {
            var deadPiece = piece.Tile.Content;
            Debug.LogFormat("Peça {0} foi morta", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }
        piece.Tile.Content = piece;
        piece.wasMoved = true;

        var tcs = new TaskCompletionSource<bool>();
        var target = Board.Instance.SelectedHighlight.transform.position;
        var timing = Vector3.Distance(piece.transform.position, target) * 0.5f;
        LeanTween.move(piece.gameObject, target, timing).setOnComplete(() => tcs.SetResult(true));

        await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }
}
