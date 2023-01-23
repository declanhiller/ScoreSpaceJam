using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class PawnMovement : ChessMovement {
        
        
        public override List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn, bool isEnemy) {
            Vector3Int availableSpace = new Vector3Int(cellPieceIsIn.x, cellPieceIsIn.y + actualDistance);
            List<ProposedSpace> returnList = new List<ProposedSpace>();
            
            // returnList.Add(availableSpace);
            
            return null;
        }

        public PawnMovement(int actualDistance) : base(actualDistance) {
            
        }
    }
}