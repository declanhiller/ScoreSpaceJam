using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceQueue : MonoBehaviour {
    [SerializeField] private Transform mainHolder;
    [SerializeField] private Transform holder1;
    [SerializeField] private Transform holder2;
    [SerializeField] private Transform holder3;

    [SerializeField] private GameObject pawn;
    [SerializeField] private GameObject rook;
    [SerializeField] private GameObject bishop;

    [SerializeField] private ChessGrid chessGrid;
    [SerializeField] private Vector3Int queueLocation;

    private void Start() {
        //set correct position
        RectTransform rectTransform = transform as RectTransform;
        Vector3 worldPos = chessGrid.grid.CellToWorld(queueLocation);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rectTransform.position = new Vector2(screenPos.x, screenPos.y);
    }
    

}
