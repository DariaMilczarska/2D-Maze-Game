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

    public void TransformIntoGraph(List<KeyValuePair<Room, Directions>> listOfTunnels)
    {
        foreach(KeyValuePair<Room, Directions> element in listOfTunnels)
        {
            Room room = GetRoomFromDirection(element.Key, element.Value);
            if (graphRepresentation.TryGetValue(element.Key, out List<Room> foundRooms))
            {
                foundRooms.Add(room);
                graphRepresentation.Remove(element.Key);
                graphRepresentation.Add(element.Key, foundRooms);
            }
            else
            {
                List<Room> newNeigbourRoom = new List<Room>();
                newNeigbourRoom.Add(room);
                graphRepresentation.Add(element.Key, newNeigbourRoom);
            }
            SaveOppositeTransition(element.Key, room);
        }
    }  
    private Room GetRoomFromDirection(Room room, Directions direction)
    {
        if (direction.Equals(Directions.LEFT))
        {
            return room.leftRoom; 
        }
        else if (direction.Equals(Directions.UP))
        {
            return room.upperRoom;
        }
        else if (direction.Equals(Directions.RIGHT))
        {
            return room.rightRoom;
        }
        else
        {
            return room.lowerRoom;
        }
    }

    public Room FindRoomByCoordinates(Coordinates coordinates)
    {
        foreach(KeyValuePair<Room, List<Room>> node in graphRepresentation)
        {
            if (node.Key.coordinates.Equals(coordinates))
            {
                return node.Key;
            }
        }
        return null;
    }

    private void SaveOppositeTransition(Room currentRoom, Room newRoom)
    {
        if (graphRepresentation.TryGetValue(newRoom, out List<Room> foundRooms))
        {
            foundRooms.Add(currentRoom);
            graphRepresentation.Remove(newRoom);
            graphRepresentation.Add(newRoom, foundRooms);
        }
        else
        {
            List<Room> newNeigbourRoom = new List<Room>();
            newNeigbourRoom.Add(currentRoom);
            graphRepresentation.Add(newRoom, newNeigbourRoom);
        }
    }
}
