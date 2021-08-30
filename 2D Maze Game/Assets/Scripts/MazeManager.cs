using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementType
{  
    VERTICAL,
    HORIZONTAL
}

public class MazeManager : MonoBehaviour
{

    private Maze maze;

    private readonly Dimensions screenSize = new Dimensions(18, 10);

    private Dimensions wallSize;

    private float scaleOfWall; 

    [SerializeField]
    private int gridWidth;

    [SerializeField]
    private int gridHeight;

    [SerializeField]
    private GameObject wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    private Dictionary<Coordinates, Room> invincibleRooms { get; set; } = new Dictionary<Coordinates, Room>();

    private List<GameObject> instantiatedWalls = new List<GameObject>();

    void Start()
    {
        maze = new Maze(gridWidth, gridHeight);
        wallSize = new Dimensions(screenSize.width / gridWidth, screenSize.height / gridHeight);

        scaleOfWall = (float) (gridWidth + gridHeight) / (float) (gridWidth * gridHeight);
        DrawGrid();
        //GenerateInvincibleRooms();
        //AddTunnels();
    }

    private void DrawGrid()
    {
        foreach(PlacementType type in (PlacementType[])Enum.GetValues(typeof(PlacementType)))
        {
            float currentPositionX = -(screenSize.width / 2) + wallSize.width / 2;
            float currentPositionY = screenSize.height / 2;

            int horizontalGridLines = 0, verticalGridLines = 0;

            switch (type)
            {
                case PlacementType.HORIZONTAL:
                    horizontalGridLines =  gridWidth;
                    verticalGridLines = gridHeight + 1; break;

                case PlacementType.VERTICAL:
                    horizontalGridLines = gridWidth + 1;
                    verticalGridLines = gridHeight; break;
            }

            for (int i = 0; i < horizontalGridLines; ++i)
            {
                for (int j = 0; j < verticalGridLines; ++j)
                {
                    GameObject wall = InstantiateWall(currentPositionX, currentPositionY, type);
                    currentPositionY -= wallSize.height;
                    instantiatedWalls.Add(wall);
                    if (i == 1 && j == 1)
                    {
                        Debug.Log(currentPositionX + " " + currentPositionY);
                    }
                }
                currentPositionX += wallSize.width;
                currentPositionY = screenSize.height / 2;
            }
        }
    }


    private GameObject InstantiateWall(float currentPosX, float currentPosY, PlacementType type)
    {
        GameObject wall = Instantiate(wallPerfab, new Vector2(currentPosX, currentPosY), Quaternion.identity);
        wall.transform.parent = transform;
        if (type == PlacementType.VERTICAL)
        {
            AdjustVerticalWall(wall);
        }
        else
        {
            wall.transform.localScale = new Vector2(wallSize.height, scaleOfWall);
        }
        return wall;
    }

    private void AdjustVerticalWall(GameObject wall)
    {
        wall.transform.localScale = new Vector2(wallSize.width, scaleOfWall); 
        wall.transform.position = new Vector2(wall.transform.position.x - wallSize.width / 2, wall.transform.position.y - wallSize.height / 2);
        wall.transform.Rotate(new Vector3(0, 0, 90));

    }

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenSize.width / 2 + wallSize.width / 2; ;
        float currentPositionY = screenSize.height / 2;

        for (int i = 0; i < gridWidth; ++i)
        {
            for(int j = 0; j < gridHeight; ++j)
            {
                //Room room = CreateRoom(i, j);
                Room instantiadedRoom = Instantiate(roomPrefab, new Vector2(currentPositionX, currentPositionY - (wallSize.height / 2)), Quaternion.identity);           
                instantiadedRoom.transform.localScale = new Vector2(wallSize.width, wallSize.height);
                //instantiadedRoom.Setup(room);
                instantiadedRoom.transform.parent = transform;
                currentPositionY -= wallSize.height;
                //invincibleRooms.Add(room.coordinates, room);
            }
            currentPositionX += wallSize.width;
            currentPositionY = screenSize.height / 2;
        }
    }

   /* private Room CreateRoom(int i, int j)
    { 
        GameObject leftWall = instantiatedWalls[(width + 1) * (height + 1) + j * height + i + 1]; //Needs change (too hard to follow)
        GameObject rightWall = instantiatedWalls[(width + 1) * (height + 1) + j * height + height + i + 2];
        GameObject upperWall = instantiatedWalls[j * height + i + 1];
        GameObject lowerWall = instantiatedWalls[j * height + i + 2];

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
    }*/
}
