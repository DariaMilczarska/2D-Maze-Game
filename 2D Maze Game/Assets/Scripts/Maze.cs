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
    public Coordinates treasureCoordinates { get; set; }
    public Coordinates currentCoordinates { get; set; } = new Coordinates(0, 0);
    private bool treasurePlaced { get; set; } 

    public List<KeyValuePair<Coordinates, Directions>> listOfTunnels { get; set; } 

    private Room[,] mazeMatrix;
    
    private Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y)
    {
        maxWidth = x;
        maxHeight = y;
        listOfTunnels = new List<KeyValuePair<Coordinates, Directions>>();
        mazeMatrix = new Room[maxWidth, maxHeight];

        for (int i = 0; i < maxWidth; ++i)
        {
            for (int j = 0; j < maxHeight; ++j)
            {
                mazeMatrix[i, j] = new Room(i, j);
            }
        }

        GenerateRandomPath();
    }

    private void GenerateRandomPath()
    {
        VisitCurrentRoom(currentCoordinates);
        while (visitedRooms.Count > 0)
        {
            currentCoordinates = visitedRooms.Peek().coordinates;
            IDictionary<int, Coordinates> unvisitedNeighbours = GetUnVisitedNeighbours();
            if (unvisitedNeighbours.Count > 0)
            {
                Coordinates coordinates = ChooseRandomNeighbour(unvisitedNeighbours); 
                VisitCurrentRoom(coordinates);
                SaveIntoGraphRepresentation(currentCoordinates, coordinates);
                currentCoordinates = coordinates;
            }
            else
            {
                visitedRooms.Pop();
                SetFinishRoom();
            }
        }
    }

    public void VisitCurrentRoom(Coordinates coordinates)
    {
        mazeMatrix[coordinates.coordinateX, coordinates.coordinateY].VisitRoom();
        visitedRooms.Push(mazeMatrix[coordinates.coordinateX, coordinates.coordinateY]);
    }

    private Coordinates ChooseRandomNeighbour(IDictionary<int, Coordinates> unvisitedNeighbours)
    {
        int randomNeigbourIndex = random.Next(0, unvisitedNeighbours.Count);
        unvisitedNeighbours.TryGetValue(randomNeigbourIndex, out Coordinates coordinates);
        return coordinates;
    }

    private IDictionary<int, Coordinates> GetUnVisitedNeighbours()
    {
        int neighbourIndex = 0;
        IDictionary<int, Coordinates> neigbourusVisited = new Dictionary<int, Coordinates>();
        if (currentCoordinates.coordinateX - 1 >= 0 && mazeMatrix[currentCoordinates.coordinateX - 1, currentCoordinates.coordinateY].visited == false)
        {
            neigbourusVisited.Add(neighbourIndex, new Coordinates(currentCoordinates.coordinateX - 1, currentCoordinates.coordinateY));
            neighbourIndex++;
        }
        if (currentCoordinates.coordinateX + 1 < maxWidth && mazeMatrix[currentCoordinates.coordinateX + 1, currentCoordinates.coordinateY].visited == false)
        {
            neigbourusVisited.Add(neighbourIndex, new Coordinates(currentCoordinates.coordinateX + 1, currentCoordinates.coordinateY));
            neighbourIndex++;
        }
        if (currentCoordinates.coordinateY - 1 >= 0 && mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY - 1].visited == false)
        {
            neigbourusVisited.Add(neighbourIndex, new Coordinates(currentCoordinates.coordinateX, currentCoordinates.coordinateY - 1));
            neighbourIndex++;
        }
        if (currentCoordinates.coordinateY + 1 < maxHeight && mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY + 1].visited == false)
        {
            neigbourusVisited.Add(neighbourIndex, new Coordinates(currentCoordinates.coordinateX, currentCoordinates.coordinateY + 1));
            neighbourIndex++;
        }  
        return neigbourusVisited;
    }

    public void SaveIntoGraphRepresentation(Coordinates currentCoordinates, Coordinates coordinates)
    {
        if(currentCoordinates.coordinateX != coordinates.coordinateX)
        {
            if(currentCoordinates.coordinateX - coordinates.coordinateX < 0)
            {
                listOfTunnels.Add(new KeyValuePair<Coordinates, Directions>(currentCoordinates, Directions.RIGHT));
            }
            else
            {
                listOfTunnels.Add(new KeyValuePair<Coordinates, Directions>(currentCoordinates, Directions.LEFT));
            }
            
        }
        else if (currentCoordinates.coordinateY != coordinates.coordinateY)
        {
            if (currentCoordinates.coordinateY - coordinates.coordinateY < 0)
            {
                listOfTunnels.Add(new KeyValuePair<Coordinates, Directions>(currentCoordinates, Directions.DOWN));
            }
            else
            {
                listOfTunnels.Add(new KeyValuePair<Coordinates, Directions>(currentCoordinates, Directions.UP));
            }
        }

    }

    private void SetFinishRoom()
    {
        if (!treasurePlaced)
        {
            treasureCoordinates = currentCoordinates;
            treasurePlaced = true;
        }
    }
}
