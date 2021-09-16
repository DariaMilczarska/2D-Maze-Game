using System;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private Graph graph = new Graph();

    private PriorityQueue<Room, float> openSet = new PriorityQueue<Room, float>();

    private List<Room> closedSet = new List<Room>();
    public Algorithm(Graph graph)
    {
        this.graph = graph;
        Room startNode = graph.FindRoomByCoordinates(graph.startCoordinates);
        
        if(startNode != null)
        {
            openSet.Enqueue(startNode, 0);
            
        }

        StartComputing(graph.startCoordinates, graph.endCoordinates);
    }

    private double GetHeuristicsDistance(Coordinates coordinates)
    {
        double distance  = 0;
        int x = Math.Abs(coordinates.coordinateX - graph.endCoordinates.coordinateX);
        int y = Math.Abs(coordinates.coordinateY - graph.endCoordinates.coordinateY);
        distance = Math.Pow(x, 2) + Math.Pow(y, 2);

        return Math.Sqrt(distance);
    }

    private void StartComputing(Coordinates startCoordinates, Coordinates stopCoordinates)
    {
        while (!openSet.IsEmpty())
        {
            Room parent = openSet.Dequeue();
            if (parent.coordinates.Equals(stopCoordinates))
            {
                ReadSolution(parent);
                break; //Computing the solution;
            }
            closedSet.Add(parent);
            List<Room> neighbours = GetNeighbours(parent);
            foreach(Room neighbour in neighbours)
            {
                if (closedSet.Contains(neighbour))
                {
                    continue;
                }

                float temp_g_score = parent.g_score + 1;

                if (!openSet.Contains(parent))
                {
                    neighbour.h_score = GetHeuristicsDistance(neighbour.coordinates);
                    AddNodeToPath(parent, neighbour, temp_g_score);
                    openSet.Enqueue(neighbour, (float)neighbour.f_score);
                }
                else if (temp_g_score < neighbour.g_score)
                {
                    AddNodeToPath(parent, neighbour, temp_g_score);
                }           
            }
        }
    }

    private List<Room> GetNeighbours(Room room)
    {
        List<Room> neighbours = new List<Room>();
        graph.graphRepresentation.TryGetValue(room, out neighbours);
        return neighbours;
    }

    private void AddNodeToPath(Room parent, Room neighbour, float temp_g_score)
    {
        neighbour.parent = parent;     
        neighbour.CalculateTotalScore(temp_g_score);
    }

    private void ReadSolution(Room node)
    {
        List<Room> solution = new List<Room>();
        while (node != null)
        {
            solution.Insert(0, node);
            node = node.parent;
        }
    }
}
