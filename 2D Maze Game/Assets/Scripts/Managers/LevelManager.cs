using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private Treasure treasure;

    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private GameObject playerPath;

    [SerializeField]
    private GameObject shortetsPath;

    [SerializeField]
    private GameObject coin;

    [SerializeField]
    private GameObject coinSound;

    [SerializeField]
    private GameObject lineRenderer;

    private MazeManager mazeManager;

    private Graph graph;
    public List<Coordinates> shortestPath { get; set; } = new List<Coordinates>();
    public List<Coordinates> playerMovementTrack { get; set; } = new List<Coordinates>();

    private AudioManager audioManager;

    private float pathSze;

    private int collectedCoins = 0;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
        ShouldPlayMusic();   
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        mazeManager = GameObject.Find("MazeManager").GetComponent<MazeManager>();
        if (mazeManager != null)
        {
            mazeManager.GenerateMaze();
            Transform playerPosition = mazeManager.FindStartRoom();
            Transform treasurePosition = mazeManager.FindTreasureRoom();
            SetUpGame(mazeManager.scaleOfWall, playerPosition, treasurePosition);
            graph = new Graph(mazeManager.maze.startCoordinates, mazeManager.treasureCoordinates);
            graph.TransformIntoGraph(mazeManager.maze.listOfTunnels);
            DetermineShortestPath();
            GenerateCoins();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audioManager.isPlaying)
            {
                Mute();
            }
            else
            {
                UnMute();
            }
        }

        if ((Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) && !uiManager.levelFinished)
        {
            if (uiManager.pausePanel.activeInHierarchy)
            {
                uiManager.ResumeGame();
            }
            else
            {
                uiManager.PauseGame();
            }          
        }
    }

    private void DetermineShortestPath()
    {
        Algorithm algorithm = new Algorithm(graph);
        Dijkstra dijkstra = new Dijkstra(graph);
        if (dijkstra.shortestPath.Count < algorithm.shortestPath.Count)
        {
            shortestPath = dijkstra.shortestPath;
        }
        else
        {
            shortestPath = algorithm.shortestPath;
        }
    }

    private void ShouldPlayMusic()
    {
        
        if (audioManager.isPlaying)
        {
            audioManager.Play();
        }
    }

    private void Mute()
    {
        audioManager.Stop();
    }

    private void UnMute()
    {
        audioManager.Play();
    }

    public void RestartLevel()
    {
        player.ResetPosition();
        playerMovementTrack = new List<Coordinates>();
    }

    public void SetUpGame(float size, Transform playerPosition, Transform treasurePosition)
    {
        SetUpComponentsSize(size);
        SetUpPlayerPosition(playerPosition);
        SetUpTreasurePosition(treasurePosition);
    }

    private void SetUpComponentsSize(float size)
    {
        pathSze = size;
        player.transform.localScale = new Vector2(7 * size, 7 * size);
        treasure.transform.localScale = new Vector2(3 * size, 3 * size);
    }

    private void SetUpTreasurePosition(Transform transform)
    {
        if (transform != null)
        {
            treasure.transform.position = transform.position;
        }
    }
    private void SetUpPlayerPosition(Transform transform)
    {
        if (transform != null)
        {
            player.SetUpPosition(transform.position.x, transform.position.y);
            player.transform.position = transform.position;
        }
    }

    public void NewRoomEntered(Coordinates coordinates)
    {
        playerMovementTrack.Add(coordinates);
    }

    public void TreasureHitted()
    {
        uiManager.levelFinished = true;
        ShowShortestPath();
        ShowPlayerPath();
        int pathPoints = CalculatePathPoints();
        int timePoints = CalculateTimePoints();
        int totalScore = pathPoints + timePoints;
        int playerPathLenght = playerMovementTrack.Count;
        if (!playerMovementTrack.Contains(new Coordinates(0,0)))
        {
            playerPathLenght++;
        }
        uiManager.ShowSummary(totalScore, collectedCoins, shortestPath.Count, playerPathLenght);
        collectedCoins = 0;
    }

    private int CalculatePathPoints()
    {
        int pathScore, shortestPathCount = shortestPath.Count;

        pathScore = 500 - (playerMovementTrack.Count - shortestPathCount);
        if (pathScore < 0)
        {
            pathScore = 0;
        }
        return pathScore;
    }

    private int CalculateTimePoints()
    {
        float timeScore = 1000 / uiManager.time;
        return (int) timeScore;
    }

    private void ShowShortestPath()
    {
        List<Transform> points = new List<Transform>();
        Coordinates coordinates = shortestPath[0];
        mazeManager.invincibleRooms.TryGetValue(coordinates, out Room room);       
        points.Add(room.transform);
        for (int i = 0; i < shortestPath.Count - 1; ++i)
        {          
            Coordinates nextRoomCoordinates = shortestPath[i + 1];
            if (mazeManager.invincibleRooms.TryGetValue(nextRoomCoordinates, out Room nextRoom))
            {
                points.Add(nextRoom.transform);
            }
        }
        lineRenderer.GetComponent<LineManager>().CreateShortestPath(points, pathSze);
    }

    private void ShowPlayerPath()
    {
        List<Transform> points = new List<Transform>();
        Coordinates coordinates = new Coordinates(0, 0);
        mazeManager.invincibleRooms.TryGetValue(coordinates, out Room room);
        points.Add(room.transform);
        coordinates = playerMovementTrack[0];
        mazeManager.invincibleRooms.TryGetValue(coordinates, out room);
        points.Add(room.transform);
        for (int i = 0; i < playerMovementTrack.Count - 1; ++i)
        {
            Debug.Log(playerMovementTrack[i + 1].ToString()); ;
            Coordinates nextRoomCoordinates = playerMovementTrack[i + 1];
            if (mazeManager.invincibleRooms.TryGetValue(nextRoomCoordinates, out Room nextRoom))
            {
                if (playerMovementTrack[i].coordinateX.Equals(nextRoom.coordinates.coordinateX) || playerMovementTrack[i].coordinateY.Equals(nextRoom.coordinates.coordinateY))
                {
                    points.Add(nextRoom.transform);
                }
               
            }
        }
        lineRenderer.GetComponent<LineManager>().CreatePlayertPath(points, pathSze);
    }

    public void CollectCoin()
    {
        collectedCoins++;
        if(uiManager.time > 2)
        {
            uiManager.time -= 2;
        }       
        coinSound.GetComponent<AudioSource>().Play();
    }

    private void GenerateCoins()
    {
        float amountOFCoins = LevelParameters.gridDimensions.width * 2;
        System.Random random = new System.Random();
        for(int i = 0; i < amountOFCoins; ++i)
        {
            int randomPosX = random.Next(0, (int) LevelParameters.gridDimensions.width);
            int randomPosY = random.Next(0, (int) LevelParameters.gridDimensions.height);
            Coordinates coordinates = new Coordinates(randomPosX, randomPosY);
            if(coordinates.Equals(new Coordinates(0,0)) || coordinates.Equals(new Coordinates((int) LevelParameters.gridDimensions.width - 1, (int) LevelParameters.gridDimensions.height - 1)))
            {
                continue;
            }
            mazeManager.invincibleRooms.TryGetValue(coordinates, out Room room);
            GameObject instantiatedCoin = Instantiate(coin, room.transform.position, Quaternion.identity);
            instantiatedCoin.transform.localScale = new Vector2(3*pathSze, 3*pathSze);
            instantiatedCoin.transform.parent = this.transform;
        }
    }
}
