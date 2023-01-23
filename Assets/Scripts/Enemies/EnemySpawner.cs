using System;
using System.Collections.Generic;
using System.Linq;
using ChessMovements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject enemy;
        [SerializeField] private Transform folderForNumbers;
        [SerializeField] private GameObject uiNumber;

        [SerializeField] private Sprite pawn;
        [SerializeField] private Sprite rook;
        [SerializeField] private Sprite bishop;
        [SerializeField] private Sprite king;
        [SerializeField] private Sprite queen;
        [SerializeField] private Sprite knight;

        [SerializeField] private Camera camera;

        [SerializeField] private ChessGrid cellGrid;

        [NonSerialized] public float enemyDensity = 0.05f;

        public void SpawnNewEnemiesforEnclosedGrid(EnclosedGrid grid) {
            int numberOfCells = grid.spaces.GetLength(0) * grid.spaces.GetLength(1);
            int numberOfEnemies = (int) (enemyDensity * numberOfCells);

            Vector3Int[] randomValues = GetRandomValues(grid.spaces, numberOfEnemies);

            foreach (Vector3Int cell in randomValues) {
                Enemy newEnemy = CreateNewEnemy(cellGrid.grid.GetCellCenterWorld(cell));
                newEnemy.cellPosition = cell;
                cellGrid.enemies.Add(newEnemy);
                grid.enemies.Add(newEnemy);
            }
            
        }
        
        public Enemy CreateNewEnemy(Vector3 worldSpace) {
            GameObject instantiate = Instantiate(enemy, worldSpace, Quaternion.identity);
            GameObject uiElement = Instantiate(uiNumber, camera.WorldToScreenPoint(worldSpace), Quaternion.identity,
                folderForNumbers);
            instantiate.GetComponent<Enemy>().moveTracker = uiElement;
            int range = Random.Range(0, 4);
            switch (range) {
                //should be using factory pattern here but currently dying so won't bother
                case 0:
                    CreateNewBishop(instantiate);
                    break;
                case 1:
                    CreateNewKing(instantiate);
                    break;
                case 2:
                    CreateNewKnight(instantiate);
                    break;
                case 3:
                    CreateNewRook(instantiate);
                    break;
                case 4:
                    CreateNewQueen(instantiate);
                    break;
            }

            return instantiate.GetComponent<Enemy>();
        }
        

        public void CreateNewKing(GameObject obj)
        {
            obj.GetComponent<Enemy>().SetChessMovement(new KingMovement(1));
            obj.GetComponent<SpriteRenderer>().sprite = king;
        }

        public void CreateNewRook(GameObject obj)
        {
            int range = Random.Range(1, 6);
            obj.GetComponent<Enemy>().SetChessMovement(new RookMovement(range));
            obj.GetComponent<SpriteRenderer>().sprite = rook;
        }

        public void CreateNewBishop(GameObject obj)
        {
            int range = Random.Range(1, 6);
            obj.GetComponent<Enemy>().SetChessMovement(new BishopMovement(range));
            obj.GetComponent<SpriteRenderer>().sprite = bishop;
        }

        public void CreateNewQueen(GameObject obj) {
            int range = Random.Range(1, 6);
            obj.GetComponent<Enemy>().SetChessMovement(new QueenMovement(range));
            obj.GetComponent<SpriteRenderer>().sprite = queen;
        }

        public void CreateNewKnight(GameObject obj)
        {
            obj.GetComponent<Enemy>().SetChessMovement(new KnightMovement(1));
            obj.GetComponent<SpriteRenderer>().sprite = knight;
        }


        
        public static T[] GetRandomValues<T>(T[,] array, int numOfRandomValues)
        {
            int values = array.GetLength(0) * array.GetLength(1);
            System.Random rand = new System.Random();

            IEnumerable<int> enumerable = Enumerable.Range(0, values - 1).OrderBy(x => rand.Next()).Take(numOfRandomValues);

            T[] returnArr = new T[numOfRandomValues];


            int counter = 0;
            foreach (int index in enumerable) {
                returnArr[counter] = array[index / array.GetLength(1), index % array.GetLength(1)];
                counter++;
            }

            return returnArr;
        }
        
    }
}