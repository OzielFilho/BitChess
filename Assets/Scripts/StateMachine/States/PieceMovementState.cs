using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    public static AvailableMove enPassantFlag;
    private AudioController audioController;
    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
        MovePiece(tcs, false, Board.Instance.selectedMove.moveType);
        await tcs.Task;
        Machine.ChangeTo<TurnEndState>();
    }

    public void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovements, MoveType moveType)
    {
        changes = new List<AffectedPiece>();
        enPassantFlag = new AvailableMove();

        switch (moveType)
        {
            case MoveType.Normal:
                NormalMove(tcs, skipMovements);
                break;
            case MoveType.Castling:
                Castling(tcs, skipMovements);
                break;
            case MoveType.PawnDoubleMove:
                PawnDoubleMove(tcs, skipMovements);
                break;
            case MoveType.EnPassant:
                EnPassant(tcs, skipMovements);
                break;
            case MoveType.Promotion:
                Promotion(tcs, skipMovements);
                break;
        }
    }

    void NormalMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var piece = Board.Instance.selectedPiece;
        AffectedPiece pieceMoving = piece.CreateAffected();
        pieceMoving.piece = piece;
        pieceMoving.from = piece.Tile;
        pieceMoving.to = Board.Instance.Tiles[Board.Instance.selectedMove.pos];
        changes.Insert(0, pieceMoving);
        piece.Tile.Content = null;
        piece.Tile = pieceMoving.to;

        if (piece.Tile.Content != null)
        {
            // audioController = GetComponent<AudioController>();
            // audioController.Play(this);
            Piece deadPiece = piece.Tile.Content;
            AffectedPiece pieceKilled = new AffectedPiece();
            pieceKilled.piece = deadPiece;
            pieceKilled.from = pieceKilled.to = piece.Tile;
            changes.Add(pieceKilled);
            Debug.LogFormat("Peça {0} foi morta", deadPiece.transform);
            deadPiece.gameObject.SetActive(false);
        }



        piece.Tile.Content = piece;
        piece.wasMoved = true;

        Vector3 v3Pos = new Vector3(Board.Instance.selectedMove.pos.x, Board.Instance.selectedMove.pos.y, 0);

        if (skipMovements)
        {
            // piece.transform.position = v3Pos;
            tcs.SetResult(true);
        }
        else
        {
            piece.wasMoved = true;
            var target = v3Pos;
            var timing = Vector3.Distance(piece.transform.position, target) * 0.5f;
            LeanTween.move(piece.gameObject, target, timing).setOnComplete(() => tcs.SetResult(true));
        }


    }

    void Castling(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece king = Board.Instance.selectedPiece;
        AffectedKingRook affectedKing = new AffectedKingRook();
        affectedKing.from = king.Tile;
        king.Tile.Content = null;
        affectedKing.piece = king;

        Piece rook = Board.Instance.Tiles[Board.Instance.selectedMove.pos].Content;
        rook.Tile.Content = null;
        AffectedKingRook affectedRook = new AffectedKingRook();
        affectedRook.from = rook.Tile;
        affectedRook.piece = rook;

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
        affectedKing.to = king.Tile;
        changes.Add(affectedKing);
        rook.Tile.Content = rook;
        affectedRook.to = rook.Tile;
        changes.Add(affectedRook);

        king.wasMoved = true;
        rook.wasMoved = true;

        if (skipMovements)
        {
            tcs.SetResult(true);
        }
        else
        {

            LeanTween.move(king.gameObject, new Vector3(king.Tile.Position.x, king.Tile.Position.y, 0), 1.5f).
               setOnComplete(() =>
               {
                   tcs.SetResult(true);
               });
            LeanTween.move(rook.gameObject, new Vector3(rook.Tile.Position.x, rook.Tile.Position.y, 0), 1.4f);

        }



    }



    void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.Instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, 1) :
            new Vector2Int(0, -1);

        enPassantFlag = new AvailableMove(pawn.Tile.Position + direction, MoveType.EnPassant);
        NormalMove(tcs, skipMovements);
    }

    void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        Piece pawn = Board.Instance.selectedPiece;
        Vector2Int direction = pawn.maxTeam ?
            new Vector2Int(0, -1) :
            new Vector2Int(0, 1);


        Tile enemy = Board.Instance.Tiles[Board.Instance.selectedMove.pos + direction];
        AffectedPiece affectedEnemy = new AffectedPiece();
        affectedEnemy.from = affectedEnemy.to = enemy;
        affectedEnemy.piece = enemy.Content;
        changes.Add(affectedEnemy);
        enemy.Content.gameObject.SetActive(false);
        enemy.Content = null;
        NormalMove(tcs, skipMovements);
    }

    async void Promotion(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        TaskCompletionSource<bool> movementTCS = new TaskCompletionSource<bool>();
        NormalMove(movementTCS, skipMovements);
        await movementTCS.Task;
        //Debug.Log("Promoção do Peão");
        Pawn pawn =  Board.Instance.selectedPiece as Pawn;

        if (!skipMovements)
        {
            StateMachineController.Instance.TaskHold = new TaskCompletionSource<object>();
            StateMachineController.Instance.PromotionPanel.SetActive(true);

            await StateMachineController.Instance.TaskHold.Task;

            string result = StateMachineController.Instance.TaskHold.Task.Result as string;
            if (result == "Knight")
            {
                Board.Instance.selectedPiece.Movement = pawn.knightMovement;
            }
            else
            {
                Board.Instance.selectedPiece.Movement = pawn.queenMovement;
            }
            StateMachineController.Instance.PromotionPanel.SetActive(false);
        }else{


            AffectedPawn affectedPawn= new AffectedPawn();
            affectedPawn.wasMoved = true;
            affectedPawn.resetMovement = true;
            affectedPawn.from = changes[0].from;
            affectedPawn.to = changes[0].to;
            affectedPawn.piece = pawn;
            changes[0] = affectedPawn;
            pawn.Movement = pawn.queenMovement;
        }




        tcs.SetResult(true);
    }
}
