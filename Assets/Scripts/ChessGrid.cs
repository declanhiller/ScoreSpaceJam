using System;
using UnityEngine;

public class ChessGrid : MonoBehaviour {

    [SerializeField] private Grid internalGrid;

    public Grid grid => internalGrid;

    private const int leftMargin = -4;
    private const int rightMargin = 3;
    private const int bottomMargin = -4;
    

    public bool IsCellContained(Vector3Int cellPos) {
        if (cellPos.x >= -4 && cellPos.x <= 3 && cellPos.y >= bottomMargin) {
            return true;
        }
        return false;
    }

    public void GenerateGrid() {
        
    }

}