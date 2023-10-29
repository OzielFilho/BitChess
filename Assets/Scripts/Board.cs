using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Board instance;
    public Dictionary<Vector2Int, Tile> tiles = new();
    public List<Sprite> spritesBoardList;
    public List<Piece> bluePieces = new();
    public List<Piece> whitePieces = new();
    public Piece selectedPiece;
    public AvailableMove selectedMove;
    public Transform blueHolder => StateMachineController.instance.player1.transform;
    public Transform whiteHolder => StateMachineController.instance.player2.transform;
    private AudioController audioController;

    void Awake()
    {
        instance = this;
        SelectBoardSprite();
        audioController = GetComponent<AudioController>();
        audioController.Play(this);
    }

    private void SelectBoardSprite()
    {
        var boardSpriteRenderer = GetComponent<SpriteRenderer>();
        var prefabIndex = PlayerPrefs.GetInt("SelectedPrefabBoard", 0);
        boardSpriteRenderer.sprite = spritesBoardList[prefabIndex];
    }

    public async Task Load()
    {
        GetTeams();
        await Task.Run(CreateBoard);
    }
    
    [ContextMenu("Reset Board")]
    public void ResetBoard(){
        foreach(var t in tiles.Values){
            t.content = null;
        }
        foreach(var p in bluePieces){
            ResetPiece(p);
        }
        foreach(var p in whitePieces){
            ResetPiece(p);
        }
    }
    
    void ResetPiece(Piece piece){
        if(!piece.gameObject.activeSelf)
            return;

        var piecePosition = piece.transform.position;
        var pos = new Vector2Int((int)piecePosition.x, (int)piecePosition.y);
        tiles.TryGetValue(pos, out piece.tile);
        piece.tile!.content = piece;
    }

    private void GetTeams()
    {
        bluePieces.AddRange(blueHolder.GetComponentsInChildren<Piece>());
        whitePieces.AddRange(whiteHolder.GetComponentsInChildren<Piece>());
    }

    public void AddPiece(string team, Piece piece)
    {
        var piecePosition = piece.transform.position;
        var position = new Vector2Int((int)piecePosition.x, (int)piecePosition.y);
        piece.tile = tiles[position];
        piece.tile.content = piece;
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
        tiles.Add(tile.position, tile);
    }

    public void Promotion(string piece)
    {
        StateMachineController.instance.taskHold.SetResult(piece);
    }
}




