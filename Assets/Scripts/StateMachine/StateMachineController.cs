using UnityEngine;
using System.Threading.Tasks;
public class StateMachineController : MonoBehaviour
{
    public static StateMachineController instance;
    public Player player1;
    public Player player2;
    public Player currentlyPlayer;
    public TaskCompletionSource<object> taskHold;
    public GameObject promotionPanel;
    private State current;
    private bool isBusy;

    private AudioController audioController;

    [SerializeField] private GameObject options;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            ActionOptionsMenu();
        }
    }

    public void ActionOptionsMenu()
    {
        options.SetActive(!options.activeSelf);
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ChangeTo<LoadState>();
    }

    public void ChangeTo<T>() where T : State
    {
        State state = GetState<T>();
        if (current != state)
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
        if (isBusy)
            return;
        isBusy = true;

        if (current != null) current.Exit();

        current = state;
        if (current != null)
            current.Enter();

        isBusy = false;
    }
}
