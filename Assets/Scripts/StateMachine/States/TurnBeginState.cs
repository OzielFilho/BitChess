using System.Threading.Tasks;
using UnityEngine;

public class TurnBeginState : State
{
    public override async void Enter()
    {
        Debug.Log("Turn begin:");
        Machine.currentlyPlayer = Machine.currentlyPlayer == Machine.player1 ? Machine.player2 : Machine.player1;

        Debug.Log(Machine.currentlyPlayer + " now playing");
        await Task.Delay(100);

        if(Machine.currentlyPlayer.AIControlled)
        {
            Machine.ChangeTo<AIPlayingState>();
        }
        else{
            Machine.ChangeTo<PieceSelectionState>();
        }
        
    }
}
