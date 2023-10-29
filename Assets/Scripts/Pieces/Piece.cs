using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    
    [HideInInspector]
    public Movement Movement;

    public Tile Tile;

    public bool wasMoved;

    public bool maxTeam;

    virtual protected void Start()
    {
        if(transform.parent.name == "BluePieces")
        {
            maxTeam = true;
        }
    }

    public virtual AffectedPiece CreateAffected()
    {
        return new AffectedPiece();
    }
    
    void OnMouseDown()
    {
        Board.Instance.TileClicked(this, transform.parent.GetComponent<Player>());
    }

}
