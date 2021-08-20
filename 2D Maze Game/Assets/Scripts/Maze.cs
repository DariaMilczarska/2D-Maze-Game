using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze 
{
    private int maxWidth { get; set;}
    private int maxHeight { get; set; }
    public Coordinates treasureCoordinates { get; set; }
    public Coordinates currentCoordinates { get; set; } = new Coordinates(0, 0);

    private Room[,] mazeMatrix;
    private bool [,] graphRepresentation;
    Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y)
    {
        maxWidth = x;
        maxHeight = y;

        mazeMatrix = new Room[maxWidth, maxHeight];
        graphRepresentation = new bool[maxWidth, maxHeight];
        for (int i = 0; i < maxWidth; ++i)
        {
            for (int j = 0; j < maxHeight; ++j)
            {
                mazeMatrix[i, j] = new Room(i, j);
            }
        }

        GenerateRandomPath();
    }

    public void GenerateRandomPath()
    {
        VisitCurrentRoom();
        while (visitedRooms.Count > 0)
        {
            currentCoordinates = visitedRooms.Peek().coordinates;
            IDictionary<int, Coordinates> unvisitedNeighbours = GetUnVisitedNeighbours();
            if (unvisitedNeighbours.Count > 0)
            {
                Coordinates coordinates = ChooseRandomNeighbour(unvisitedNeighbours);
                currentCoordinates = coordinates;
                VisitCurrentRoom();
            }
            else
            {
                visitedRooms.Pop();
            }
        } 
    }

    public void VisitCurrentRoom()
    {
        mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY].VisitRoom();
        visitedRooms.Push(mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY]);
        treasureCoordinates = currentCoordinates;
        graphRepresentation[currentCoordinates.coordinateX, currentCoordinates.coordinateY] = true;
    }

    public Coordinates ChooseRandomNeighbour(IDictionary<int, Coordinates> unvisitedNeighbours)
    {
        System.Random random = new System.Random();
        int randomNeigbourIndex = random.Next(0, unvisitedNeighbours.Count);
        unvisitedNeighbours.TryGetValue(randomNeigbourIndex, out Coordinates coordinates);
        return coordinates;
    }

    public IDictionary<int, Coordinates> GetUnVisitedNeighbours()
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
}
