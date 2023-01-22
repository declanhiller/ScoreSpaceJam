using System;
using System.Collections.Generic;
using System.Linq;
using ChessMovements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies {
    public class EnemySpawner : MonoBehaviour {
        [SerializeField] private GameObject enemy;

        [SerializeField] private Sprite pawn;
        [SerializeField] private Sprite rook;
        [SerializeField] private Sprite bishop;
        [SerializeField] private Sprite king;
        [SerializeField] private Sprite queen;
        [SerializeField] private Sprite knight;

        [SerializeField] private ChessGrid cellGrid;

        public void SpawnNewEnemiesforEnclosedGrid(EnclosedGrid grid) {
            float enemyDensity = 0.05f;
            int numberOfCells = grid.spaces.GetLength(0) * grid.spaces.GetLength(1);
            int numberOfEnemies = (int) (enemyDensity * numberOfCells);

            Vector3Int[] randomValues = GetRandomValues(grid.spaces, numberOfEnemies);

            foreach (Vector3Int cell in randomValues) {
                Enemy newEnemy = CreateNewEnemy(cellGrid.grid.GetCellCenterWorld(cell));
                newEnemy.cellPosition = cell;
                grid.enemies.Add(newEnemy);
            }
            
        }
        
        public Enemy CreateNewEnemy(Vector3 worldSpace) {
            GameObject instantiate = Instantiate(enemy, worldSpace, Quaternion.identity);
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
            obj.GetComponent<Enemy>().chessMovement = new KingMovement(1);
            obj.GetComponent<SpriteRenderer>().sprite = king;
        }

        public void CreateNewRook(GameObject obj)
        {
            obj.GetComponent<Enemy>().chessMovement = new RookMovement(3);
            obj.GetComponent<SpriteRenderer>().sprite = rook;
        }

        public void CreateNewBishop(GameObject obj)
        {
            obj.GetComponent<Enemy>().chessMovement = new BishopMovement(3);
            obj.GetComponent<SpriteRenderer>().sprite = bishop;
        }

        public void CreateNewQueen(GameObject obj)
        {
            obj.GetComponent<Enemy>().chessMovement = new QueenMovement(5);
            obj.GetComponent<SpriteRenderer>().sprite = queen;
        }

        public void CreateNewKnight(GameObject obj)
        {
            obj.GetComponent<Enemy>().chessMovement = new KnightMovement(1);
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