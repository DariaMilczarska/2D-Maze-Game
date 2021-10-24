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


    private MazeManager mazeManager;

    private Graph graph;

    public List<Coordinates> shortestPath { get; set; } = new List<Coordinates>();

    public List<Coordinates> playerMovementTrack { get; set; } = new List<Coordinates>();

    private float pathSze;

    private void Start()
    {
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
            Algorithm algorithm = new Algorithm(graph);
            shortestPath = algorithm.shortestPath;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.PauseGame();
        }
    }

    private void ShouldPlayMusic()
    {
        AudioManager audioManager = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>();
        if (audioManager.isPlaying)
        {
            audioManager.Play();
        }
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
        player.transform.localScale = new Vector2(1.5f * size, 1.5f * size);
        treasure.transform.localScale = new Vector2(2 * size, 2 * size);
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
        uiManager.ShowSummary(totalScore);       
    }

    private int CalculatePathPoints()
    {
        int onPathPoints = 0, pathScore = 0, shortestPathCount = shortestPath.Count;
        foreach (Coordinates coordinates in playerMovementTrack)
        {
            if (shortestPath.Contains(coordinates))
            {
                shortestPath.Remove(coordinates);
                onPathPoints++;
            }
        }

        pathScore = 500 - (playerMovementTrack.Count - shortestPathCount) + onPathPoints;
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
        foreach (Coordinates coordinates in shortestPath)
        {
            if (mazeManager.invincibleRooms.TryGetValue(coordinates, out Room room))
            {
                GameObject path = Instantiate(shortetsPath, (room.transform.position - new Vector3(mazeManager.scaleOfWall, 0, 0)), Quaternion.identity);
                path.transform.parent = uiManager.transform;
                path.transform.localScale = new Vector2(100 * pathSze, 100 * pathSze);
            }
        }        
    }

    private void ShowPlayerPath()
    {
        foreach (Coordinates coordinates in playerMovementTrack)
        {
            if (mazeManager.invincibleRooms.TryGetValue(coordinates, out Room room))
            {
                GameObject path = Instantiate(playerPath, (room.transform.position + new Vector3(mazeManager.scaleOfWall, 0, 0)), Quaternion.identity);
                path.transform.parent = uiManager.transform;
                path.transform.localScale = new Vector2(100 * pathSze, 100 * pathSze);
            }
        }
    }
}
