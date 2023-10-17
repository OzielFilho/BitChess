using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ply 
{
    public List<PieceEvaluation> whites;

    public List<PieceEvaluation> blues;


    public float score;

    
    public String name;

    List<AffectedPiece> changes;

    //public MoveType moveType;

    public Ply originPly;

    public List<Ply> futurePlies;
}

