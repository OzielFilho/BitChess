using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    protected StateMachineController Machine
    {
        get { return StateMachineController.Instance; }
    }
    
    public virtual void Enter()
    {
        
    }
    
    public virtual void Exit()
    {
        
    }
}
