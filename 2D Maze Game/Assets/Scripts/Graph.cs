using System.Collections;
using System.Collections.Generic;

public class Graph   
{
    public Dictionary<Room, List<Room>> graphRepresentation = new Dictionary<Room, List<Room>>();

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
