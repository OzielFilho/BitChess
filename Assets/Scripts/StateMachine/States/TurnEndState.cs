using System.Threading.Tasks;
using UnityEngine;

public class TurnEndState : State
{
    private AudioController audioController;
    private MenuController menuController;

    private AudioSource audioSourceChessboard;

    private GameObject chessboard;
    void Start()
    {
        GameObject menuControllerFind = GameObject.Find("MenuController");
        audioController = GetComponent<AudioController>();
        if (menuControllerFind != null)
            menuController = menuControllerFind.GetComponent<MenuController>();
        GameObject outroObjeto = GameObject.Find("Chessboard");
        if (outroObjeto != null)
            audioSourceChessboard = outroObjeto.GetComponent<AudioSource>();
    }
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
            audioSourceChessboard.Stop();
            audioController.PlayLoseSoundGame();
            menuController.LoseGame();
            Debug.Log("White team wins");
            return true;
        }

        var whitePiece = Board.instance.whitePieces.Find(p => p.gameObject.activeSelf);

        if (whitePiece == null)
        {
            audioSourceChessboard.Stop();
            audioController.PlayWinSoundGame();
            menuController.WinGame();
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }

    bool CheckConditions()
    {
        if (CheckTeams() || CheckKing())
        {
            return true;
        }
        return false;
    }


    private bool CheckKing()
    {
        King king = Board.instance.blueHolder.GetComponentInChildren<King>();
        if (king == null)
        {
            audioSourceChessboard.Stop();
            audioController.PlayLoseSoundGame();
            menuController.LoseGame();
            Debug.Log("White team wins");
            return true;
        }
        king = Board.instance.whiteHolder.GetComponentInChildren<King>();

        if (king == null)
        {
            audioSourceChessboard.Stop();
            audioController.PlayWinSoundGame();
            menuController.WinGame();
            Debug.Log("Blue team wins");
            return true;
        }

        return false;
    }
}
