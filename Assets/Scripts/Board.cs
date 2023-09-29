using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public delegate void TileClickedEvent(object sender, object args);

public class Board : MonoBehaviour
{
    public static Board Instance;
    public Dictionary<Vector2Int, Tile> Tiles = new();
    public TileClickedEvent TileClicked = delegate { };

    public List<Piece> bluePieces = new();
    public List<Piece> whitePieces = new();
    public Piece selectedPiece;
    public HighlightClick selectedHighlight;

    private Transform BlueHolder => StateMachineController.Instance.player1.transform;
    private Transform WhiteHolder => StateMachineController.Instance.player2.transform;

    private AudioController audioController;

    void Awake()
    {
        Instance = this;
        audioController = GetComponent<AudioController>();
        audioController.Play(this);
    }

    public async Task Load()
    {
        GetTeams();

        await Task.Run(CreateBoard);
    }

    private void GetTeams()
    {
        bluePieces.AddRange(BlueHolder.GetComponentsInChildren<Piece>());
        whitePieces.AddRange(WhiteHolder.GetComponentsInChildren<Piece>());
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
        for (var i = 0; i < 8; i++)
        {
            for (var j = 0; j < 8; j++)
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
