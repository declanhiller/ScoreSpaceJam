using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class Enemy : MonoBehaviour {
        
        public ChessMovement chessMovement { get; set; }

        [SerializeField] private ChessGrid chessGrid;
        [SerializeField] private Player.Player player;

        [SerializeField] private float moveSpeed;

        public Vector3Int cellPosition { get; set; }

        public void GiveRandomAbility() {
            
        }

        public void Activate()
        {
            List<ChessMovement.ProposedSpace> allowedSpacesToMoveToo = chessMovement.AllowedSpacesToMoveToo(chessGrid, cellPosition);
            ChessMovement.ProposedSpace closetSpace = allowedSpacesToMoveToo[0];
            Vector3Int playerCellPos = chessGrid.grid.WorldToCell(player.transform.position);
            float smallestDist = Vector3Int.Distance(closetSpace.position, playerCellPos);
            foreach (ChessMovement.ProposedSpace space in allowedSpacesToMoveToo)
            {
                float tmpDistance = Vector3Int.Distance(space.position, playerCellPos);
                if(smallestDist > tmpDistance)
                {
                    smallestDist = tmpDistance;
                    closetSpace = space;
                }
            }

            StartCoroutine(MoveTo(closetSpace.position));

        }
        
        IEnumerator MoveTo(Vector3Int targetCellPos) {
            Vector3 targetPosition = chessGrid.grid.GetCellCenterWorld(targetCellPos);

            while (transform.position != targetPosition) {
                Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                transform.position = nextPos;
                yield return new WaitForEndOfFrame();
            }

            cellPosition = targetCellPos;
            
            // foreach(EnclosedGrid grid in chessGrid.gri)
            
        }
        
        
        
    }
}