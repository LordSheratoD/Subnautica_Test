using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VisualHelperESP.MapHelper;

public class MiniMapController : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public float mapScale = 5.0f; // Escala del mapa, controla el rango
    public Color locationColor = Color.red; // Color para las ubicaciones

    private List<Location> locationsToShow; // Lista para almacenar las ubicaciones a mostrar

    private void Awake()
    {
        // Inicializa tu lista de ubicaciones aquí, por ejemplo:
        locationsToShow = new List<Location>();
        // Añade ubicaciones a locationsToShow según tu lógica
    }

    private void OnGUI()
    {
        if (SceneManager.GetActiveScene().name == "Main" && Player.main != null && Camera.main != null)
        {
            player = Player.main.transform;
            // Suponiendo que GetNearLocations devuelve una lista de ubicaciones
            locationsToShow = Ubications.GetNearLocations(player.position, 100);

            var mapWidth = 70;
            var mapHeight = 70;
            var offsetX = Screen.width - mapWidth - 20;
            var offsetY = 20;

            // Guarda la matriz GUI actual
            var oldMatrix = GUI.matrix;

            // Mueve y escala el GUI para centrar el mini mapa en el jugador
            GUI.matrix = Matrix4x4.TRS(new Vector3(offsetX, offsetY, 0), Quaternion.identity, new Vector3(mapScale, mapScale, 1));

            // Dibuja el fondo del mini mapa
            GUI.Box(new Rect(-mapWidth / 2, -mapHeight / 2, mapWidth, mapHeight), "");

            // Dibuja el jugador en el centro del mini mapa como un círculo
            GUI.color = Color.green; // Color del jugador
            GUI.Box(new Rect(0, 0, 10, 10), "P"); // Ajusta según el tamaño deseado para el jugador
            GUI.color = Color.white; // Restablece el color por defecto

            // Dibuja las ubicaciones en el minimapa
            foreach (var location in locationsToShow)
            {
                Vector2 locationPos = WorldToMapPosition(location.position);
                GUI.color = locationColor; // Color para las ubicaciones
                GUI.Box(new Rect(locationPos.x - 3, locationPos.y - 3, 6, 6), ""); // Ajusta según el tamaño deseado para las ubicaciones
            }

            GUI.color = Color.white; // Restablece el color por defecto

            // Restaura la matriz GUI original
            GUI.matrix = oldMatrix;
        }
    }

    // Convierte la posición del mundo a la posición del minimapa
    private Vector2 WorldToMapPosition(Vector3 worldPosition)
    {
        // Calcula la posición relativa de la ubicación al jugador
        Vector3 relativePosition = worldPosition - player.position;

        // Ajusta el valor relativo al escalar del minimapa
        // Nota: Puedes necesitar ajustar estos valores dependiendo de cómo quieras que se mapee el rango del mundo al tamaño del minimapa
        float minimapPosX = (relativePosition.x / mapScale) * (70 / 2); // 70 es el ancho y alto del minimapa como se define en OnGUI
        float minimapPosY = (relativePosition.z / mapScale) * (70 / 2); // Usamos 'z' porque en 3D, 'y' es hacia arriba, pero en el minimapa, 'z' es nuestra 'y'

        // Calcula la posición final en la GUI, ajustando para que el centro del jugador esté en el centro del minimapa
        // Asume que el minimapa se dibuja desde el centro, por lo tanto, ajusta por la mitad del ancho/alto del minimapa
        Vector2 mapPosition = new Vector2(minimapPosX + Screen.width - 70 - 20, minimapPosY + 20);

        return mapPosition;
    }


    // Método para cambiar el rango del minimapa
    public void AdjustMapScale(float newScale)
    {
        mapScale = newScale;
    }
}
