using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn end:");
        var gameFinished = CheckTeams();
        await Task.Delay(100);
        if(gameFinished)
            Machine.ChangeTo<GameEndState>();
        else
            Machine.ChangeTo<TurnBeginState>();
    }

    private bool CheckTeams()
    {
        var bluePiece = Board.Instance.BluePieces.Find(p => p.gameObject.activeSelf);
        if(bluePiece == null)
        {
            Debug.Log("White team wins");
            return true;
        }
        
        var whitePiece = Board.Instance.WhitePieces.Find(p => p.gameObject.activeSelf);
        if(whitePiece == null)
        {
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }
}
