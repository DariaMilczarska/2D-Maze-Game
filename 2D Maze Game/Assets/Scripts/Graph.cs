using System.Collections;
using System.Collections.Generic;

public class Graph   
{
    public Dictionary<Coordinates, List<Coordinates>> graphRepresentation = new Dictionary<Coordinates, List<Coordinates>>();

    public Coordinates startCoordinates { get; set; }
    public Coordinates endCoordinates { get; set; }

    public Graph(Coordinates startCoordinates, Coordinates endCoordinates)
    {
        this.startCoordinates = startCoordinates;
        this.endCoordinates = endCoordinates;
    }

    public Graph()
    {
    }

    public void TransformIntoGraph(List<KeyValuePair<Coordinates, Directions>> listOfTunnels)
    {
        foreach(KeyValuePair<Coordinates, Directions> element in listOfTunnels)
        {
            Coordinates coordinates = GetCoordinatesFromDirection(element.Key, element.Value);
            if (graphRepresentation.TryGetValue(element.Key, out List<Coordinates> foundCoordinates))
            {
                foundCoordinates.Add(coordinates);
                graphRepresentation.Remove(element.Key);
                graphRepresentation.Add(element.Key, foundCoordinates);
            }
            else
            {
                List<Coordinates> newCoordinates = new List<Coordinates>();
                newCoordinates.Add(coordinates);
                graphRepresentation.Add(element.Key, newCoordinates);
            }
        }
    }  
    private Coordinates GetCoordinatesFromDirection(Coordinates startCoordinates, Directions direction)
    {
        if (direction.Equals(Directions.LEFT))
        {
            return new Coordinates(startCoordinates.coordinateX - 1, startCoordinates.coordinateY); 
        }
        else if (direction.Equals(Directions.UP))
        {
            return new Coordinates(startCoordinates.coordinateX, startCoordinates.coordinateY - 1);
        }
        else if (direction.Equals(Directions.RIGHT))
        {
            return new Coordinates(startCoordinates.coordinateX + 1, startCoordinates.coordinateY);
        }
        else
        {
            return new Coordinates(startCoordinates.coordinateX, startCoordinates.coordinateY + 1);
        }
    }
}
