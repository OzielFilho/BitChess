using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile Tile;
    [HideInInspector]
    public Movement Movement;

    public bool wasMoved;
    
    void OnMouseDown()
    {
        Board.Instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }

    protected void Start()
    {
        Board.Instance.AddPiece(transform.parent.name, this);
    }
}
