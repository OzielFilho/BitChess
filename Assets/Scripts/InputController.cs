using UnityEngine;

public delegate void TileClickedEvent(object sender, object args);

public class InputController : MonoBehaviour
{
    public static InputController instance;
    public TileClickedEvent tileClicked = delegate { };
    public TileClickedEvent returnClicked = delegate { };

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            returnClicked(null, null);
        }
    }

    public void Promotion(string piece)
    {
        StateMachineController.instance.taskHold.SetResult(piece);
    }
}