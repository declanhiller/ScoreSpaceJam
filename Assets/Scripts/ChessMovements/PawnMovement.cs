using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class PawnMovement : ChessMovement {
        
        
        public override List<Vector3Int> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn) {
            Vector3Int availableSpace = new Vector3Int(cellPieceIsIn.x, cellPieceIsIn.y + actualDistance);
            List<Vector3Int> returnList = new List<Vector3Int>();
            
            returnList.Add(availableSpace);
            
            return returnList;
        }

        public PawnMovement(int actualDistance) : base(actualDistance) {
            
        }
    }
}