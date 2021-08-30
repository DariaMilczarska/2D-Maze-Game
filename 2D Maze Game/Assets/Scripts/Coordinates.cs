using System.Collections;
using System.Collections.Generic;

public class Coordinates 
{
    public int coordinateX { get; set; } = 0;
    public int coordinateY { get; set; } = 0;

    public Coordinates(int cX, int cY)
    {
        coordinateX = cX;
        coordinateY = cY;
    }

    public Coordinates(Coordinates c)
    {
        coordinateX = c.coordinateX;
        coordinateY = c.coordinateY;
    }

    public override bool Equals(object obj)
    {
        return obj is Coordinates coordinates &&
               coordinateX == coordinates.coordinateX &&
               coordinateY == coordinates.coordinateY;
    }

    public override int GetHashCode()
    {
        int hashCode = -1908953177;
        hashCode = hashCode * -1521134295 + coordinateX.GetHashCode();
        hashCode = hashCode * -1521134295 + coordinateY.GetHashCode();
        return hashCode;
    }
}
