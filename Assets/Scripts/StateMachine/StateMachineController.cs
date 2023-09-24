using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineController : MonoBehaviour
{
    public static StateMachineController Instance;
    public Player player1;
    public Player player2;
    public Player currentlyPlayer;
    private State _current;
    public bool IsBusy { get; private set; }
    
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ChangeTo<LoadState>();
    }

    public void ChangeTo<T>() where T : State
    {
        State state = GetState<T>();
        if (_current != state)
        {
            ChangeState(state);
        }
    }

    private T GetState<T>() where T : State
    {
        var target = GetComponent<T>() ?? gameObject.AddComponent<T>();

        return target;
    }
    
    private void ChangeState(State state)
    {
        if (IsBusy)
            return;
        IsBusy = true;
        
        if(_current != null) _current.Exit();

        _current = state;
        if(_current != null)
            _current.Enter();

        IsBusy = false;
    }
}
