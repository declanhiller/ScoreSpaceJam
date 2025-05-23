using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class ChessMovement {


        private const int maxDistance = 8;

        protected int actualDistance;

        public int distance {
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
        
        public List<Ability> abilities = new List<Ability>();

        public abstract List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn, bool isEnemy);
        
        public struct ProposedSpace {
            public bool containsEnemy;
            public Vector3Int position;
        }
    }
}