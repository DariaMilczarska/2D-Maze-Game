using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    private int maxWidth { get; set;}
    private int maxHeight { get; set; }

    private Room[,] mazeMatrix;

    public Maze(int x, int y)
    {
        maxWidth = x;
        maxHeight = y;

        mazeMatrix = new Room[maxWidth, maxHeight];
        for (int i = 0; i < maxWidth; ++i)
        {
            for (int j = 0; j < maxHeight; ++j)
            {
                mazeMatrix[i, j] = new Room(i, j);
            }
        }
    }
}
