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
}
