using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    void Awake()
    {
        Movement = new KingMovement();
    }
}
