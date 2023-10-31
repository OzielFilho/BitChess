using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlights : MonoBehaviour
{
    public static Highlights instance;
    public SpriteRenderer highlightsPrefab;
    private Queue<SpriteRenderer> _activeHighlights = new ();
    private Queue<SpriteRenderer> _onReserve = new ();
    
    private void Awake()
    {
        instance = this;
    }
    
    public void SelectTiles(List<AvailableMove> availableMoves)
    {
        foreach (var move in availableMoves)
        {
            if(_onReserve.Count == 0) CreateHighlight();
            var spriteRenderer = _onReserve.Dequeue();
            spriteRenderer.gameObject.SetActive(true);
            spriteRenderer.color = StateMachineController.instance.currentlyPlayer.color;
            spriteRenderer.transform.position = new Vector3(move.pos.x, move.pos.y, 0);
            spriteRenderer.GetComponent<HighlightClick>().move = move;
            _activeHighlights.Enqueue(spriteRenderer);
        }
    }
    
    public void DeSelectTiles()
    {
        while(_activeHighlights.Count != 0)
        {
            var spriteRenderer = _activeHighlights.Dequeue();
            spriteRenderer.gameObject.SetActive(false);
            _onReserve.Enqueue(spriteRenderer);
        }
    }
    
    private void CreateHighlight()
    {
        var spriteRenderer = Instantiate(highlightsPrefab, Vector3.zero, Quaternion.identity, transform);
        _onReserve.Enqueue(spriteRenderer);
    }
}
