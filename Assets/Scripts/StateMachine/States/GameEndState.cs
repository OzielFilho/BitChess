using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : State
{
    public override void Enter()
    {
        Debug.Log("Acabou o jogo");
    }
}
