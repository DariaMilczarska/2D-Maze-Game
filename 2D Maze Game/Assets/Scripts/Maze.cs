using System;
using System.Collections.Generic;

public class Maze
{
    private int maxWidth { get; set;}
    private int maxHeight { get; set; }
    public Coordinates treasureCoordinates { get; set; }
    public Coordinates currentCoordinates { get; set; }
    public Coordinates previousCoordinates { get; set; }

    private Room[,] mazeMatrix;
    Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y) //Generates empty maze
    {
        maxWidth = x;
        maxHeight = y;
        treasureCoordinates.coordinateX = maxWidth;
        treasureCoordinates.coordinateY = maxHeight / 2;

        mazeMatrix = new Room[maxWidth, maxHeight];
        for (int i = 0; i < maxWidth; ++i)
        {
            for (int j = 0; j < maxHeight; ++j)
            {
                mazeMatrix[i, j] = new Room(i, j);
            }
        }
    }

    public void GenerateRandomPath()
    {
        VisitCurrentRoom();
        ChooseRandomNeighbour();
        while (!CheckIfAccesible(currentCoordinates)) //Check if neighbour was visited
        {
            currentCoordinates = previousCoordinates;
            ChooseRandomNeighbour();
            //then go to next Neigbour
        }
        if () // no unvisited neigbours
        {
            visitedRooms.Pop();
            return;
        }
        GenerateRandomPath();    
    }

    public bool CheckIfAccesible(Coordinates coordinates)
    {
        if(coordinates.coordinateX < 0 || coordinates.coordinateX == maxWidth)
        {
            return false;
        }
        if (coordinates.coordinateY < 0 || coordinates.coordinateY == maxHeight)
        {
            return false;
        }
        if (mazeMatrix[coordinates.coordinateX, coordinates.coordinateY].visited)
        {
            return false;
        }

        return true;
    }

    public void VisitCurrentRoom() //I have to add another matrix to save which rooms are conected and insert data into it here
    {
        mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY].VisitRoom();
        visitedRooms.Push(mazeMatrix[currentCoordinates.coordinateX, currentCoordinates.coordinateY]);
    }

    public void ChooseRandomNeighbour()
    {
        previousCoordinates = currentCoordinates;
        Random random = new Random();
        int randomX = random.Next(-1, 2);
        int randomY = random.Next(-1, 2);

        currentCoordinates.coordinateX += randomX;
        currentCoordinates.coordinateY += randomY;
    }

    public IDictionary<string, bool> GetUnVisitedNeighbours()
    {
        public IDictionary<string, bool> neigbourusVisited { get; set; } = new Dictionary<string, bool>();
        if(currentPositionX -1 >= 0 && mazeMatrix[currentPositionX - 1, currentPositionY].visited == false)
        {
            return true;
        }
        if (currentPositionX + 1 < maxWidth  && mazeMatrix[currentPositionX + 1, currentPositionY].visited == false)
        {
            return true;
        }
        if (currentPositionX - 1 >= 0 && mazeMatrix[currentPositionX - 1, currentPositionY].visited == false)
        {
            return true;
        }
        if (currentPositionX - 1 >= 0 && mazeMatrix[currentPositionX - 1, currentPositionY].visited == false)
        {
            return true;
        }
    }

}
