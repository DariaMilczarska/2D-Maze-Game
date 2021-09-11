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

    private readonly Dimensions screenSize = new Dimensions(17.7f, 10);

    private Dimensions wallSize;

    private float scaleOfWall;

    private GameManager gameManager;

    [SerializeField]
    private int gridWidth;

    [SerializeField]
    private int gridHeight;

    [SerializeField]
    private Wall wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    public Dictionary<Coordinates, Room> invincibleRooms { get; set; } = new Dictionary<Coordinates, Room>();

    private List<Wall> instantiatedWalls = new List<Wall>();

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        maze = new Maze(gridWidth, gridHeight);
        wallSize = new Dimensions(screenSize.width / (float) gridWidth, screenSize.height / (float) gridHeight);

        scaleOfWall = (float) (gridWidth + gridHeight) / (float) (gridWidth * gridHeight);
        GenerateMaze();
    }

    private void GenerateMaze()
    {
        GenerateInvincibleRooms();
        AddTunnels();
        gameManager.SetUpGame(scaleOfWall, FindStartRoom(), FindTreasureRoom());
    }

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenSize.width / 2 + wallSize.width / 2; ;
        float currentPositionY = screenSize.height / 2;

        for (int i = 0; i < gridWidth; ++i)
        {
            for (int j = 0; j < gridHeight; ++j)
            {
                Room room = CreateRoom(currentPositionX, currentPositionY, i, j);
                Room instantiadedRoom = Instantiate(roomPrefab, new Vector2(currentPositionX, currentPositionY - (wallSize.height / 2)), Quaternion.identity);
                instantiadedRoom.transform.localScale = new Vector2(wallSize.width, wallSize.height);
                instantiadedRoom.Setup(room);
                instantiadedRoom.transform.parent = transform;
                currentPositionY -= wallSize.height;
                invincibleRooms.Add(instantiadedRoom.coordinates, instantiadedRoom);
            }
            currentPositionX += wallSize.width;
            currentPositionY = screenSize.height / 2;
        }
    }

    private Wall InstantiateWall(float currentPosX, float currentPosY, PlacementType type)
    {
        Wall wall = Instantiate(wallPerfab, new Vector2(currentPosX, currentPosY), Quaternion.identity);
        instantiatedWalls.Add(wall);
        wall.transform.parent = transform;
        if (type == PlacementType.VERTICAL)
        {
            AdjustVerticalWall(wall);
        }
        else
        {
            wall.transform.localScale = new Vector2(wallSize.width, scaleOfWall);
        }
        return wall;
    }

    private void AdjustVerticalWall(Wall wall)
    {
        wall.transform.localScale = new Vector2(wallSize.height, scaleOfWall); 
        wall.transform.position = new Vector2(wall.transform.position.x - wallSize.width / 2, wall.transform.position.y - wallSize.height / 2);
        wall.transform.Rotate(new Vector3(0, 0, 90));
    }

    private Room CreateRoom(float currentPositionX, float currentPositionY, int i, int j)
    {
        Coordinates coordinates = new Coordinates(i, j);
        Wall leftWall = GetInstantiatedWall(coordinates, PlacementType.VERTICAL);
        if (leftWall == null)
        {
            leftWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.VERTICAL);
            leftWall.Setup(coordinates, PlacementType.VERTICAL);
        }
        
        Wall upperWall = GetInstantiatedWall(coordinates, PlacementType.HORIZONTAL);
        if (upperWall == null)
        {
            upperWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.HORIZONTAL);
            upperWall.Setup(coordinates, PlacementType.HORIZONTAL);
        }

        Wall rightWall = GetInstantiatedWall(new Coordinates(i + 1, j), PlacementType.VERTICAL);
        if (rightWall == null)
        {
            rightWall = InstantiateWall(currentPositionX + wallSize.width, currentPositionY, PlacementType.VERTICAL);
            rightWall.Setup(new Coordinates(i + 1, j), PlacementType.VERTICAL);
        }

        Wall lowerWall = GetInstantiatedWall(new Coordinates(i, j + 1), PlacementType.HORIZONTAL);
        if (lowerWall == null)
        {
            currentPositionY -= wallSize.height;
            lowerWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.HORIZONTAL);
            lowerWall.Setup(new Coordinates(i, j + 1), PlacementType.HORIZONTAL);

        }
        return new Room(i, j, leftWall, rightWall, upperWall, lowerWall);
    }
   
    private Wall GetInstantiatedWall(Coordinates coordinates, PlacementType placementType)
    {      
        foreach(Wall w in instantiatedWalls)
        {
            if (w.coordinates.Equals(coordinates) && w.type.Equals(placementType))
            {
                return w;
            }
        }
        return null;
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
                        Wall wall = room.rightWall;
                        Destroy(wall.gameObject); break;
                    case Directions.LEFT:
                        wall = room.leftWall;
                        Destroy(wall.gameObject); break;
                    case Directions.DOWN:
                        wall = room.lowerWall;
                        Destroy(wall.gameObject); break;
                    case Directions.UP:
                        wall = room.upperWall;
                        Destroy(wall.gameObject); break;
                }            
            }               
        }
    }

    public Transform FindTreasureRoom()
    {
        Coordinates coordinates = new Coordinates(gridWidth - 1, gridHeight - 1);
        if (invincibleRooms.TryGetValue(coordinates, out Room room))
        {
            return room.transform;
        }
        return null;
    }

    public Transform FindStartRoom()
    {
        Coordinates coordinates = new Coordinates(0, 0);
        if (invincibleRooms.TryGetValue(coordinates, out Room room))
        {
            return room.transform;
        }
        return null;
    }
}
