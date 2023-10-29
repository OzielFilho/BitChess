using System.Threading.Tasks;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn begin:");
        machine.currentlyPlayer = machine.currentlyPlayer == machine.player1 ? machine.player2 : machine.player1;

        Debug.Log(machine.currentlyPlayer + " now playing");
        await Task.Delay(100);

        if(machine.currentlyPlayer.aiControlled)
        {
            machine.ChangeTo<AIPlayingState>();
        }
        else{
            machine.ChangeTo<PieceSelectionState>();
        }
        
    }
}
