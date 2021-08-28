using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Coordinates coordinates { get; set; }
    public bool visited { get; set; }

    public GameObject leftWall { get; set; }

    public GameObject rightWall { get; set; }

    public GameObject upperWall { get; set; }

    public GameObject lowerWall { get; set; }


    public Room(int x, int y)
    {
        coordinates = new Coordinates(x, y);
       
    }

    public Room(int x, int y, GameObject lfWall, GameObject rWall, GameObject uWall, GameObject loWall)
    {
        coordinates = new Coordinates(x, y);
        leftWall = lfWall;
        rightWall = rWall;
        upperWall = uWall;
        lowerWall = loWall;
    }

    public void Setup(Room room)
    {
        coordinates = new Coordinates(room.coordinates);
        leftWall = room.leftWall;
        rightWall = room.rightWall;
        upperWall = room.upperWall;
        lowerWall = room.lowerWall;
    }

    public void VisitRoom()
    {
        visited = true;
    }
}
