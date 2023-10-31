using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PieceMovementState : State
{
    public static List<AffectedPiece> changes;
    public static AvailableMove enPassantFlag;

    public override async void Enter()
    {
        Debug.Log("PieceMovementState:");
        var tcs = new TaskCompletionSource<bool>();
        MovePiece(tcs, false, Board.instance.selectedMove.moveType);

        await tcs.Task;
        machine.ChangeTo<TurnEndState>();
    }

    public static void MovePiece(TaskCompletionSource<bool> tcs, bool skipMovements, MoveType moveType)
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
            default:
                throw new ArgumentOutOfRangeException(nameof(moveType), moveType, null);
        }
    }

    private static void NormalMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var piece = Board.instance.selectedPiece;
        var pieceMoving = piece.CreateAffected();
        pieceMoving.piece = piece;
        pieceMoving.from = piece.tile;
        pieceMoving.to = Board.instance.tiles[Board.instance.selectedMove.pos];
        changes.Insert(0, pieceMoving);

        piece.tile.content = null;
        piece.tile = pieceMoving.to;

        if (piece.tile.content != null)
        {
            var deadPiece = piece.tile.content;
            var pieceKilled = new AffectedEnemy
            {
                piece = deadPiece
            };
            pieceKilled.to = pieceKilled.from = piece.tile;
            changes.Add(pieceKilled);
            deadPiece.gameObject.SetActive(false);
            pieceKilled.index = deadPiece.team.IndexOf(deadPiece);
            deadPiece.team.RemoveAt(pieceKilled.index);
        }

        piece.tile.content = piece;
        piece.wasMoved = true;

        if (skipMovements)
        {
            piece.wasMoved = true;
            tcs.SetResult(true);
        }
        else
        {
            var v3Pos = new Vector3(Board.instance.selectedMove.pos.x, Board.instance.selectedMove.pos.y, 0);
            var timing = Vector3.Distance(piece.transform.position, v3Pos) * 0.5f;

            LeanTween.move(piece.gameObject, v3Pos, timing)
                .setOnComplete(() => { tcs.SetResult(true); });
        }
    }

    private static void Castling(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var king = Board.instance.selectedPiece;
        var affectedKing = new AffectedKingRook
        {
            from = king.tile
        };
        king.tile.content = null;
        affectedKing.piece = king;

        var rook = Board.instance.tiles[Board.instance.selectedMove.pos].content;
        var affectedRook = new AffectedKingRook
        {
            from = rook.tile
        };
        rook.tile.content = null;
        affectedRook.piece = rook;

        var direction = rook.tile.position - king.tile.position;
        if (direction.x > 0)
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.position.x + 2, king.tile.position.y)];
            rook.tile = Board.instance.tiles[new Vector2Int(king.tile.position.x - 1, king.tile.position.y)];
        }
        else
        {
            king.tile = Board.instance.tiles[new Vector2Int(king.tile.position.x - 2, king.tile.position.y)];
            rook.tile = Board.instance.tiles[new Vector2Int(king.tile.position.x + 1, king.tile.position.y)];
        }

        king.tile.content = king;
        affectedKing.to = king.tile;
        changes.Add(affectedKing);
        rook.tile.content = rook;
        affectedRook.to = rook.tile;
        changes.Add(affectedRook);

        king.wasMoved = true;
        rook.wasMoved = true;

        if (skipMovements)
        {
            tcs.SetResult(true);
        }
        else
        {
            LeanTween.move(king.gameObject, new Vector3(king.tile.position.x, king.tile.position.y, 0), 1.5f)
                .setOnComplete(() => { tcs.SetResult(true); });
            LeanTween.move(rook.gameObject, new Vector3(rook.tile.position.x, rook.tile.position.y, 0), 1.4f);
        }
    }

    private static void PawnDoubleMove(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var pawn = Board.instance.selectedPiece;
        var direction = pawn.maxTeam ? new Vector2Int(0, 1) : new Vector2Int(0, -1);

        enPassantFlag = new AvailableMove(pawn.tile.position + direction, MoveType.EnPassant);
        NormalMove(tcs, skipMovements);
    }

    private static void EnPassant(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var pawn = Board.instance.selectedPiece;
        var direction = pawn.maxTeam ? new Vector2Int(0, -1) : new Vector2Int(0, 1);

        var enemy = Board.instance.tiles[Board.instance.selectedMove.pos + direction];
        var affectedEnemy = new AffectedEnemy();
        affectedEnemy.from = affectedEnemy.to = enemy;
        affectedEnemy.piece = enemy.content;
        affectedEnemy.index = affectedEnemy.piece.team.IndexOf(affectedEnemy.piece);
        affectedEnemy.piece.team.RemoveAt(affectedEnemy.index);
        changes.Add(affectedEnemy);
        enemy.content.gameObject.SetActive(false);
        enemy.content = null;

        NormalMove(tcs, skipMovements);
    }

    private static async void Promotion(TaskCompletionSource<bool> tcs, bool skipMovements)
    {
        var movementTcs = new TaskCompletionSource<bool>();
        NormalMove(movementTcs, skipMovements);
        await movementTcs.Task;
        //Debug.Log("promoveu");
        var pawn = Board.instance.selectedPiece as Pawn;

        if (!skipMovements)
        {
            StateMachineController.instance.taskHold = new TaskCompletionSource<object>();
            StateMachineController.instance.promotionPanel.SetActive(true);

            await StateMachineController.instance.taskHold.Task;

            var result = StateMachineController.instance.taskHold.Task.Result as string;
            Board.instance.selectedPiece.movement = result == "Knight" ? pawn!.knightMovement : pawn!.queenMovement;

            StateMachineController.instance.promotionPanel.SetActive(false);
        }
        else
        {
            var affectedPawn = new AffectedPawn
            {
                wasMoved = true,
                resetMovement = true,
                from = changes[0].from,
                to = changes[0].to,
                piece = pawn
            };

            changes[0] = affectedPawn;
            Debug.Log(pawn!.queenMovement);
            pawn.movement = pawn.queenMovement;
        }

        tcs.SetResult(true);
    }
}