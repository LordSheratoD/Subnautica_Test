using UnityEngine;

namespace VisualHelperESP.MapHelper;

public class Location
{
    public string name;
    public Vector3 position;
    public object type; // Puede ser de cualquier enum en LocType

    public Location(object type, string name, Vector3 position)
    {
        this.type = type;
        this.name = name;
        this.position = position;
    }
}