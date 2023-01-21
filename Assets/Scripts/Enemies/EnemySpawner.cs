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

        [SerializeField] private ChessGrid cellGrid;

        public void SpawnNewEnemiesforEnclosedGrid(EnclosedGrid grid) {
            float enemyDensity = 0.1f;
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
            int range = Random.Range(0, 5);
            switch (range) {
                //should be using factory pattern here but currently dying so won't bother
                case 0:
                    CreateNewPawn(instantiate);
                    break;
                default:
                    CreateNewPawn(instantiate);
                    break;
            }

            return instantiate.GetComponent<Enemy>();
        }

        public void CreateNewPawn(GameObject obj) {
            obj.GetComponent<Enemy>().chessMovement = new PawnMovement(1);
            obj.GetComponent<SpriteRenderer>().sprite = pawn;
        }


        
        public static T[] GetRandomValues<T>(T[,] array, int numOfRandomValues)
        {
            
            
            Debug.Log("1st dimension is " + array.GetLength(0));
            Debug.Log("2nd dimension is " + array.GetLength(1));
            
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