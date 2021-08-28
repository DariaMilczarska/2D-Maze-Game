using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    private float screenWidth = 16f;

    private float sreenHeight = 10f;

    private float wallWidth;

    private float wallHeight;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private GameObject wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    private List<Room> invincibleRooms { get; set; } = new List<Room>();

    private List<GameObject> instantiatedWalls = new List<GameObject>();

    void Start()
    {
        maze = new Maze(width, height);
        wallWidth = screenWidth / (width - 1);
        wallHeight = sreenHeight / height;

        DrawGrid();
        GenerateInvincibleRooms();
       // AddTunnels();
    }

    private void DrawGrid()
    {
        int counter = 0;
        while (counter < 2)
        {
            float currentPositionX = -screenWidth / 2;
            float currentPositionY = sreenHeight / 2;

            for (int i = 0; i <= width; ++i)
            {
                for (int j = 0; j <= height; ++j)
                {
                    InstantiateWall(currentPositionX, currentPositionY, counter);
                    currentPositionY -= wallHeight;
                }
                currentPositionX += wallWidth;
                currentPositionY = sreenHeight / 2;
            }
            counter++;
        }
    }


    private void InstantiateWall(float currentPosX, float currentPosY, int counter)
    {
        GameObject wall = Instantiate(wallPerfab, new Vector2(currentPosX, currentPosY), Quaternion.identity);
        wall.transform.parent = transform;
        if (counter == 1)
        {
            AdjustVerticalWall(wall, wallWidth, wallHeight);
        }
        else
        {
            wall.transform.localScale = new Vector2(sreenHeight / height, 0.3f);
        }
        instantiatedWalls.Add(wall);
    }

    private void AdjustVerticalWall(GameObject wall, float wallWidth, float wallHeight)
    {
        wall.transform.localScale = new Vector2(screenWidth / width, 0.3f); // add scaling width of wall
        wall.transform.position = new Vector2(wall.transform.position.x - wallWidth / 2, wall.transform.position.y - wallHeight / 2);
        wall.transform.Rotate(new Vector3(0, 0, 90));

    }

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenWidth / 2;
        float currentPositionY = sreenHeight / 2;

        for (int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                Room room = CreateRoom(i, j);
                Room instantiadedRoom = Instantiate(roomPrefab, new Vector2(currentPositionX, currentPositionY - (wallHeight / 2)), Quaternion.identity);           
                instantiadedRoom.transform.localScale = new Vector2(wallWidth, wallHeight);
                instantiadedRoom.Setup(room);
                currentPositionY -= wallHeight;
                invincibleRooms.Add(room);
            }
            currentPositionX += wallWidth;
            currentPositionY = sreenHeight / 2;
        }       
    }

    private Room CreateRoom(int i, int j)
    {
        GameObject leftWall = instantiatedWalls[width * height + j * width + i];
        GameObject rightWall = instantiatedWalls[width * height + j * width + i * height];
        GameObject upperWall = instantiatedWalls[j * height + i];
        GameObject lowerWall = instantiatedWalls[j * height + i + 1];
        return new Room(i, j, leftWall, rightWall, upperWall, lowerWall);
    }
   
    /*private void AddTunnels()
    {
        Coordinates [,] graphRepresentation = maze.graphRepresentation;

        for(int i = 0; i < graphRepresentation.Length; ++i)
        {
            for()
        }
    }*/
}
