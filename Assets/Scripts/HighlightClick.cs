using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public AvailableMove move;
    
    void OnMouseDown()
    {
        InputController.instance.tileClicked(this, null);
    }
}
