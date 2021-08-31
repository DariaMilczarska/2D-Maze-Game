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

    [SerializeField]
    private int gridWidth;

    [SerializeField]
    private int gridHeight;

    [SerializeField]
    private Wall wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    private Dictionary<Coordinates, Room> invincibleRooms { get; set; } = new Dictionary<Coordinates, Room>();

    private List<Wall> instantiatedWalls = new List<Wall>();

    void Start()
    {
        maze = new Maze(gridWidth, gridHeight);
        wallSize = new Dimensions(screenSize.width / (float) gridWidth, screenSize.height / (float) gridHeight);
        scaleOfWall = (float) (gridWidth + gridHeight) / (float) (gridWidth * gridHeight);
        DrawGrid();
        GenerateInvincibleRooms();
        AddTunnels();
    }

    private void DrawGrid()
    {
        foreach(PlacementType type in (PlacementType[])Enum.GetValues(typeof(PlacementType)))
        {
            float currentPositionX = -(screenSize.width / 2f) + wallSize.width / 2f;
            float currentPositionY = screenSize.height / 2f;

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
                    Wall wall = InstantiateWall(currentPositionX, currentPositionY, type);
                    wall.Setup(type, new Coordinates(i, j));
                    currentPositionY -= wallSize.height;
                    instantiatedWalls.Add(wall);
                }
                currentPositionX += wallSize.width;
                currentPositionY = screenSize.height / 2;
            }
        }
    }

    private Wall InstantiateWall(float currentPosX, float currentPosY, PlacementType type)
    {
        Wall wall = Instantiate(wallPerfab, new Vector2(currentPosX, currentPosY), Quaternion.identity);
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

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenSize.width / 2 + wallSize.width / 2; ;
        float currentPositionY = screenSize.height / 2;

        for (int i = 0; i < gridWidth; ++i)
        {
            for(int j = 0; j < gridHeight; ++j)
            {
                Room room = CreateRoom(i, j);
                Room instantiadedRoom = Instantiate(roomPrefab, new Vector2(currentPositionX, currentPositionY - (wallSize.height / 2)), Quaternion.identity);           
                instantiadedRoom.transform.localScale = new Vector2(wallSize.width, wallSize.height);
                instantiadedRoom.Setup(room);
                instantiadedRoom.transform.parent = transform;
                currentPositionY -= wallSize.height;
                invincibleRooms.Add(room.coordinates, room);
            }
            currentPositionX += wallSize.width;
            currentPositionY = screenSize.height / 2;
        }
    }

    private Room CreateRoom(int i, int j)
    {
        Dictionary<Directions, Wall> roomWalls = GetWallsForRoom(new Coordinates(i, j));
        roomWalls.TryGetValue(Directions.LEFT, out Wall leftWall);
        roomWalls.TryGetValue(Directions.RIGHT, out Wall rightWall);
        roomWalls.TryGetValue(Directions.UP, out Wall upperWall);
        roomWalls.TryGetValue(Directions.DOWN, out Wall lowerWall);

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

    private Dictionary<Directions, Wall> GetWallsForRoom(Coordinates coordinates)
    {
        Dictionary<Directions, Wall> placeholder = new Dictionary<Directions, Wall>();
        Coordinates rightWallCoordinates = new Coordinates(coordinates.coordinateX + 1, coordinates.coordinateY);
        Coordinates lowerWallCoordinates = new Coordinates(coordinates.coordinateX, coordinates.coordinateY + 1);

        foreach (Wall wall in instantiatedWalls)
        {
            if (wall.coordinates.Equals(coordinates))
            {
                if(wall.type == PlacementType.HORIZONTAL)
                {
                    placeholder.Add(Directions.UP, wall);
                }
                else
                {
                    placeholder.Add(Directions.LEFT, wall);
                }             
            }
            else if (wall.coordinates.Equals(rightWallCoordinates) && wall.type == PlacementType.VERTICAL)
            {
                placeholder.Add(Directions.RIGHT, wall);
            }
            else if (wall.coordinates.Equals(lowerWallCoordinates) && wall.type == PlacementType.HORIZONTAL)
            {
                placeholder.Add(Directions.DOWN, wall);
            }
            if(placeholder.Count == 4)
            {
                break;
            }
        }
        return placeholder;
    }
}
