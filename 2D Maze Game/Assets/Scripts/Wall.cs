using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public PlacementType type { get; set; }

    public Coordinates coordinates { get; set; }

    public Wall(PlacementType type, Coordinates coordinates)
    {
        this.type = type;
        this.coordinates = coordinates;
    }

    public Wall()
    {
    }

    public void Setup(Coordinates coordinates, PlacementType type)
    {
        this.coordinates = coordinates;
        this.type = type;
    }

    public override bool Equals(object obj)
    {
        return obj is Wall wall &&
               type == wall.type &&
               EqualityComparer<Coordinates>.Default.Equals(coordinates, wall.coordinates);
    }

    public override int GetHashCode()
    {
        int hashCode = 350415997;
        hashCode = hashCode * -1521134295 + type.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Coordinates>.Default.GetHashCode(coordinates);
        return hashCode;
    }
}
