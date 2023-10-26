using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;

    public HighlightClick AIhighlight;

    public Ply currenState;
    int calculationCount;

    public int objectivePlyDepth = 2;

    float lastInterval;

    public AvailableMove enPassantFlagSaved;

    Ply maxPly;

    Ply minPly;

    void Awake()
    {
        instance = this;
        maxPly = new Ply();
        maxPly.score = 999999;
        minPly = new Ply();
        minPly.score = -999999;
    }
    [ContextMenu("Calculate Plays")]

    public async Task<Ply> CalculatePlays()
    {

        
        lastInterval = Time.realtimeSinceStartup;
        int minimaxDirection;
        if (StateMachineController.Instance.currentlyPlayer == StateMachineController.Instance.player1)
        {
            minimaxDirection = 1;
        }
        else
        {
            minimaxDirection = -1;
        }

        enPassantFlagSaved = PieceMovementState.enPassantFlag;
        currenState = CreateSnapShot();
        calculationCount = 0;


        Ply currentPly = currenState;
        currentPly.originPly = null;
        int currentPlyDepth = 0;
        currentPly.changes = new List<AffectedPiece>();

        Debug.Log("começo");
        Task<Ply> calculation = CalculatePly(currentPly, GetTeam(currentPly, minimaxDirection),
        currentPlyDepth,
        minimaxDirection);
        await calculation;
        currentPly.bestFuture = calculation.Result;
        Debug.Log("Calculations: " + calculationCount);
        Debug.Log("Time: " + (Time.realtimeSinceStartup + lastInterval));
        PrintBestPly(currentPly.bestFuture);
        PieceMovementState.enPassantFlag = enPassantFlagSaved;
        return currentPly.bestFuture;

    }

    async Task<Ply> CalculatePly(Ply parentPly, List<PieceEvaluation> team, int currentPlyDepth, int minimaxDirection)
    {
        parentPly.futurePlies = new List<Ply>();
        currentPlyDepth++;

        if (currentPlyDepth > objectivePlyDepth)
        {
            EvaluateBoard(parentPly);
            //    Task evaluationTask =  Task.Run(()=>EvaluateBoard(parentPly)); 
            //    await evaluationTask;
            return parentPly;
        }

        if (minimaxDirection == 1)
        {
            parentPly.bestFuture = minPly;
        }
        else
        {
            parentPly.bestFuture = maxPly;
        }
        foreach (PieceEvaluation eva in team)
        {


            foreach (AvailableMove move in eva.availableMoves)
            {
                calculationCount++;
                Board.Instance.selectedPiece = eva.piece;
                Board.Instance.selectedMove = move;
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                PieceMovementState pieceMovementState = new PieceMovementState();
                pieceMovementState.MovePiece(tcs, true, move.moveType);

                await tcs.Task;
                Ply newPly = CreateSnapShot(parentPly);
                newPly.changes = PieceMovementState.changes;
                newPly.enPassantFlag = PieceMovementState.enPassantFlag;

                Task<Ply> calculation = CalculatePly(newPly,
                GetTeam(newPly, minimaxDirection * -1),
                currentPlyDepth, minimaxDirection * -1);
                await calculation;

                parentPly.bestFuture = IsBest(parentPly.bestFuture, minimaxDirection, calculation.Result);
                newPly.originPly = parentPly;
                parentPly.futurePlies.Add(newPly);

                PieceMovementState.enPassantFlag = parentPly.enPassantFlag;
                ResetBoardBackwards(newPly);

            }
        }
        return parentPly.bestFuture;
    }
    List<PieceEvaluation> GetTeam(Ply ply, int minimaxDirection)
    {

        if (minimaxDirection == 1)
        {
            return ply.blues;
        }
        else
        {
            return ply.whites;
        }
    }

    Ply IsBest(Ply ply, int minimaxDirection, Ply potentialBest)
    {
        if (minimaxDirection == 1)
        {
            if (potentialBest.score > ply.score)
            {
                return potentialBest;
            }
            else
            {
                return ply;
            }
        }
        else
        {
            if (potentialBest.score < ply.score)
            {
                return potentialBest;
            }
            else
            {
                return ply;
            }
        }
    }
    Ply CreateSnapShot()
    {
        Ply ply = new Ply();
        ply.blues = new List<PieceEvaluation>();
        ply.whites = new List<PieceEvaluation>();

        foreach (Piece p in Board.Instance.bluePieces)
        {
            if (p.gameObject.activeSelf)
            {
                ply.blues.Add(CreateEvaluationPiece(p, ply));
            }
        }

        foreach (Piece p in Board.Instance.whitePieces)
        {
            if (p.gameObject.activeSelf)
            {
                ply.whites.Add(CreateEvaluationPiece(p, ply));
            }
        }


        return ply;
    }

    Ply CreateSnapShot(Ply parentPly)
    {
        Ply ply = new Ply();
        ply.blues = new List<PieceEvaluation>();
        ply.whites = new List<PieceEvaluation>();

        foreach (PieceEvaluation p in parentPly.blues)
        {
            if (p.piece.gameObject.activeSelf)
            {
                ply.blues.Add(CreateEvaluationPiece(p.piece, ply));
            }
        }

        foreach (PieceEvaluation p in parentPly.whites)
        {
            if (p.piece.gameObject.activeSelf)
            {
                ply.whites.Add(CreateEvaluationPiece(p.piece, ply));
            }
        }


        return ply;
    }

    PieceEvaluation CreateEvaluationPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
        Board.Instance.selectedPiece = eva.piece;
        eva.availableMoves = eva.piece.Movement.GetValidMoves();
        return eva;
    }

    public void EvaluateBoard(Ply ply)
    {


        foreach (PieceEvaluation piece in ply.blues)
        {
            EvaluatePiece(piece, ply, 1);
        }

        foreach (PieceEvaluation piece in ply.whites)
        {
            EvaluatePiece(piece, ply, -1);
        }



        Debug.Log("Board Score" + ply.score);
    }

    void EvaluatePiece(PieceEvaluation eva, Ply ply, int scoreDirection)
    {
        ply.score += eva.piece.Movement.value * scoreDirection;
    }
    void ResetBoardBackwards(Ply ply)
    {
        foreach (AffectedPiece p in ply.changes)
        {
            p.Undo();

        }

    }

    void PrintBestPly(Ply finalPly)
    {
        Ply currentPly = finalPly;
        Debug.Log("Melhor jogada: ");
        while (currentPly.originPly != null)
        {
            Debug.LogFormat("{0}-{1}->{2}",
            currentPly.changes[0].piece.transform.parent.name,
            currentPly.changes[0].piece.name,
            currentPly.changes[0].to.Position
            );

            currentPly = currentPly.originPly;
        }
    }
}
