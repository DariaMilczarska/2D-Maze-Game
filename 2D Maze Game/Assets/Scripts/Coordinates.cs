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

}
