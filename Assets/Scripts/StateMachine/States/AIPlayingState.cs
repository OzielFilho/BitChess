using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayingState : State
{
  
public async override void Enter()
{
        
   Task<Ply> task = AIController.instance.CalculatePlays();
        await task;
        Ply bestResult = task.Result;
        MakeBestPlay(bestResult);
}
    async void MakeBestPlay(Ply ply){
        Ply currentPly = ply;

        for(int i=1; i < AIController.instance.objectivePlyDepth; i++){
            currentPly = currentPly.originPly;
        }
        Board.Instance.selectedPiece = currentPly.changes[0].piece;
        Debug.Log(currentPly.changes[0].piece.name);
        Board.Instance.selectedMove = GetMoveType(currentPly);
        Debug.Log(Board.Instance.selectedMove);
        await Task.Delay(100);
        Machine.ChangeTo<PieceMovementState>();
    }
    AvailableMove GetMoveType(Ply ply){
        List<AvailableMove> moves = Board.Instance.selectedPiece.Movement.GetValidMoves();
        foreach(AvailableMove m in moves){
            if(m.pos == ply.changes[0].to.Position)
                return m;
        }
        return new AvailableMove();
    }

}
