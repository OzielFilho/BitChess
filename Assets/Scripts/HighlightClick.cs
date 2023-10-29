using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public AvailableMove move;
    
    void OnMouseDown()
    {
        Board.Instance.TileClicked(this, null);
    }
}
