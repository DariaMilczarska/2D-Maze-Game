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

    private MazeManager mazeManager;

    private Graph graph;

    public List<Coordinates> shortestPath { get; set; } = new List<Coordinates>();

    public List<Coordinates> playerMovementTrack { get; set; } = new List<Coordinates>();

    private void Start()
    {
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
            player.ResetPosition();
            playerMovementTrack = new List<Coordinates>();
        }
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
        CalculatePathPoints();
        //CalculateTimePoints();
    }

    private float CalculatePathPoints()
    {
        int onPathPoints = 0;
        float score = 0;
        foreach (Coordinates coordinates in playerMovementTrack)
        {
            if (shortestPath.Contains(coordinates))
            {
                onPathPoints++;
            }
        }

        score = 500 - (playerMovementTrack.Count - shortestPath.Count) + onPathPoints;
        if (score < 0)
        {
            score = 0;
        }
        return score;
    }
}