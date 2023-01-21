using System;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class Enemy : MonoBehaviour {
        
        public ChessMovement chessMovement { get; set; }

        public Vector3Int cellPosition { get; set; }

        public void GiveRandomAbility() {
            
        }
        
        private void OnEnable() {
            
        }

        private void OnDisable() {
            
        }
    }
}