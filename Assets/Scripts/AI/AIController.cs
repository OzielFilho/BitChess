using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class AIController : MonoBehaviour
{
    public static AIController instance;
    public HighlightClick aIhighlight;
    public int objectivePlyDepth = 2;
    public readonly PieceSquareTable squareTable = new();

    private AvailableMove enPassantFlagSaved;
    private int calculationCount;
    private float lastInterval;
    private Ply maxPly;
    private Ply minPly;

    void Awake()
    {
        instance = this;
        maxPly = new Ply
        {
            score = 999999
        };
        minPly = new Ply
        {
            score = -999999
        };
        squareTable.SetDictionaries();
    }

    [ContextMenu("Calculate Plays")]
    public async Task<Ply> CalculatePlays()
    {
        lastInterval = Time.realtimeSinceStartup;
        int minimaxDirection;
        if (StateMachineController.instance.currentlyPlayer == StateMachineController.instance.player1)
            minimaxDirection = 1;
        else
            minimaxDirection = -1;

        enPassantFlagSaved = PieceMovementState.enPassantFlag;
        var currentPly = new Ply();
        calculationCount = 0;

        currentPly.originPly = null;
        var currentPlyDepth = 0;
        currentPly.changes = new List<AffectedPiece>();

        Debug.Log("Começo");
        var calculation = CalculatePly(currentPly,
            -1000000, 1000000,
            currentPlyDepth,
            minimaxDirection);
        await calculation;
        currentPly.bestFuture = calculation.Result;

        Debug.Log("Calculations: " + calculationCount);
        Debug.Log("Time: " + (Time.realtimeSinceStartup - lastInterval));
        PrintBestPly(currentPly.bestFuture);
        PieceMovementState.enPassantFlag = enPassantFlagSaved;
        return currentPly.bestFuture;
    }

    private async Task<Ply> CalculatePly(Ply parentPly, int alpha, int beta, int currentPlyDepth, int minimaxDirection)
    {
        currentPlyDepth++;
        if (currentPlyDepth > objectivePlyDepth)
        {
            EvaluateBoard(parentPly);
            //Task evaluationTask = Task.Run(()=> EvaluateBoard(parentPly));
            //await evaluationTask;
            return parentPly;
        }

        List<Piece> team;
        if (minimaxDirection == 1)
        {
            team = Board.instance.bluePieces;
            parentPly.bestFuture = minPly;
        }
        else
        {
            team = Board.instance.whitePieces;
            parentPly.bestFuture = maxPly;
        }

        for (var index = 0; index < team.Count; index++)
        {
            var t = team[index];
            Board.instance.selectedPiece = t;
            foreach (var move in t.movement.GetValidMoves())
            {
                calculationCount++;
                //Debug.Log("aa");
                Board.instance.selectedPiece = t;
                Board.instance.selectedMove = move;
                var tcs = new TaskCompletionSource<bool>();
                PieceMovementState.MovePiece(tcs, true, move.moveType);

                await tcs.Task;
                var newPly = new Ply
                {
                    changes = PieceMovementState.changes,
                    enPassantFlag = PieceMovementState.enPassantFlag
                };

                var calculation = CalculatePly(newPly,
                    alpha, beta,
                    currentPlyDepth, minimaxDirection * -1);
                await calculation;

                parentPly.bestFuture = IsBest(parentPly.bestFuture, minimaxDirection, calculation.Result,
                    ref alpha, ref beta);
                newPly.originPly = parentPly;

                PieceMovementState.enPassantFlag = parentPly.enPassantFlag;
                ResetBoardBackwards(newPly);
                if (beta <= alpha)
                {
                    return parentPly.bestFuture;
                }
            }
        }

        return parentPly.bestFuture;
    }

    private Ply IsBest(Ply ply, int minimaxDirection, Ply potentialBest, ref int alpha, ref int beta)
    {
        var best = ply;
        if (minimaxDirection == 1)
        {
            if (potentialBest.score > ply.score)
                best = potentialBest;
            alpha = Mathf.Max(alpha, best.score);
        }
        else
        {
            if (potentialBest.score < ply.score)
                best = potentialBest;
            beta = Mathf.Min(beta, best.score);
        }

        return best;
    }

    private void EvaluateBoard(Ply ply)
    {
        foreach (var piece in Board.instance.bluePieces)
        {
            EvaluatePiece(piece, ply, 1);
        }

        foreach (var piece in Board.instance.whitePieces)
        {
            EvaluatePiece(piece, ply, -1);
        }
        //Debug.Log("Board score: "+ply.score);
    }

    private void EvaluatePiece(Piece eva, Ply ply, int scoreDirection)
    {
        var positionValue = eva.movement.positionValue[eva.tile.position];
        ply.score += (eva.movement.value + positionValue) * scoreDirection;
    }

    private void ResetBoardBackwards(Ply ply)
    {
        foreach (var p in ply.changes)
        {
            p.Undo();
        }
    }

    private void PrintBestPly(Ply finalPly)
    {
        var currentPly = finalPly;
        Debug.Log("Melhor jogada:");
        while (currentPly.originPly != null)
        {
            Debug.LogFormat("{0}-{1}->{2}",
                currentPly.changes[0].piece.transform.parent.name,
                currentPly.changes[0].piece.name,
                currentPly.changes[0].to.position);

            currentPly = currentPly.originPly;
        }
    }
}