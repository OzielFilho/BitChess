using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AIPlayingState : State
{
    public override async void Enter()
    {
        var bestResult = await AIController.instance.CalculatePlays();
        MakeBestPlay(bestResult);
    }

    async void MakeBestPlay(Ply ply)
    {
        var currentPly = ply;

        for (var i = 1; i < AIController.instance.objectivePlyDepth; i++)
        {
            currentPly = currentPly.originPly;
        }

        Board.instance.selectedPiece = currentPly.changes[0].piece;
        Debug.Log(currentPly.changes[0].piece.name);
        Board.instance.selectedMove = GetMoveType(currentPly);
        Debug.Log(Board.instance.selectedMove);
        await Task.Delay(100);
        machine.ChangeTo<PieceMovementState>();
    }

    AvailableMove GetMoveType(Ply ply)
    {
        var moves = Board.instance.selectedPiece.movement.GetValidMoves();
        foreach (var m in moves)
        {
            if (m.pos == ply.changes[0].to.position)
                return m;
        }

        return new AvailableMove();
    }
}