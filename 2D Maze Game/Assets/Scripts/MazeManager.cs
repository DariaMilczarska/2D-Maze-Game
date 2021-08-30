using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    private float screenWidth = 18f;

    private float sreenHeight = 10f;

    private float wallWidth;

    private float wallHeight;

    private float scaleOfWall; 

    [SerializeField]
    private int width;

    [SerializeField]
    private int height;

    [SerializeField]
    private GameObject wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    private Dictionary<Coordinates, Room> invincibleRooms { get; set; } = new Dictionary<Coordinates, Room>();

    private List<GameObject> instantiatedWalls = new List<GameObject>();

    void Start()
    {
        maze = new Maze(width, height);
        wallWidth = screenWidth / width;
        wallHeight = sreenHeight / height;

        scaleOfWall = (float) (width + height) / (float) (width * height);
        DrawGrid();
        GenerateInvincibleRooms();
        AddTunnels();
    }

    private void DrawGrid()
    {
        int vertical = 0;
        int horizontal = 1;
        while (vertical < 2)
        {
            float currentPositionX = -(screenWidth / 2) + wallWidth/2;
            float currentPositionY = sreenHeight / 2;

            for (int i = 0; i < width + vertical; ++i)
            {
                for (int j = 0; j < height + horizontal; ++j)
                {
                    InstantiateWall(currentPositionX, currentPositionY, horizontal);
                    currentPositionY -= wallHeight;
                    if (i == 1 && j == 1)
                    {
                        Debug.Log(currentPositionX + " " + currentPositionY);
                    }
                }
                currentPositionX += wallWidth;
                currentPositionY = sreenHeight / 2;
            }
            horizontal = 0;
            vertical ++;
        }
    }


    private void InstantiateWall(float currentPosX, float currentPosY, int horizontal)
    {
        GameObject wall = Instantiate(wallPerfab, new Vector2(currentPosX, currentPosY), Quaternion.identity);
        wall.transform.parent = transform;
        if (horizontal == 0)
        {
            AdjustVerticalWall(wall, wallWidth, wallHeight);
        }
        else
        {
            wall.transform.localScale = new Vector2(sreenHeight / height, scaleOfWall);
        }
        instantiatedWalls.Add(wall);
    }

    private void AdjustVerticalWall(GameObject wall, float wallWidth, float wallHeight)
    {
        wall.transform.localScale = new Vector2(screenWidth / width, scaleOfWall); 
        wall.transform.position = new Vector2(wall.transform.position.x - wallWidth / 2, wall.transform.position.y - wallHeight / 2);
        wall.transform.Rotate(new Vector3(0, 0, 90));

    }

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenWidth / 2 + wallWidth / 2; ;
        float currentPositionY = sreenHeight / 2;

        for (int i = 0; i < width; ++i)
        {
            for(int j = 0; j < height; ++j)
            {
                Room room = CreateRoom(i, j);
                Room instantiadedRoom = Instantiate(roomPrefab, new Vector2(currentPositionX, currentPositionY - (wallHeight / 2)), Quaternion.identity);           
                instantiadedRoom.transform.localScale = new Vector2(wallWidth, wallHeight);
                instantiadedRoom.Setup(room);
                instantiadedRoom.transform.parent = transform;
                currentPositionY -= wallHeight;
                invincibleRooms.Add(room.coordinates, room);
            }
            currentPositionX += wallWidth;
            currentPositionY = sreenHeight / 2;
        }
    }

    private Room CreateRoom(int i, int j)
    { 
        GameObject leftWall = instantiatedWalls[width * height + j * height + i];
        GameObject rightWall = instantiatedWalls[width * height + j * height + height + i];
        GameObject upperWall = instantiatedWalls[j * height + i];
        GameObject lowerWall = instantiatedWalls[j * height + i + 1];

        if (i == 1 && j == 1)
        {
            Debug.Log("LeftWall" + leftWall.transform.position.x +  " " + leftWall.transform.position.y);
            Debug.Log("RightWall" + rightWall.transform.position.x + " " +  rightWall.transform.position.y);
            Debug.Log("Up" + upperWall.transform.position.x + " " + upperWall.transform.position.y);
            Debug.Log("Dpwn" + lowerWall.transform.position.x + " " + lowerWall.transform.position.y);
        }
        return new Room(i, j, leftWall, rightWall, upperWall, lowerWall);
    }
   
    private void AddTunnels()
    {
        List<KeyValuePair<Coordinates, Directions>> graphRepresentation = maze.graphRepresentation;
        foreach (KeyValuePair <Coordinates, Directions> item in graphRepresentation)
        {
            if (invincibleRooms.TryGetValue(item.Key, out Room room))
            {
                switch (item.Value)
                {
                    case Directions.RIGHT:
                        GameObject wall = room.rightWall;
                        Destroy(wall); break;
                    case Directions.LEFT:
                        wall = room.leftWall;
                        Destroy(wall); break;
                    case Directions.DOWN:
                        wall = room.lowerWall;
                        Destroy(wall); break;
                    case Directions.UP:
                        wall = room.upperWall;
                        Destroy(wall); break;
                }            
            }               
        }
    }
}
