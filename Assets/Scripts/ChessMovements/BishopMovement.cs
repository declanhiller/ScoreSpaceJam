using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class BishopMovement : ChessMovement {
        
        public BishopMovement(int actualDistance) : base(actualDistance) {
        }

        public override List<Vector3Int> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn) {
            List<Vector3Int> returnList = new List<Vector3Int>();

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    if(i == 0 || j == 0) continue;
                    for (int k = 1; k <= distance; k++) {
                        Vector3Int proposedSpace = new Vector3Int(cellPieceIsIn.x + i * k, cellPieceIsIn.y + j * k);
                        if(!grid.IsCellContained(proposedSpace)) continue;
                        returnList.Add(proposedSpace);
                    }
                }
            }

            return returnList;
        }
    }
}