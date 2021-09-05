using System;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private Graph graph = new Graph();
    public Algorithm(Graph graph)
    {
        this.graph = graph;
    }

    private double GetHeuristicsDistance(Coordinates coordinates)
    {
        double distance  = 0;
        int x = Math.Abs(coordinates.coordinateX - graph.endCoordinates.coordinateX);
        int y = Math.Abs(coordinates.coordinateY - graph.endCoordinates.coordinateY);
        distance = Math.Pow(x, 2) + Math.Pow(y, 2);

        return Math.Sqrt(distance);
    }
}
