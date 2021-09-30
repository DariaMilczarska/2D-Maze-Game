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


    private MazeManager mazeManager;

    private Graph graph;

    public List<Coordinates> shortestPath { get; set; } = new List<Coordinates>();

    public List<Coordinates> playerMovementTrack { get; set; } = new List<Coordinates>();

    private void Start()
    {
        GameObject.FindGameObjectWithTag("Music").GetComponent<AudioManager>().Play();
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
        player.transform.localScale = new Vector2(2 * size, 2 * size);
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
        int pathPoints = CalculatePathPoints();
        int timePoints = CalculateTimePoints();
        int totalScore = pathPoints + timePoints;
        uiManager.ShowSummary(totalScore);
        pathPoints = 0; timePoints = 0; totalScore = 0;
    }

    private int CalculatePathPoints()
    {
        int onPathPoints = 0, score = 0, shortestPathCount = shortestPath.Count;
        foreach (Coordinates coordinates in playerMovementTrack)
        {
            if (shortestPath.Contains(coordinates))
            {
                shortestPath.Remove(coordinates);
                onPathPoints++;
            }
        }

        score = 500 - (playerMovementTrack.Count - shortestPathCount) + onPathPoints;
        if (score < 0)
        {
            score = 0;
        }
        return score;
    }

    private int CalculateTimePoints()
    {
        float score = 1000 / uiManager.time;
        return (int) score;
    }
}
