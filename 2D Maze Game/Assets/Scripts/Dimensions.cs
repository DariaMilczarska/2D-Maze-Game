using System.Collections;
using System.Collections.Generic;

public class Dimensions
{
    public int width { get; set; }
    public int height { get; set; }

    public Dimensions(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public Dimensions()
    {
    }

    public override bool Equals(object obj)
    {
        return obj is Dimensions dimensions &&
               width == dimensions.width &&
               height == dimensions.height;
    }

    public override int GetHashCode()
    {
        int hashCode = 1263118649;
        hashCode = hashCode * -1521134295 + width.GetHashCode();
        hashCode = hashCode * -1521134295 + height.GetHashCode();
        return hashCode;
    }
}
