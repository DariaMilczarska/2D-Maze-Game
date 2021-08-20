using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    Coordinates coordinates;
    public bool visited { get; set; }

    public Room(int x, int y)
    {
        coordinates.coordinateX = x;
        coordinates.coordinateY = y;
    }
    public void VisitRoom()
    {
        visited = true;
    }
}
