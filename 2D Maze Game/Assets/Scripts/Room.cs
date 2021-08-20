using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room 
{
    public Coordinates coordinates { get; set; }
    public bool visited { get; set; }

    public Room(int x, int y)
    {
        coordinates = new Coordinates(x, y);
    }
    public void VisitRoom()
    {
        visited = true;
    }
}
