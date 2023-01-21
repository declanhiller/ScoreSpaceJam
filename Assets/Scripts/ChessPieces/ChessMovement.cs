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

        public abstract List<Vector3Int> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn);
    }
}