using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public delegate void TileClickedEvent(object sender, object args);

public class Board : MonoBehaviour
{
    public static Board Instance;
    public Dictionary<Vector2Int, Tile> Tiles = new ();
    public Transform BlueHolder => StateMachineController.Instance.player1.transform;
    public Transform WhiteHolder => StateMachineController.Instance.player2.transform;
    public List<Piece> BluePieces = new ();
    public List<Piece> WhitePieces = new ();
    public TileClickedEvent TileClicked = delegate { };
    public Piece SelectedPiece;
    public HighlightClick SelectedHighlight;

    void Awake()
    {
        Instance = this;
    }

    public async Task Load()
    {
        GetTeams();

        await Task.Run(CreateBoard);
    }

    private void GetTeams()
    {
        BluePieces.AddRange(BlueHolder.GetComponentsInChildren<Piece>());
        WhitePieces.AddRange(WhiteHolder.GetComponentsInChildren<Piece>());
    }

    public void AddPiece(string team, Piece piece)
    {
        var piecePosition = piece.transform.position;
        var position = new Vector2Int((int)piecePosition.x, (int)piecePosition.y);
        piece.Tile = Tiles[position];
        piece.Tile.Content = piece;
    }

    public void CreateBoard()
    {
        for(var i = 0; i<8; i++)
        {
            for(var j = 0; j<8; j++)
            {
                CreateTile(i, j);
            }
        }
    }
    
    void CreateTile(int i, int j)
    {
        var position = new Vector2Int(i, j);
        var tile = new Tile(position);
        Tiles.Add(tile.Position, tile);
    }
}
