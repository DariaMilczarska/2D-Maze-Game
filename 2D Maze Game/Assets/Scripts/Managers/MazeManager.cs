using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlacementType
{  
    VERTICAL,
    HORIZONTAL
}

public class MazeManager : MonoBehaviour
{
    public Maze maze { get; set; }

    private readonly Dimensions screenSize = new Dimensions(17.7f, 10);

    private Dimensions wallSize;

    private Dimensions wallScale;

    private Dimensions gridDimensions = LevelParameters.gridDimensions;

    public Coordinates treasureCoordinates { get; set; }

    [SerializeField]
    public float scaleOfWall { get; set; }  

    [SerializeField]
    private Wall wallPerfab;

    [SerializeField]
    private Room roomPrefab;

    public Dictionary<Coordinates, Room> invincibleRooms { get; set; } = new Dictionary<Coordinates, Room>();

    private List<Wall> instantiatedWalls = new List<Wall>();

    void Start()
    {
        treasureCoordinates = new Coordinates((int) gridDimensions.width - 1, (int) gridDimensions.height - 1);
        wallSize = new Dimensions(screenSize.width / gridDimensions.width, screenSize.height / gridDimensions.height);
        wallScale = new Dimensions((screenSize.width / 4.2f) / gridDimensions.width, (screenSize.height / 4.2f) / gridDimensions.height);
        scaleOfWall = gridDimensions.height / (gridDimensions.height * gridDimensions.width);
    }

    public void GenerateMaze()
    {
        GenerateInvincibleRooms();
        AdjustNeigbours();
        maze = new Maze((int) gridDimensions.width, (int) gridDimensions.height, invincibleRooms);
        AddTunnels();
        AddRandomPaths();
    }

    private void GenerateInvincibleRooms()
    {
        float currentPositionX = -screenSize.width / 2 + wallSize.width / 2; ;
        float currentPositionY = screenSize.height / 2;

        for (int i = 0; i < gridDimensions.width; ++i)
        {
            for (int j = 0; j < gridDimensions.height; ++j)
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
            wall.transform.localScale = new Vector2(wallScale.width, scaleOfWall);
        }
        return wall;
    }

    private void AdjustVerticalWall(Wall wall)
    {
        wall.transform.localScale = new Vector2(wallScale.height, scaleOfWall); 
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
        List<KeyValuePair<Room, Directions>> listOfTunnels = maze.listOfTunnels;
        foreach (KeyValuePair <Room, Directions> item in listOfTunnels)
        {
            if (invincibleRooms.TryGetValue(item.Key.coordinates, out Room room))
            {
                switch (item.Value)
                {
                    case Directions.RIGHT:
                        RemoveWall(room.rightWall); break;
                    case Directions.LEFT:
                        RemoveWall(room.leftWall); break;
                    case Directions.DOWN:
                        RemoveWall(room.lowerWall); break;
                    case Directions.UP:
                        RemoveWall(room.upperWall); break;
                }            
            }               
        }
    }

    public Transform FindTreasureRoom()
    {
        if (invincibleRooms.TryGetValue(treasureCoordinates, out Room room))
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

    private void RemoveWall(Wall wall)
    {
        instantiatedWalls.Remove(wall);
        Destroy(wall.gameObject); 
    }

    private void AddRandomPaths()
    {
        int deletedWalls = 0, numberOfWallsToDelete = (int) (gridDimensions.height + gridDimensions.width) / (2*2);
        System.Random random = new System.Random();

        while(deletedWalls < numberOfWallsToDelete)
        {
            int wallIndex = random.Next(0, instantiatedWalls.Count);
            Coordinates wallCoordinates = instantiatedWalls[wallIndex].coordinates;
            if (wallCoordinates.coordinateX > 0 && wallCoordinates.coordinateX < gridDimensions.width - 1)
            {
                if (wallCoordinates.coordinateY > 0 && wallCoordinates.coordinateY < gridDimensions.height - 1)
                {
                    invincibleRooms.TryGetValue(wallCoordinates, out Room room);
                    if (instantiatedWalls[wallIndex].type == PlacementType.HORIZONTAL)
                    {
                        maze.listOfTunnels.Add(new KeyValuePair<Room, Directions>(room, Directions.UP));
                    }
                    else
                    {
                        maze.listOfTunnels.Add(new KeyValuePair<Room, Directions>(room, Directions.LEFT));
                    }                 
                    deletedWalls++;
                    RemoveWall(instantiatedWalls[wallIndex]);
                }
            }
        }
    }

    private void AdjustNeigbours()
    {
        foreach(KeyValuePair<Coordinates, Room> room in invincibleRooms)
        {
            Room leftRoom = null, rightRoom = null, upperRoom = null, lowerRoom = null;
            if(room.Key.coordinateX > 0)
            {               
                invincibleRooms.TryGetValue(new Coordinates(room.Key.coordinateX - 1, room.Key.coordinateY), out leftRoom);
            }
            if (room.Key.coordinateX < gridDimensions.width - 1)
            {                
                invincibleRooms.TryGetValue(new Coordinates(room.Key.coordinateX + 1, room.Key.coordinateY), out rightRoom);
            }
            if (room.Key.coordinateY > 0)
            {              
                invincibleRooms.TryGetValue(new Coordinates(room.Key.coordinateX, room.Key.coordinateY - 1), out upperRoom);
            }
            if(room.Key.coordinateY < gridDimensions.height - 1)
            {              
                invincibleRooms.TryGetValue(new Coordinates(room.Key.coordinateX, room.Key.coordinateY + 1), out lowerRoom);
            }       
            room.Value.AdjustNeighbours(leftRoom, rightRoom, upperRoom, lowerRoom);
        }      
    }
}
