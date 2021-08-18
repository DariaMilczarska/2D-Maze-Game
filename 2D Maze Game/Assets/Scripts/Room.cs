using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    private int coordinateX { get; set; }

    private int coordinateY { get; set; }
    private bool visited { get; set; }

    public Room(int x, int y)
    {
        coordinateX = x;
        coordinateY = y;
    }

    public void VisitRoom()
    {
        visited = true;
    }
}
