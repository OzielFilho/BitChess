using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightClick : MonoBehaviour
{
    public Tile Tile;
    void OnMouseDown()
    {
        Board.Instance.TileClicked(this, null);
    }
}
