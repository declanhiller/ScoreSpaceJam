using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class EnclosedGrid {
    public Vector3Int[,] spaces { get; }
    public List<Enemy> enemies = new List<Enemy>();

    public EnclosedGrid(Vector3Int[,] spaces) {
        this.spaces = spaces;
    }
    
    
}