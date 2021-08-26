using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    private float screenWidth = 16f;

    private float sreenHeight = 10f;

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private GameObject wallPerfab;

    [SerializeField]
    private GameObject roomPrefab;

    private List<Room> invincibleRooms { get; set; } = new List<Room>();

    private List<GameObject> instantiatedWalls = new List<GameObject>();

    void Start()
    {
        maze = new Maze(width, height);
        float wallWidth = screenWidth / width;
        float wallheight = sreenHeight / height;

        DrawGrid(wallWidth, wallheight);
        GenerateInvincibleRooms();
       // AddTunnels();
    }

    private void DrawGrid(float wallWidth, float wallHeight)
    {
        int counter = 0;
        while (counter < 2)
        {
            float currentPositionX = -screenWidth / 2;
            float currentPositionY = sreenHeight / 2;

            for (int i = 0; i <= width + 1; ++i)
            {
                for (int j = 0; j <= height; ++j)
                {
                    InstantiateWall(currentPositionX, currentPositionY, wallWidth, wallHeight, counter);
                    currentPositionY -= wallHeight;
                }
                currentPositionX += wallWidth;
                currentPositionY = sreenHeight / 2;
            }
            counter++;
        }
    }


    private void InstantiateWall(float currentPosX, float currentPosY, float wallWidth, float wallHeight, int counter)
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
        wall.transform.localScale = new Vector2(screenWidth / width, 0.3f);
        wall.transform.position = new Vector2(wall.transform.position.x - wallWidth / 2, wall.transform.position.y - wallHeight / 2);
        wall.transform.Rotate(new Vector3(0, 0, 90));

    }

    private void GenerateInvincibleRooms()
    {
        for(int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                GameObject leftWall = instantiatedWalls[width * height + j*width + i];
                GameObject rightWall = instantiatedWalls[width * height + j * width + i * height];
                GameObject upperWall = instantiatedWalls[j * height + i];
                GameObject lowerWall = instantiatedWalls[j * height + i + 1];
                Room room = new Room(i, j, leftWall, rightWall, upperWall, lowerWall);
                invincibleRooms.Add(room);
            }
        }       
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
