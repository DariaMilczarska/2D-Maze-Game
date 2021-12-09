using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra 
{
    private Graph graph = new Graph();

    private List<Room> visitedRooms = new List<Room>();

    private PriorityQueue<Room, int> notVisitedRooms = new PriorityQueue<Room, int>();

    private int[] minCostsTable;

    private int[] parentTable;
    public List<Coordinates> shortestPath { get; set; } = new List<Coordinates>();

    public Dijkstra(Graph graph)
    {
        this.graph = graph;
        Room startNode = graph.FindRoomByCoordinates(graph.startCoordinates);
        minCostsTable = new int[graph.graphRepresentation.Count];
        parentTable = new int[graph.graphRepresentation.Count];

        foreach (Room room in graph.graphRepresentation.Keys)
        {
            if (room.Equals(startNode))
            {
                minCostsTable[startNode.id] = 0;
                parentTable[startNode.id] = -1;
                notVisitedRooms.Enqueue(startNode, 0);
            }
            else
            {
                minCostsTable[room.id] = int.MaxValue;
                parentTable[room.id] = -1;
                notVisitedRooms.Enqueue(room, int.MaxValue);
            }        
        }
        StartComputing(graph.startCoordinates, graph.endCoordinates);
    }

    private void StartComputing(Coordinates startCoordinates, Coordinates stopCoordinates)
    {
       while (!notVisitedRooms.IsEmpty())
       {
            Room smallestNode = notVisitedRooms.Dequeue();
            visitedRooms.Add(smallestNode);
            List<Room> neighbours = GetNeighbours(smallestNode);
            foreach(Room neighbour in neighbours)
            {
                if (!visitedRooms.Contains(neighbour) && (minCostsTable[neighbour.id] > minCostsTable[smallestNode.id] + 1))
                {
                    minCostsTable[neighbour.id] = minCostsTable[smallestNode.id] + 1;
                    parentTable[neighbour.id] = smallestNode.id;
                    notVisitedRooms.ChangeValueForKey(neighbour, minCostsTable[neighbour.id]);
                }
            }         
       }
       ReadSolution(graph.FindRoomByCoordinates(graph.endCoordinates));
    }

    private List<Room> GetNeighbours(Room room)
    {
        List<Room> neighbours = new List<Room>();
        graph.graphRepresentation.TryGetValue(room, out neighbours);
        return neighbours;
    }

    public void ReadSolution(Room endRoom)
    {
        while (endRoom != null)
        {
            shortestPath.Insert(0, endRoom.coordinates);
            endRoom = graph.FindRoomByID(parentTable[endRoom.id]);
        }
    }
}
