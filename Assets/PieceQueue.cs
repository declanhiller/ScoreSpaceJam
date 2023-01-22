using System;
using System.Collections;
using System.Collections.Generic;
using ChessMovements;
using Player;
using UnityEngine;
using UnityEngine.UI;

public class PieceQueue : MonoBehaviour {
    [SerializeField] private Image[] containers;
    
    [SerializeField] private Sprite rook;
    [SerializeField] private Sprite bishop;
    [SerializeField] private Sprite knight;
    [SerializeField] private Sprite queen;
    [SerializeField] private Sprite king;

    [SerializeField] private ChessGrid chessGrid;
    [SerializeField] private Vector3Int queueLocation;

    private void Start() {
        //set correct position
        // RectTransform rectTransform = transform as RectTransform;
        // Vector3 worldPos = chessGrid.grid.CellToWorld(queueLocation);
        // Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        // rectTransform.position = new Vector2(screenPos.x, screenPos.y);
    }

    public void UpdateDisplay(Queue<ChessMovement> queue) {
        ChessMovement[] chessMovements = queue.ToArray();
        for (int i = 0; i < containers.Length; i++) {
            ChessMovement chessMovement = chessMovements[i];
            Image container = containers[i];
            //apart of me dies writing this code, sprite should have been stored with chess movement :p
            if (chessMovement.GetType() == typeof(RookMovement)) {
                container.sprite = rook;
            } else if (chessMovement.GetType() == typeof(BishopMovement)) {
                container.sprite = bishop;
            } else if (chessMovement.GetType() == typeof(KnightMovement)) {
                container.sprite = knight;
            } else if (chessMovement.GetType() == typeof(QueenMovement)) {
                container.sprite = queen;
            } else if (chessMovement.GetType() == typeof(KingMovement)) {
                container.sprite = king;
            }
        }
    }
    

}
