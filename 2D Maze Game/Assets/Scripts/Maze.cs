using System;
using System.Collections.Generic;

public class Maze
{
    private int maxWidth { get; set;}
    private int maxHeight { get; set; }
    private int treasurePositionX { get; set; }
    private int treasurePositionY { get; set; }

    private int currentPositionX = 0;

    private int currentPositionY = 0;


    private Room[,] mazeMatrix;
    Stack<Room> visitedRooms = new Stack<Room>();

    public Maze(int x, int y) //Generates empty maze
    {
        maxWidth = x;
        maxHeight = y;
        treasurePositionX = maxWidth;
        treasurePositionY = maxHeight / 2;

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
        while (CheckIfAccesible(currentPositionX, currentPositionY)) //Check if neighbour was visited
        {
            GenerateRandomPath();
            //then go to next Neigbour
        }

    }

    public bool CheckIfAccesible(int posX, int posY)
    {
        if(currentPositionX < 0 || currentPositionX == maxWidth)
        {
            return false;
        }
        if (currentPositionY < 0 || currentPositionY == maxHeight)
        {
            return false;
        }
        if (mazeMatrix[currentPositionX, currentPositionY].visited)
        {
            return false;
        }

        return true;
    }

    public void VisitCurrentRoom() //I have to add another matrix to save which rooms are conected and insert data into it here
    {
        mazeMatrix[currentPositionX, currentPositionY].VisitRoom();
        visitedRooms.Push(mazeMatrix[currentPositionX, currentPositionY]);
    }

    public void ChooseRandomNeighbour()
    {
        Random random = new Random();
        int randomX = random.Next(-1, 2);
        int randomY = random.Next(-1, 2);

        currentPositionX += randomX;
        currentPositionY += randomY;
    }

}
