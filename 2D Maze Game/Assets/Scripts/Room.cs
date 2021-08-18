using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public int coordinateX { get; set; }

    public int coordinateY { get; set; }
    public bool visited { get; set; }

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
