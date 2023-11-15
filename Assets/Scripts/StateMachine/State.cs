using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachineController machine => StateMachineController.instance;

    public virtual void Enter()
    {
    }
    
    public virtual void Exit()
    {
    }
    
    protected void SetColliders(bool state)
    {
        foreach (var b in machine.currentlyPlayer.GetComponentsInChildren<BoxCollider2D>())
        {
            b.enabled = state;
        }
    }
}
