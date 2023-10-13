using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public static AIController instance;

    public Ply currentState;
    
    void Awake()
    {
        instance = this;
    }
    [ContextMenu("Create Evaluations")]

    public void CreateEvaluations(){
        Ply ply = new Ply();
        ply.whites = new List<PieceEvaluation>();
        ply.blues = new List<PieceEvaluation>();

        foreach(Piece p in Board.Instance.whitePieces){
            if(p.gameObject.activeSelf)
            {
                ply.whites.Add(CreateEvaluationPiece(p, ply));
            }
        }
         foreach(Piece p in Board.Instance.bluePieces){
            if(p.gameObject.activeSelf)
            {
                ply.blues.Add(CreateEvaluationPiece(p, ply));
            }
        }

        currentState = ply;
    }

    PieceEvaluation CreateEvaluationPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
        return eva;
    }
    [ContextMenu("Evaluate")]
    public void EvaluateBoard(){
        Ply ply = currentState;

        foreach(PieceEvaluation piece in ply.whites)
        {
            EvaluatePiece(piece, ply, 1);
        }

        foreach(PieceEvaluation piece in ply.blues)
        {
            EvaluatePiece(piece, ply, -1);
        }

        Debug.Log(ply.score);
    }

    void EvaluatePiece(PieceEvaluation eva, Ply ply, int scoreDirection ){
        Board.Instance.selectedPiece = eva.piece;
        List<Tile> tiles = eva.piece.Movement.GetValidMoves();
        eva.availableMoves = tiles.Count;

        eva.score = eva.piece.Movement.value;
        ply.score += eva.score* scoreDirection;
    }
}
