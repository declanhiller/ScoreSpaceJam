using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

public class ChessGrid : MonoBehaviour {

    [SerializeField] private Grid internalGrid;

    private static ChessGrid INSTANCE;

    private Camera cam;
    
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase whiteTile;
    [SerializeField] private TileBase blackTile;

    [SerializeField] private Player.Player player;

    [SerializeField] private EnemySpawner enemySpawner;

    [SerializeField] private int outOfBoundsGenerationLength = 8;
    [SerializeField] public int cellYAmountPerGrid = 9;

    public Grid grid => internalGrid;
    

    private const int leftMargin = -4;
    private const int rightMargin = 3;
    private const int bottomMargin = -4;

    private float visibleBottomY;
    private float numOfCellsOnScreen = 10;

    private bool hasGenerated;
    private bool finishedPrelimGeneration;
    private bool generateBlackFirst;
    private int furthestGeneratedY;

    [SerializeField] private int queueSize = 3;
    public EnclosedGrid[] enclosedGridsQueue;

    public List<Enemy> enemies = new List<Enemy>();

    public event Action onEnemyFinishMoving;

    
    
    private void Start() {
        INSTANCE = this;
        enclosedGridsQueue = new EnclosedGrid[queueSize];
        for (int i = 0; i < queueSize; i++) {
            enclosedGridsQueue[i] = GenerateGrid();
        }
        finishedPrelimGeneration = true;
        cam = Camera.main;
        player.LoadExtraStuffCuzImStupid();
    }
    
    

    public void CheckIfFirstGridIsStillVisible() {
        EnclosedGrid enclosedGrid = enclosedGridsQueue[0];
        Vector3Int cell = enclosedGrid.spaces[0, cellYAmountPerGrid - 1];
        Vector3 cellWorldPos = tilemap.GetCellCenterWorld(cell);
        Vector3 cellScreenPos = Camera.main.WorldToScreenPoint(cellWorldPos);
        if (cellScreenPos.y < 0) {
            CleanUpOldGrid();
            for (int i = 1; i < queueSize; i++) {
                enclosedGridsQueue[i - 1] = enclosedGridsQueue[i];
            }

            enclosedGridsQueue[queueSize - 1] = GenerateGrid();
        }
        
    }

    public bool IsCellContained(Vector3Int cellPos) {
        if (cellPos.x >= -4 && cellPos.x <= 3 && cellPos.y >= bottomMargin) {
            return true;
        }
        return false;
    }

    public EnclosedGrid GenerateGrid() {
        int generationStartY = !hasGenerated ? bottomMargin - 1 : furthestGeneratedY;

        //for determining where to put enemies
        Vector3Int[,] spaces = new Vector3Int[8, cellYAmountPerGrid];
        int iIndexCounter = 0;
        int jIndexCounter = 0;
        TileBase tileToPaintWith = generateBlackFirst ? blackTile : whiteTile;
        for (int i = leftMargin; i <= rightMargin; i++) {
            for (int j = generationStartY + 1; j <= generationStartY + cellYAmountPerGrid; j++) {
                Vector3Int pos = new Vector3Int(i, j);
                spaces[iIndexCounter, jIndexCounter] = pos;
                tilemap.SetTile(pos, tileToPaintWith);
                tileToPaintWith = tileToPaintWith == blackTile ? whiteTile : blackTile;
                jIndexCounter++;
            }

            jIndexCounter = 0;
            iIndexCounter++;
        }


        furthestGeneratedY = generationStartY + cellYAmountPerGrid;
        
        generateBlackFirst = !generateBlackFirst;

        EnclosedGrid enclosedGrid = new EnclosedGrid(spaces);
        
        if (!hasGenerated) {
            hasGenerated = true;
        } else {
            enemySpawner.SpawnNewEnemiesforEnclosedGrid(enclosedGrid);
        }

        
        return enclosedGrid;
    }

    private void CleanUpOldGrid() {
        foreach (Vector3Int cell in enclosedGridsQueue[0].spaces)
        {
            tilemap.SetTile(cell, null);
        }
    }


    public bool ContainsEnemy(Vector3Int position) {
        foreach (Enemy enemy in enemies) {
            if (enemy.cellPosition == position) {
                return true;
            }
        }

        return false;
    }

    public bool ContainsEnemy(Vector3Int position, out Enemy output) {
        foreach (Enemy enemy in enemies) {
            if (enemy.cellPosition == position) {
                output = enemy;
                return true;
            }
        }

        output = null;
        return false;
    }

    public void MoveEnemies()
    {
        List<Enemy> visibleEnemies = GetVisibleEnemies();
        foreach (Enemy enemy in visibleEnemies) {
            enemy.Activate();
        }
        StartCoroutine(CheckIfEnemiesAreDone());
    }

    IEnumerator CheckIfEnemiesAreDone() {
        bool enemiesStillMoving = true;
        while (enemiesStillMoving) {
            enemiesStillMoving = false;
            foreach (Enemy enemy in enemies) {
                if (enemy.moving) {
                    enemiesStillMoving = true;
                    break;
                }
            }
            
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Show possible spaces");
        onEnemyFinishMoving.Invoke();
    }

    private List<Enemy> GetVisibleEnemies()
    {
        List<Enemy> returnList = new List<Enemy>();
        foreach (Enemy enemy in enemies) {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.grid.GetCellCenterWorld(enemy.cellPosition));
            bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
            if(onScreen) returnList.Add(enemy);
        }

        return returnList;
    }
}