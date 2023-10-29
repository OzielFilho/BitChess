using System.Collections.Generic;
using UnityEngine;

public class PieceSquareTable
{
    public Dictionary<Vector2Int, int> pawnGold = new();
    public Dictionary<Vector2Int, int> pawnGreen = new ();
    public Dictionary<Vector2Int, int> bishopGold = new ();
    public Dictionary<Vector2Int, int> bishopGreen = new ();
    public Dictionary<Vector2Int, int> kingGold = new ();
    public Dictionary<Vector2Int, int> kingGreen = new ();
    public Dictionary<Vector2Int, int> knightGold = new ();
    public Dictionary<Vector2Int, int> knightGreen = new ();
    public Dictionary<Vector2Int, int> queenGold = new ();
    public Dictionary<Vector2Int, int> queenGreen = new ();
    public Dictionary<Vector2Int, int> rookGold = new ();
    public Dictionary<Vector2Int, int> rookGreen = new ();

    public void SetDictionaries()
    {
        var pawnValues = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5, 5, 10, 25, 25, 10, 5, 5,
            0, 0, 0, 20, 20, 0, 0, 0,
            5, -5, -10, 0, 0, -10, -5, 5,
            5, 10, 10, -20, -20, 10, 10, 5,
            0, 0, 0, 0, 0, 0, 0, 0
        };
        SetDictionary(pawnGold, pawnGreen, pawnValues);
        var knightValues = new List<int>()
        {
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -30, 0, 10, 15, 15, 10, 0, -30,
            -30, 5, 15, 20, 20, 15, 5, -30,
            -30, 0, 15, 20, 20, 15, 0, -30,
            -30, 5, 10, 15, 15, 10, 5, -30,
            -40, -20, 0, 5, 5, 0, -20, -40,
            -50, -40, -30, -30, -30, -30, -40, -50
        };
        SetDictionary(knightGold, knightGreen, knightValues);
        var bishopValues = new List<int>()
        {
            -20, -10, -10, -10, -10, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 10, 10, 5, 0, -10,
            -10, 5, 5, 10, 10, 5, 5, -10,
            -10, 0, 10, 10, 10, 10, 0, -10,
            -10, 10, 10, 10, 10, 10, 10, -10,
            -10, 5, 0, 0, 0, 0, 5, -10,
            -20, -10, -10, -10, -10, -10, -10, -20
        };
        SetDictionary(bishopGold, bishopGreen, bishopValues);
        var kingValues = new List<int>()
        {
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -30, -40, -40, -50, -50, -40, -40, -30,
            -20, -30, -30, -40, -40, -30, -30, -20,
            -10, -20, -20, -20, -20, -20, -20, -10,
            20, 20, 0, 0, 0, 0, 20, 20,
            20, 30, 10, 0, 0, 10, 30, 20
        };
        SetDictionary(kingGold, kingGreen, kingValues);
        var queenValues = new List<int>()
        {
            -20, -10, -10, -5, -5, -10, -10, -20,
            -10, 0, 0, 0, 0, 0, 0, -10,
            -10, 0, 5, 5, 5, 5, 0, -10,
            -5, 0, 5, 5, 5, 5, 0, -5,
            0, 0, 5, 5, 5, 5, 0, -5,
            -10, 5, 5, 5, 5, 5, 0, -10,
            -10, 0, 5, 0, 0, 0, 0, -10,
            -20, -10, -10, -5, -5, -10, -10, -20
        };
        SetDictionary(queenGold, queenGreen, queenValues);
        var rookValues = new List<int>()
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            5, 10, 10, 10, 10, 10, 10, 5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            -5, 0, 0, 0, 0, 0, 0, -5,
            0, 0, 0, 5, 5, 0, 0, 0
        };
        SetDictionary(rookGold, rookGreen, rookValues);
    }

    void SetDictionary(Dictionary<Vector2Int, int> gold, Dictionary<Vector2Int, int> green, List<int> values)
    {
        var i = 0;
        for (var y = 0; y < 8; y++)
        {
            for (var x = 0; x < 8; x++)
            {
                gold.Add(new Vector2Int(x, y), values[i]);
                green.Add(new Vector2Int(7 - x, 7 - y), values[i++]);
            }
        }
    }
}