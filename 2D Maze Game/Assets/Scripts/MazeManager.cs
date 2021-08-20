using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    void Start()
    {
        Generate(5,5);
    }

    public void Generate(int n, int m)
    {
        maze = new Maze(n, m);
    }
}
