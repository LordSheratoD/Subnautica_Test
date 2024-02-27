using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.MapHelper;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceFinderController : MonoBehaviour
{
    private List<ResourceTracker> _resourceTrackers = new List<ResourceTracker>();
    private float maxDistance = 25f;  
    private float updateInterval = 1f;
    private float lastUpdateTime = 0f; // Control de tiempo para actualización

    private void Start()
    {
        UpdateResources(); // Inicializa la lista en el Start, no en Awake para asegurarse de que todos los objetos estén cargados
    }
    
    private void Update()
    {
        // Actualiza recursos basado en intervalo de tiempo en vez de usar InvokeRepeating
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdateResources();
            lastUpdateTime = Time.time;
        }
    }
    
    private void UpdateResources()
    {
        _resourceTrackers = UnityEngine.Object.FindObjectsOfType<ResourceTracker>()
            .Where(obj => obj != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance)
            .ToList();
    }
    
    public static bool WorldToScreen(Vector3 worldPosition, out Vector3 screenPosition)
    {
        Camera mainCamera = MainCamera.camera; // Asume que usas la cámara principal para la conversión
        screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
    
        // Verifica si el objeto está detrás de la cámara usando el clip space z
        bool isBehind = screenPosition.z < 0;
    
        // Ajusta la posición de la pantalla para que y sea invertida, Unity usa el origen en la esquina inferior izquierda, pero OnGUI lo usa en la superior izquierda
        screenPosition.y = Screen.height - screenPosition.y;
    
        return !isBehind && screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height;
    }

    private static string GetResourceName(ResourceTracker tracker)
    {
        var techType = tracker.techType;
        if (techType != null)
        {
            var resourceName = TechTypeExtensions.Get(Language.main, techType);
            if (!string.IsNullOrEmpty(resourceName))
            {
                return resourceName;
            }
        }

        return "Uknown";
    }

    
    private void OnGUI()
    {
        if (ESP.IsInGame() && _resourceTrackers != null)
        {
            foreach (var resource in _resourceTrackers)
            {
                if (resource.transform == null) continue;
                if (!WorldToScreen(resource.transform.position, out var screen) || ESP.CalculateDistanceToPlayer(resource.transform.position) > maxDistance) continue;

                Render.DrawDistanceString(screen, GetResourceName(resource), new RGBAColor(0, 255, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
            }
        }
    }
}