using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [HideInInspector] public Movement movement;
    public Tile tile;
    public bool wasMoved;
    public bool maxTeam;
    public List<Piece> team;

    protected virtual void Start()
    {
        if (transform.parent.name == "BluePieces")
        {
            team = Board.instance.bluePieces;
            maxTeam = true;
        }
        else
        {
            team = Board.instance.whitePieces;
        }
    }

    public virtual AffectedPiece CreateAffected()
    {
        return new AffectedPiece();
    }

    void OnMouseDown()
    {
        InputController.instance.tileClicked(this, transform.parent.GetComponent<Player>());
    }
}