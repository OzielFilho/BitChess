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

        MoveType moveType = Board.Instance.selectedHighlight.Tile.MoveType;

        ClearEnPassants();

        var tcs = new TaskCompletionSource<bool>();

        switch (moveType)
        {
            case MoveType.Normal:
                NormalMove(tcs);
                break;
            case MoveType.Castling:
                Castling(tcs);
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove(tcs);
                break;
            case MoveType.EnPassant:
                EnPassant(tcs);
                break;
            case MoveType.Promotion:
                Promotion(tcs);
                break;
        }

        await Task.Delay(100);

        Machine.ChangeTo<TurnEndState>();
    }

    void NormalMove(TaskCompletionSource<bool> tcs)
    {
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

        var target = Board.Instance.selectedHighlight.transform.position;
        var timing = Vector3.Distance(piece.transform.position, target) * 0.5f;
        LeanTween.move(piece.gameObject, target, timing).setOnComplete(() => tcs.SetResult(true));
    }

    void Castling(TaskCompletionSource<bool> tcs)
    {
        Piece king = Board.Instance.selectedPiece;
        king.Tile.Content = null;
        Piece rook = Board.Instance.selectedHighlight.Tile.Content;
        rook.Tile.Content = null;

        Vector2Int direction = rook.Tile.Position - king.Tile.Position;
        if (direction.x > 0)
        {
            king.Tile = Board.Instance.Tiles[new Vector2Int(king.Tile.Position.x + 2, king.Tile.Position.y)];
            rook.Tile = Board.Instance.Tiles[new Vector2Int(king.Tile.Position.x - 1, king.Tile.Position.y)];
        }
        else
        {
            king.Tile = Board.Instance.Tiles[new Vector2Int(king.Tile.Position.x - 2, king.Tile.Position.y)];
            rook.Tile = Board.Instance.Tiles[new Vector2Int(king.Tile.Position.x + 1, king.Tile.Position.y)];
        }
        king.Tile.Content = king;
        rook.Tile.Content = rook;
        king.wasMoved = true;
        rook.wasMoved = true;

        LeanTween.move(king.gameObject, new Vector3(king.Tile.Position.x, king.Tile.Position.y, 0), 1.5f).
            setOnComplete(() =>
            {
                tcs.SetResult(true);
            });
        LeanTween.move(rook.gameObject, new Vector3(rook.Tile.Position.x, rook.Tile.Position.y, 0), 1.4f);
    }

    void ClearEnPassants()
    {
        ClearEnPassants(5);
        ClearEnPassants(2);
    }

    void ClearEnPassants(int height)
    {
        Vector2Int positions = new Vector2Int(0, height);
        for (int i = 0; i < 7; i++)
        {
            positions.x = positions.x + 1;
            Board.Instance.Tiles[positions].MoveType = MoveType.Normal;
        }
    }

    void PawnDoubleMove(TaskCompletionSource<bool> tcs)
    {
        Piece pawn = Board.Instance.selectedPiece;
        Vector2Int direction = pawn.Tile.Position.y > Board.Instance.selectedHighlight.Tile.Position.y ?
            new Vector2Int(0, -1) :
            new Vector2Int(0, 1);
        Board.Instance.Tiles[pawn.Tile.Position + direction].MoveType = MoveType.EnPassant;
        NormalMove(tcs);
    }

    void EnPassant(TaskCompletionSource<bool> tcs)
    {
        Piece pawn = Board.Instance.selectedPiece;
        Vector2Int direction = pawn.Tile.Position.y > Board.Instance.selectedHighlight.Tile.Position.y ?
            new Vector2Int(0, 1) :
            new Vector2Int(0, -1);
        Tile enemy = Board.Instance.Tiles[Board.Instance.selectedHighlight.Tile.Position + direction];
        enemy.Content.gameObject.SetActive(false);
        enemy.Content = null;
        NormalMove(tcs);
    }

    async void Promotion(TaskCompletionSource<bool> tcs)
    {
        TaskCompletionSource<bool> movementTCS = new TaskCompletionSource<bool>();
        NormalMove(movementTCS);
        await movementTCS.Task;
        Debug.Log("Promoção do Peão");

        StateMachineController.Instance.TaskHold = new TaskCompletionSource<object>();
        StateMachineController.Instance.PromotionPanel.SetActive(true);

        await StateMachineController.Instance.TaskHold.Task;

        string result = StateMachineController.Instance.TaskHold.Task.Result as string;
        if (result == "Knight")
        {
            Board.Instance.selectedPiece.Movement = new KnightMovement();
        }
        else
        {
            Board.Instance.selectedPiece.Movement = new QueenMovement();
        }

        StateMachineController.Instance.PromotionPanel.SetActive(false);
        tcs.SetResult(true);
    }
}
