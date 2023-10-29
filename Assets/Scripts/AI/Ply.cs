
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ply
{
    public int score;
    public List<AffectedPiece> changes;
    public Ply originPly;
    public AvailableMove enPassantFlag;
    public Ply bestFuture;
}


