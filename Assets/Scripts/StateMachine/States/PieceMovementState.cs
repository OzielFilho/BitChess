using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    private AudioController audioController;
    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        var piece = Board.Instance.selectedPiece;
        piece.Tile.Content = null;
        piece.Tile = Board.Instance.selectedHighlight.Tile;

        if (piece.Tile.Content != null)
        {
            audioController = GetComponent<AudioController>();
            audioController.Play(this);
            var deadPiece = piece.Tile.Content;
            Debug.LogFormat("Peça {0} foi morta", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }
        piece.Tile.Content = piece;
        piece.wasMoved = true;

        var tcs = new TaskCompletionSource<bool>();
        var target = Board.Instance.selectedHighlight.transform.position;
        var timing = Vector3.Distance(piece.transform.position, target) * 0.5f;
        LeanTween.move(piece.gameObject, target, timing).setOnComplete(() => tcs.SetResult(true));

        await Task.Delay(100);
        Machine.ChangeTo<TurnEndState>();
    }
}
