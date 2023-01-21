using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class KingMovement : ChessMovement {
        public KingMovement(int actualDistance) : base(actualDistance) {
        }

        public override List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn) {
            List<ProposedSpace> returnList = new List<ProposedSpace>();

            for (int i = -1; i <= 1; i++) {
                for (int j = -1; j <= 1; j++) {
                    for (int k = 1; k <= actualDistance; k++) {
                        if (i == 0 && j == 0) break;
                        ProposedSpace space = new ProposedSpace();
                        Vector3Int cellLocation = new Vector3Int(cellPieceIsIn.x + i * k, cellPieceIsIn.y + j * k);
                        space.position = cellLocation;
                        if(!grid.IsCellContained(cellLocation)) continue;
                        space.containsEnemy = grid.ContainsEnemy(cellLocation);
                        returnList.Add(space);
                        if (space.containsEnemy) {
                            break;
                        }
                    }
                    
                }
            }
            

            return returnList;
        }
    }
}