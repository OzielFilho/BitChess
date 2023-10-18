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
    [ContextMenu("Calculate Plays")]
   
   public async void CalculatePlays(){
    currentState = CreateSnapShot();
    EvaluateBoard(currentState);
   }

    Ply CreateSnapShot(){
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

        return ply;
    }

    PieceEvaluation CreateEvaluationPiece(Piece piece, Ply ply)
    {
        PieceEvaluation eva = new PieceEvaluation();
        eva.piece = piece;
        return eva;
    }
    
    public void EvaluateBoard(Ply ply){
      

        foreach(PieceEvaluation piece in ply.whites)
        {
            EvaluatePiece(piece, ply, 1);
        }

        foreach(PieceEvaluation piece in ply.blues)
        {
            EvaluatePiece(piece, ply, -1);
        }

        Debug.Log("Board Score" + ply.score);
    }

    void EvaluatePiece(PieceEvaluation eva, Ply ply, int scoreDirection ){
        Board.Instance.selectedPiece = eva.piece;
       eva.availableMoves  = eva.piece.Movement.GetValidMoves();
        

        eva.score = eva.piece.Movement.value;
        ply.score += eva.score* scoreDirection;
    }
}
