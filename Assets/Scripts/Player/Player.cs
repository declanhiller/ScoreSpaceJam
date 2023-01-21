using System;
using System.Collections;
using System.Collections.Generic;
using ChessMovements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : IChessPiece
    {
        private Queue<ChessMovement> queuedMovements = new Queue<ChessMovement>();

        private List<ChessMovement> allMovements = new List<ChessMovement>();

        private Keybinds keybinds;
        
        private Camera camera;

        [SerializeField] private float playerMoveSpeed = 1f;
        
        [SerializeField] private ChessGrid chessGrid;
        [SerializeField] private int numberOfSpacesPlayerIsFromBack = 4;
        
        private static readonly int QUEUE_SIZE = 4;

        private List<Vector3Int> possibleMoves;

        [SerializeField] private Transform validMoveSpriteFolder;
        [SerializeField] private GameObject validMoveSpritePrefab;
        [SerializeField] private int numberOfValidMoveSpritesToPool = 40;
        private List<GameObject> pooledValidMoveSprites;
        

        private void Awake() {
            keybinds = new Keybinds();
            allMovements.Add(new PawnMovement(1));
            allMovements.Add(new RookMovement(3));
            allMovements.Add(new BishopMovement(3));
        }

        private void Start()
        {

            //Pool valid move sprites
            pooledValidMoveSprites = new List<GameObject>();
            for (int i = 0; i < numberOfValidMoveSpritesToPool; i++) {
                GameObject instantiate = Instantiate(validMoveSpritePrefab, validMoveSpriteFolder);
                instantiate.SetActive(false);
                pooledValidMoveSprites.Add(instantiate);
            }
            
            
            keybinds.Enable();
            keybinds.Player.Click.started += OnClickOnBoard;
            
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
            ChessMovement chessMovement = queuedMovements.Peek();
            possibleMoves = chessMovement.AllowedSpacesToMoveToo(chessGrid, chessGrid.grid.WorldToCell(transform.position));
            DisplaySpaces(possibleMoves);
        }

        private void DisplaySpaces(List<Vector3Int> allowedSpacesToMoveToo)
        {
            foreach (Vector3Int space in allowedSpacesToMoveToo) {
                GameObject pooledValidSprite = GetPooledValidSprite();
                pooledValidSprite.transform.position = chessGrid.grid.GetCellCenterWorld(space);
                pooledValidSprite.SetActive(true);
            }
        }

        void OnClickOnBoard(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2Int cellMouseIsIn = (Vector2Int) chessGrid.grid.WorldToCell(mousePos);

            if (!IsMouseContained(cellMouseIsIn)) return;
            
            
            queuedMovements.Dequeue();
            
            DeactivateAllValidSprites();
            
            StartCoroutine(MoveTo(cellMouseIsIn));
            

        }

        IEnumerator MoveTo(Vector2Int cellMouseIsIn) {
            Vector3 targetPosition = chessGrid.grid.GetCellCenterWorld((Vector3Int) cellMouseIsIn);
            while (transform.position != targetPosition) {
                Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPosition, playerMoveSpeed * Time.deltaTime);
                transform.position = nextPos;
                yield return new WaitForEndOfFrame();
            }
            
            //Add new movement
            AddNextMovement();
            
            ShowPossibleSpaces();

        }

        private void AddNextMovement()
        {
            int index = Random.Range(0, allMovements.Count);
            queuedMovements.Enqueue(allMovements[index]);
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

        private GameObject GetPooledValidSprite() {
            for (int i = 0; i < numberOfValidMoveSpritesToPool; i++) {
                if(pooledValidMoveSprites[i].activeInHierarchy) continue;
                return pooledValidMoveSprites[i];
            }
            return null;
        }

        private void DeactivateAllValidSprites() {
            for (int i = 0; i < numberOfValidMoveSpritesToPool; i++) {
                if(!pooledValidMoveSprites[i].activeInHierarchy) continue;
                pooledValidMoveSprites[i].SetActive(false);
            }
        }
        
    }
}