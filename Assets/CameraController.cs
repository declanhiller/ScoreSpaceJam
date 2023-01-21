using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private int cameraCellOffset = 2;
    [SerializeField] private ChessGrid chessGrid;
    [SerializeField] private Transform player;

    [SerializeField] private float xOffset = 3f;
    private float finalXPos;
    
    private const float zOffset = -10;

    private float yOffsetValue;
    
    
    private void Start() {
        // Vector3Int playerCellPos = chessGrid.grid.WorldToCell(player.position);
        // playerCellPos.Set(playerCellPos.x, playerCellPos.y + cameraCellOffset, playerCellPos.z);
        // Vector3 finalCameraPos = chessGrid.grid.GetCellCenterWorld(playerCellPos);
        // finalCameraPos.Set(finalCameraPos.x, finalCameraPos.y + chessGrid.grid.cellSize.y - chessGrid.grid.cellSize.y/2, zOffset);
        yOffsetValue = (chessGrid.grid.cellSize.y * cameraCellOffset) - (chessGrid.grid.cellSize.y/2);
        finalXPos = player.position.x + xOffset;

    }

    private void Update() {
        float yPos = player.position.y + yOffsetValue;
        if (yPos > transform.position.y) { 
            transform.position = new Vector3(0, yPos, zOffset);
        }
        
    }
    
    
}
