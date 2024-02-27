using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace VisualHelperESP.MapHelper
{
    public class Ubications
    {
        internal static List<Location> locations = new List<Location>();

        // Constructor para inicializar con algunas ubicaciones de ejemplo
        public static void InitializeLocations()
        {
            // Añade ubicaciones de ejemplo aquí, esto se llamará explícitamente para llenar la lista.
            locations.Add(new Location(LocType.Communications.PDA, "This was just mean", new Vector3(-215, -122, -285)));
            locations.Add(new Location(LocType.Tech.Fragment, "Coral", new Vector3(-216, -19, 50)));
            // Continúa añadiendo otras ubicaciones...
        }
        
        public static List<Location> GetLocationsByType(object type)
        {
            return locations.Where(loc => loc.type.Equals(type)).ToList();
        }

        public static List<Location> GetLocationsByName(string name)
        {
            return locations.Where(loc => loc.name.Contains(name)).ToList();
        }

        public static List<Location> GetNearLocations(Vector3 position, float meters)
        {
            return locations.Where(loc => Vector3.Distance(loc.position, position) <= meters).ToList();
        }
    }
}