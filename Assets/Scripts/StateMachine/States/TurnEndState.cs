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
            Machine.ChangeTo<GameEndState>();
        else
            Machine.ChangeTo<TurnBeginState>();
    }

    private bool CheckTeams()
    {
        audioController = GetComponent<AudioController>();
        var bluePiece = Board.Instance.bluePieces.Find(p => p.gameObject.activeSelf);
        if (bluePiece == null)
        {
            audioController.Play(this);
            Debug.Log("White team wins");
            return true;
        }

        var whitePiece = Board.Instance.whitePieces.Find(p => p.gameObject.activeSelf);

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
        
      King king = Board.Instance.BlueHolder.GetComponentInChildren<King>();

      if(king == null)
      {
            audioController.Play(this);
            Debug.Log("White team wins");
            return true;
        }
      king = Board.Instance.WhiteHolder.GetComponentInChildren<King>();

     if (king == null)
        {
            audioController.Play(this);
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }
}
