using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class ChessMovement {


        private const int maxDistance = 8;

        protected int actualDistance;

        protected int distance {
            get => actualDistance;
            set {
                if (value > maxDistance) {
                    actualDistance = maxDistance;
                    return;
                }
                actualDistance = value;
            }
        }

        protected ChessMovement(int actualDistance) {
            this.actualDistance = actualDistance;
        }
        
        private List<IAbility> abilities = new List<IAbility>();

        public abstract List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn);
        
        public struct ProposedSpace {
            public bool containsEnemy;
            public Vector3Int position;
        }
    }
}