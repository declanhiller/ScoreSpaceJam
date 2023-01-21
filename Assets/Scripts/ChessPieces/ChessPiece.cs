using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public abstract class ChessPiece : MonoBehaviour
    {
        public Vector3Int cellPosition { get; private set;}

        private ChessGrid grid;
        
        
    }
}