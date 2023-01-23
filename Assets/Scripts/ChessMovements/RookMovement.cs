using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements {
    public class RookMovement : ChessMovement {
        
        public RookMovement(int actualDistance) : base(actualDistance) {
        }

        public override List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn, bool isEnemy) {
            List<ProposedSpace> returnList = new List<ProposedSpace>();

            for (int i = 0; i < 4; i++) {
                for (int j = 1; j <= actualDistance; j++) {
                    Vector3Int position = new Vector3Int(cellPieceIsIn.x, cellPieceIsIn.y);
                    ProposedSpace proposedSpace = new ProposedSpace();

                    switch (i) {
                        case 0:
                            position.Set(position.x + j, position.y, 0);
                            break;
                        case 1:
                            position.Set(position.x - j, position.y, 0);
                            break;
                        case 2:
                            position.Set(position.x, position.y + j, 0);
                            break;
                        case 3:
                            position.Set(position.x, position.y - j, 0);
                            break;
                    }

                    if(!grid.IsCellContained(position)) continue;
                    proposedSpace.containsEnemy = grid.ContainsEnemy(position);
                    proposedSpace.position = position;
                    returnList.Add(proposedSpace);
                    if (proposedSpace.containsEnemy) break;
                }
            }

            return returnList;

        }


    }
}