using System;
using System.Collections.Generic;
using UnityEngine;

public class Maze 
{
    public int maxWidth { get; set;}
    public int maxHeight { get; set; }
    public Coordinates treasureCoordinates { get; set; }
    public Coordinates currentCoordinates { get; set; } = new Coordinates(0, 0);

    public bool generationFinished { get; set; }

    private Room[,] mazeMatrix;
    private Coordinates [,] graphRepresentation;
    Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y)
    {
        maxWidth = x;
        maxHeight = y;

        mazeMatrix = new Room[maxWidth, maxHeight];

        graphRepresentation = new Coordinates[maxWidth * maxHeight, 4];
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
        VisitCurrentRoom(currentCoordinates);
        while (visitedRooms.Count > 0)
        {
            currentCoordinates = visitedRooms.Peek().coordinates;
            IDictionary<int, Coordinates> unvisitedNeighbours = GetUnVisitedNeighbours();
            if (unvisitedNeighbours.Count > 0)
            {
                Coordinates coordinates = ChooseRandomNeighbour(unvisitedNeighbours); 
                VisitCurrentRoom(coordinates);
                SaveIntoGraphRepresentation(coordinates);
                currentCoordinates = coordinates;
            }
            else
            {
                visitedRooms.Pop();
            }
        }
        generationFinished = true;
    }

    public void VisitCurrentRoom(Coordinates coordinates)
    {
        mazeMatrix[coordinates.coordinateX, coordinates.coordinateY].VisitRoom();
        visitedRooms.Push(mazeMatrix[coordinates.coordinateX, coordinates.coordinateY]);
        treasureCoordinates = coordinates;
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

    public void SaveIntoGraphRepresentation(Coordinates coordinates)
    {
        if(currentCoordinates.coordinateX != coordinates.coordinateX)
        {
            if(currentCoordinates.coordinateX - coordinates.coordinateX > 0)
            {
                graphRepresentation[currentCoordinates.coordinateY * maxWidth + currentCoordinates.coordinateX, 0] = coordinates; //right
            }
            else
            {
                graphRepresentation[currentCoordinates.coordinateY * maxWidth + currentCoordinates.coordinateX, 1] = coordinates; //left
            }
            
        }
        else if (currentCoordinates.coordinateY != coordinates.coordinateY)
        {
            if (currentCoordinates.coordinateY - coordinates.coordinateY > 0)
            {
                graphRepresentation[currentCoordinates.coordinateY * maxWidth + currentCoordinates.coordinateX, 2] = coordinates; //down
            }
            else
            {
                graphRepresentation[currentCoordinates.coordinateY * maxWidth + currentCoordinates.coordinateX, 3] = coordinates; //up
            }
        }

    }
}
