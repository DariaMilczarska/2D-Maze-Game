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
        //DrawGrid();
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

    /*   private void DrawGrid()
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
       }*/

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


    private Room CreateRoom(float currentPositionX, float currentPositionY, int i, int j)
    {
        Coordinates coordinates = new Coordinates(i, j);
        Wall leftWall = GetInstantiatedWall(coordinates, PlacementType.VERTICAL);
        if (leftWall == null)
        {
            leftWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.VERTICAL);
        }
        
        Wall upperWall = GetInstantiatedWall(coordinates, PlacementType.HORIZONTAL);
        if (upperWall == null)
        {
            upperWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.HORIZONTAL);
        }

        Wall rightWall = GetInstantiatedWall(new Coordinates(i + 1, j), PlacementType.VERTICAL);
        if (upperWall == null)
        {
            upperWall = InstantiateWall(currentPositionX + wallSize.width, currentPositionY, PlacementType.VERTICAL);
        }

        Wall lowerWall = GetInstantiatedWall(new Coordinates(i, j + 1), PlacementType.HORIZONTAL);
        if (upperWall == null)
        {
            currentPositionY -= wallSize.height;
            upperWall = InstantiateWall(currentPositionX, currentPositionY, PlacementType.HORIZONTAL);
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
