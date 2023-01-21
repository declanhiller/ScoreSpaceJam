using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class IChessMovement
    {
        private int distance;
        
        private List<IAbility> abilities = new List<IAbility>();
        
        public abstract List<Vector2Int> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int currentCell);
    }
}