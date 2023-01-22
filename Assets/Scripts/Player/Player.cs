using System;
using System.Collections;
using System.Collections.Generic;
using ChessMovements;
using Enemies;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Player
{
    public class Player : ChessPiece
    {
        private Queue<ChessMovement> queuedMovements = new Queue<ChessMovement>();

        private List<ChessMovement> allMovements = new List<ChessMovement>();

        private Keybinds keybinds;
        
        private Camera camera;

        [SerializeField] private float playerMoveSpeed = 1f;
        
        [SerializeField] private ChessGrid chessGrid;

        [SerializeField] private PieceQueue pieceQueue;

        [SerializeField] private TextMeshProUGUI scoreText;
        
        private static readonly int QUEUE_SIZE = 4;

        private List<ChessMovement.ProposedSpace> possibleMoves;

        [SerializeField] private Transform validMoveSpriteFolder;
        [SerializeField] private GameObject validMoveSpritePrefab;
        [SerializeField] private int numberOfValidMoveSpritesToPool = 40;
        private List<GameObject> pooledValidMoveSprites;

        private int score;

        

        private void Awake() {
            keybinds = new Keybinds();
            allMovements.Add(new RookMovement(3));
            allMovements.Add(new BishopMovement(3));
            allMovements.Add(new QueenMovement(3));
            allMovements.Add(new KingMovement(1));
            allMovements.Add(new KnightMovement(1));
        }

        private void Start()
        {

            //Pool valid move sprites

            score = 0;

            //Display the possible movements

        }

        //this is really shitty design :p, don't tightly couple ur classes peeps
        public void LoadExtraStuffCuzImStupid() {
            
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
            
            pieceQueue.UpdateDisplay(queuedMovements);

            ShowPossibleSpaces();

            chessGrid.onEnemyFinishMoving += ShowPossibleSpaces;
            

        }
        

        private void ShowPossibleSpaces()
        {
            ChessMovement chessMovement = queuedMovements.Dequeue();
            possibleMoves = chessMovement.AllowedSpacesToMoveToo(chessGrid, chessGrid.grid.WorldToCell(transform.position));
            DisplaySpaces(possibleMoves);
        }

        private void DisplaySpaces(List<ChessMovement.ProposedSpace> allowedSpacesToMoveToo)
        {
            foreach (ChessMovement.ProposedSpace space in allowedSpacesToMoveToo) {
                GameObject pooledValidSprite = GetPooledValidSprite();
                pooledValidSprite.transform.position = chessGrid.grid.GetCellCenterWorld(space.position);
                pooledValidSprite.SetActive(true);
                if (space.containsEnemy) {
                    pooledValidSprite.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                } else {
                    pooledValidSprite.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0);
                }
            }
        }

        void OnClickOnBoard(InputAction.CallbackContext context)
        {
            Vector2 mousePos = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int cellMouseIsIn = chessGrid.grid.WorldToCell(mousePos);

            if (!IsMouseContained(cellMouseIsIn)) return;
            
            
            // queuedMovements.Dequeue();
            
            DeactivateAllValidSprites();
            
            StartCoroutine(MoveTo(cellMouseIsIn));
        }

        private bool firstTime = true;
        
        IEnumerator MoveTo(Vector3Int cellMouseIsIn) {
            Vector3 targetPosition = chessGrid.grid.GetCellCenterWorld((Vector3Int) cellMouseIsIn);

            int scoreIncrease = cellMouseIsIn.y - chessGrid.grid.WorldToCell(transform.position).y;

            while (transform.position != targetPosition) {
                Vector3 nextPos = Vector3.MoveTowards(transform.position, targetPosition, playerMoveSpeed * Time.deltaTime);
                transform.position = nextPos;
                yield return new WaitForEndOfFrame();
            }

            if (scoreIncrease > 0) {
                score += scoreIncrease;
                scoreText.text = "" + score;
            }

            chessGrid.CheckIfFirstGridIsStillVisible();
            
            AddNextMovement();
            
            pieceQueue.UpdateDisplay(queuedMovements);

            bool containsEnemy = chessGrid.ContainsEnemy(cellMouseIsIn, out var enemy);
            if (containsEnemy) {
                chessGrid.enemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }

            chessGrid.MoveEnemies();
        }

        private void AddNextMovement() {
            int index = Random.Range(0, allMovements.Count);
            queuedMovements.Enqueue(allMovements[index]);
        }

        private bool IsMouseContained(Vector3Int worldToCell)
        {
            foreach (ChessMovement.ProposedSpace cell in possibleMoves)
            {
                if (cell.position.Equals(worldToCell))
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