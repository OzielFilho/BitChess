using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public Tile Tile; 
    
    void OnMouseDown()
    {
        Debug.Log("clicked");
    }

    protected void Start()
    {
        Board.Instance.AddPiece(transform.parent.name, this);
    }
}
