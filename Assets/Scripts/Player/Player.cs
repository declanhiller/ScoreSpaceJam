using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private SpriteRenderer renderer;

        [SerializeField] private float playerMoveSpeed = 1f;
        
        [SerializeField] private ChessGrid chessGrid;

        [SerializeField] private PieceQueue pieceQueue;

        [SerializeField] private TextMeshProUGUI scoreText;

        [SerializeField] private Sprite bishop;
        [SerializeField] private Sprite king;
        [SerializeField] private Sprite knight;
        [SerializeField] private Sprite queen;
        [SerializeField] private Sprite rook;
        
        private static readonly int QUEUE_SIZE = 4;

        private List<ChessMovement.ProposedSpace> possibleMoves;
        private ChessMovement currentShownChessMovement;

        [SerializeField] private Transform validMoveSpriteFolder;
        [SerializeField] private GameObject validMoveSpritePrefab;
        [SerializeField] private int numberOfValidMoveSpritesToPool = 40;
        private List<GameObject> pooledValidMoveSprites;

        [SerializeField] private EnemySpawner spawner;

        [SerializeField] private PieceChooser pieceChooser;

        private int killedEnemies = 0;
        private int enemyIncrease = 1;
        private int nextBenchmark = 1;

        private int howMuchScoreToIncreaseEnemyDensity = 20;
        private float ratioToIncreaseBy = 1.03f;
        private int increaseEnemyCounter;
        

        private bool canClick = true;

        public int score { get; private set; }



        private void Awake() {
            keybinds = new Keybinds();
            allMovements.Add(new RookMovement(2));
            allMovements.Add(new BishopMovement(2));
            allMovements.Add(new QueenMovement(2));
            allMovements.Add(new KingMovement(1));
            allMovements.Add(new KnightMovement(1));
        }

        private void Start() {

            renderer = GetComponent<SpriteRenderer>();
            
            //Pool valid move sprites

            score = 0;
            pieceChooser.gameObject.SetActive(false);
            increaseEnemyCounter = howMuchScoreToIncreaseEnemyDensity;
            camera = Camera.main;


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
            

            
            //Generate Movements for the queue
            for (int i = 0; i < QUEUE_SIZE; i++)
            {
                AddNextMovement();
            }
            
            pieceQueue.UpdateDisplay(queuedMovements);

            ShowPossibleSpaces();

            chessGrid.onEnemyFinishMoving += ShowPossibleSpaces;
            

        }

        private void OnDestroy() {
            keybinds.Player.Click.started -= OnClickOnBoard;
        }


        private void ShowPossibleSpaces()
        {
            ChessMovement chessMovement = queuedMovements.Dequeue();
            currentShownChessMovement = chessMovement;
            possibleMoves = chessMovement.AllowedSpacesToMoveToo(chessGrid, chessGrid.grid.WorldToCell(transform.position), false);
            if (chessMovement.GetType() == typeof(BishopMovement)) {
                renderer.sprite = bishop;
            } else if (chessMovement.GetType() == typeof(KingMovement)) {
                renderer.sprite = king;
            } else if (chessMovement.GetType() == typeof(KnightMovement)) {
                renderer.sprite = knight;
            } else if (chessMovement.GetType() == typeof(QueenMovement)) {
                renderer.sprite = queen;
            } else if (chessMovement.GetType() == typeof(RookMovement)) {
                renderer.sprite = rook;
            }
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

        void OnClickOnBoard(InputAction.CallbackContext context) {
            if (!canClick) return;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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
                increaseEnemyCounter -= scoreIncrease;
                if (increaseEnemyCounter <= 0) {
                    Debug.Log("Increase enemy density");
                    spawner.enemyDensity = spawner.enemyDensity * ratioToIncreaseBy;
                    increaseEnemyCounter = howMuchScoreToIncreaseEnemyDensity;
                }
            }

            chessGrid.CheckIfFirstGridIsStillVisible();
            
            AddNextMovement();
            
            pieceQueue.UpdateDisplay(queuedMovements);

            bool containsEnemy = chessGrid.ContainsEnemy(cellMouseIsIn, out var enemy);
            if (containsEnemy) {
                killedEnemies++;
                chessGrid.enemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }

            //activate abilities first
            StartCoroutine(ActivateAbilities());
            
        }
        

        IEnumerator ActivateAbilities() {
            List<Ability> abilities = currentShownChessMovement.abilities;
            Ability[] orderedAbilities = abilities.OrderBy(x => x.priority).ToArray();
            for (int i = 0; i < orderedAbilities.Length; i++) {
                Ability ability = orderedAbilities[i];
                ability.Activate(this);
                while (!ability.IsDone()) {
                    ability.Tick();
                    yield return new WaitForEndOfFrame();
                }
            }
            chessGrid.MoveEnemies();
            
            if (killedEnemies >= nextBenchmark) {
                Debug.Log("get next ability");
                nextBenchmark += enemyIncrease;
                canClick = false;
                pieceChooser.gameObject.SetActive(true);
                System.Random rand = new System.Random();

                IEnumerable<int> enumerable = Enumerable.Range(0, allMovements.Count - 1).OrderBy(x => rand.Next()).Take(3);
                List<ChessMovement> list = new List<ChessMovement>();
                foreach (int index in enumerable) {
                    list.Add(allMovements[index]);
                }
                pieceChooser.BringUpAbilities((() => {
                    canClick = true;
                    pieceChooser.gameObject.SetActive(false);
                }), list);
            }
            
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