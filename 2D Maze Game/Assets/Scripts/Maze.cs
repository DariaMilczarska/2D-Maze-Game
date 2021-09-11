using System;
using System.Collections.Generic;
using UnityEngine;

public enum Directions
{
    RIGHT,
    LEFT,
    DOWN,
    UP
}

public class Maze 
{
    private System.Random random = new System.Random();
    public int maxWidth { get; set;}
    public int maxHeight { get; set; }
    public Coordinates startCoordinates { get; set; } = new Coordinates(0, 0);
    public Room currentRoom { get; set; }
    public List<KeyValuePair<Room, Directions>> listOfTunnels { get; set; } = new List<KeyValuePair<Room, Directions>>();
    
    private Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y, Dictionary<Coordinates, Room> rooms)
    {
        maxWidth = x;
        maxHeight = y;

        rooms.TryGetValue(startCoordinates, out Room room);
        currentRoom = room;

        GenerateRandomPath();
    }

    private void GenerateRandomPath()
    {
        VisitCurrentRoom(currentRoom);
        while (visitedRooms.Count > 0)
        {
            currentRoom = visitedRooms.Peek();
            Dictionary<Room, Directions> unvisitedNeighbours = GetUnVisitedNeighbours();
            if (unvisitedNeighbours.Count > 0)
            {
                Room room = ChooseRandomNeighbour(unvisitedNeighbours); 
                if(room != null)
                {
                    VisitCurrentRoom(room);
                    currentRoom = room;
                }              
            }
            else
            {
                visitedRooms.Pop();
            }
        }
    }

    public void VisitCurrentRoom(Room room)
    {
        room.VisitRoom();
        visitedRooms.Push(room);
    }

    private Dictionary<Room, Directions> GetUnVisitedNeighbours()
    {
        Dictionary<Room, Directions> neigbourusVisited = new Dictionary<Room, Directions>();
        if (currentRoom.leftRoom != null && currentRoom.leftRoom.visited == false)
        {
            neigbourusVisited.Add(currentRoom.leftRoom, Directions.LEFT);
        }
        if (currentRoom.rightRoom != null && currentRoom.rightRoom.visited == false)
        {
            neigbourusVisited.Add(currentRoom.rightRoom, Directions.RIGHT);
        }
        if (currentRoom.upperRoom != null && currentRoom.upperRoom.visited == false)
        {
            neigbourusVisited.Add(currentRoom.upperRoom, Directions.UP);
        }
        if (currentRoom.lowerRoom != null && currentRoom.lowerRoom.visited == false)
        {
            neigbourusVisited.Add( currentRoom.lowerRoom, Directions.DOWN);
        }
        return neigbourusVisited;
    }

    private Room ChooseRandomNeighbour(Dictionary<Room, Directions> unvisitedNeighbours)
    {
        int randomNeigbourIndex = random.Next(0, unvisitedNeighbours.Count);
        int currentIndex = 0;
        foreach(KeyValuePair<Room, Directions> pair in unvisitedNeighbours)
        {
            if(currentIndex == randomNeigbourIndex)
            {
                listOfTunnels.Add(new KeyValuePair<Room, Directions>(currentRoom, pair.Value));
                return pair.Key;
            }
            currentIndex++;
        }
        return null;
    }
}
