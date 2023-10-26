
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ply
{
    public List<PieceEvaluation> whites;

    public List<PieceEvaluation> blues;


    public int score;

    public List<AffectedPiece> changes;

    public Ply originPly;

    public List<Ply> futurePlies;
    
    public Ply bestFuture;


    public AvailableMove enPassantFlag;
}


