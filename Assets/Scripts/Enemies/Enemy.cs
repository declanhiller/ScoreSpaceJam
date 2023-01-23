using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Enemies {
    public class Enemy : MonoBehaviour {
        
        public ChessMovement chessMovement { get; set; }

        [SerializeField] private ChessGrid chessGrid;
        [SerializeField] private Player.Player player;

        [SerializeField] private Transform uiMarker;

        [NonSerialized] public GameObject moveTracker;

        [SerializeField] private float moveSpeed;

        [NonSerialized] public bool moving;

        private Camera camera;

        public Vector3Int cellPosition { get; set; }

        private bool isGoingToLose;

        private void Start() {
            camera = Camera.main;
        }

        public void SetChessMovement(ChessMovement movement) {
            chessMovement = movement;
            moveTracker.GetComponentInChildren<TextMeshProUGUI>().text = "" + movement.distance;
        }

        public void GiveRandomAbility() {
            
        }

        private void LateUpdate() {
            if (moveTracker != null) {
                moveTracker.transform.position = camera.WorldToScreenPoint(uiMarker.position);
            }
            
        }

        public void Activate() {
            moving = true;
            List<ChessMovement.ProposedSpace> allowedSpacesToMoveToo = chessMovement.AllowedSpacesToMoveToo(chessGrid, cellPosition, true);
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

                if (tmpDistance == 0) {
                    isGoingToLose = true;
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

            if (isGoingToLose) {
                LootLockerManager.INSTANCE.playerScore = player.score;
                SceneController.INSTANCE.Leaderboard();
            }

            cellPosition = targetCellPos;
            
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(chessGrid.grid.GetCellCenterWorld(cellPosition));

            moving = false;
            
            if (screenPoint.y < 0) {
                chessGrid.enemies.Remove(this);
                Destroy(gameObject);
            }
            
        }

        private void OnDestroy() {
            Destroy(moveTracker);
        }
    }
}