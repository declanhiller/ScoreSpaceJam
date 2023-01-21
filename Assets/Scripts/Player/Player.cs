using System;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : IChessPiece
    {
        private Queue<IChessMovement> queuedMovements;

        private List<IChessMovement> allMovements;
        
        private Camera camera;
        private ChessGrid chessGrid;
        private static readonly int QUEUE_SIZE = 4;

        private List<Vector2Int> possibleMoves;

        private void Start()
        {
            camera = Camera.main;
            //Generate Movements for the queue
            for (int i = 0; i < QUEUE_SIZE; i++)
            {
                AddNextMovement();
            }

            //Display the possible movements
            ShowPossibleSpaces();
        }

        private void ShowPossibleSpaces()
        {
            IChessMovement chessMovement = queuedMovements.Peek();
            possibleMoves = chessMovement.AllowedSpacesToMoveToo(chessGrid, chessGrid.grid.WorldToCell(transform.position));
            DisplaySpaces(possibleMoves);
        }

        private void DisplaySpaces(List<Vector2Int> allowedSpacesToMoveToo)
        {
            // throw new NotImplementedException();
        }

        void OnClickOnBoard(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2Int cellMouseIsIn = (Vector2Int) chessGrid.grid.WorldToCell(mousePos);

            if (!IsMouseContained(cellMouseIsIn)) return;

            MoveToo(cellMouseIsIn);

            queuedMovements.Dequeue();
            
            //Add new movement
            AddNextMovement();
            
            ShowPossibleSpaces();

        }

        private void AddNextMovement()
        {
            int index = Random.Range(0, allMovements.Count - 1);
            queuedMovements.Enqueue(allMovements[index]);
        }

        private void MoveToo(Vector2Int cellMouseIsIn)
        {
            transform.position = chessGrid.grid.GetCellCenterWorld((Vector3Int) cellMouseIsIn);
        }

        private bool IsMouseContained(Vector2Int worldToCell)
        {
            foreach (Vector2Int cell in possibleMoves)
            {
                if (cell.Equals(worldToCell))
                {
                    return true;
                }
            }

            return false;
        }

        private IChessMovement GetNextMovement()
        {
            IChessMovement chessMovement = queuedMovements.Dequeue();
            //Generate random movement and queue next movement
            return chessMovement;
        }

        
    }
}