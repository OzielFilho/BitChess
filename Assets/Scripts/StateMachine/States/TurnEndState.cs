using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
    private AudioController audioController;
    public override async void Enter()
    {
        Debug.Log("Turn end:");
        var gameFinished = CheckConditions();
        await Task.Delay(100);
        if (gameFinished)
            machine.ChangeTo<GameEndState>();
        else
            machine.ChangeTo<TurnBeginState>();
    }

    private bool CheckTeams()
    {
        audioController = GetComponent<AudioController>();
        var bluePiece = Board.instance.bluePieces.Find(p => p.gameObject.activeSelf);
        if (bluePiece == null)
        {
            audioController.Play(this);
            Debug.Log("White team wins");
            return true;
        }

        var whitePiece = Board.instance.whitePieces.Find(p => p.gameObject.activeSelf);

        if (whitePiece == null)
        {
            audioController.Play(this);
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }

    bool CheckConditions(){
    if(CheckTeams() || CheckKing()){
        return true;
    }
    return false;
    }


    private bool CheckKing()
    {
        audioController = GetComponent<AudioController>();
        
      King king = Board.instance.blueHolder.GetComponentInChildren<King>();

      if(king == null)
      {
            audioController.Play(this);
            Debug.Log("White team wins");
            return true;
        }
      king = Board.instance.whiteHolder.GetComponentInChildren<King>();

     if (king == null)
        {
            audioController.Play(this);
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }
}
