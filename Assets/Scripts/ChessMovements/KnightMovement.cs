using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace ChessMovements
{
    public class KnightMovement : ChessMovement
    {
        
        int knightFirstMoveDist = 2;

        public KnightMovement(int actualDistance) : base(actualDistance)
        {
        }

        //This is extraordinarily bad code that should not be used as reference
        public override List<ProposedSpace> AllowedSpacesToMoveToo(ChessGrid grid, Vector3Int cellPieceIsIn)
        {
            List<ProposedSpace> returnList = new List<ProposedSpace>();
            
            
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (Math.Abs(i) == Math.Abs(j)) continue;
                    int iPos = i * knightFirstMoveDist + cellPieceIsIn.x;
                    int jPos = j * knightFirstMoveDist + cellPieceIsIn.y;
                    if (i == 0)
                    {
                        for (int k = 1; k <= distance; k++)
                        {
                            ProposedSpace proposedSpace1 = new ProposedSpace();
                            proposedSpace1.position = new Vector3Int(iPos + k, jPos);
                            ProposedSpace proposedSpace2 = new ProposedSpace();
                            proposedSpace2.position = new Vector3Int(iPos - k, jPos);
                            if (grid.IsCellContained(proposedSpace1.position))
                            {
                                proposedSpace1.containsEnemy = grid.ContainsEnemy(proposedSpace1.position);
                                returnList.Add(proposedSpace1);
                            }

                            if (grid.IsCellContained(proposedSpace2.position))
                            {
                                proposedSpace2.containsEnemy = grid.ContainsEnemy(proposedSpace2.position);
                                returnList.Add(proposedSpace2);
                            }
                        }
                    }else if (j == 0)
                    {
                        for (int k = 1; k <= distance; k++)
                        {
                            ProposedSpace proposedSpace1 = new ProposedSpace();
                            proposedSpace1.position = new Vector3Int(iPos, jPos + k);
                            ProposedSpace proposedSpace2 = new ProposedSpace();
                            proposedSpace2.position = new Vector3Int(iPos, jPos - k);
                            if (grid.IsCellContained(proposedSpace1.position))
                            {
                                proposedSpace1.containsEnemy = grid.ContainsEnemy(proposedSpace1.position);
                                returnList.Add(proposedSpace1);
                            }

                            if (grid.IsCellContained(proposedSpace2.position))
                            {
                                proposedSpace2.containsEnemy = grid.ContainsEnemy(proposedSpace2.position);
                                returnList.Add(proposedSpace2);
                            }
                        }
                    }
                    
                }
            }


            return returnList;
        }
    }
}