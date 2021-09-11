using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Coordinates coordinates { get; set; }
    public bool visited { get; set; }

    public Room leftRoom { get; set; }

    public Room rightRoom { get; set; }

    public Room upperRoom { get; set; }

    public Room lowerRoom { get; set; }

    public Wall leftWall { get; set; }

    public Wall rightWall { get; set; }

    public Wall upperWall { get; set; }

    public Wall lowerWall { get; set; }

    public Room()
    {

    }

    public Room(int x, int y, Wall lfWall, Wall rWall, Wall uWall, Wall loWall)
    {
        coordinates = new Coordinates(x, y);
        leftWall = lfWall;
        rightWall = rWall;
        upperWall = uWall;
        lowerWall = loWall;
    }

    public void AdjustNeighbours(Room lR, Room rR, Room uR, Room loR)
    {
        this.leftRoom = lR;
        this.rightRoom = rR;
        this.lowerRoom = loR;
        this.upperRoom = uR;
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
