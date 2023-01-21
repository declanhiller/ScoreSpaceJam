using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class RookMovement : ChessMovement {
        
        public RookMovement(int actualDistance) : base(actualDistance) {
        }

        public override List<Vector3Int> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn) {
            List<Vector3Int> returnList = new List<Vector3Int>();

            for (int i = 0; i < 4; i++) {
                for (int j = 1; j <= actualDistance; j++) {
                    Vector3Int proposedSpace = new Vector3Int(cellPieceIsIn.x, cellPieceIsIn.y);
                    switch (i) {
                        case 0:
                            proposedSpace.Set(proposedSpace.x + j, proposedSpace.y, 0);
                            break;
                        case 1:
                            proposedSpace.Set(proposedSpace.x - j, proposedSpace.y, 0);
                            break;
                        case 2:
                            proposedSpace.Set(proposedSpace.x, proposedSpace.y + j, 0);
                            break;
                        case 3:
                            proposedSpace.Set(proposedSpace.x, proposedSpace.y - j, 0);
                            break;
                    }

                    if(!grid.IsCellContained(proposedSpace)) continue;
                    returnList.Add(proposedSpace);
                }
            }

            return returnList;

        }
    }
}