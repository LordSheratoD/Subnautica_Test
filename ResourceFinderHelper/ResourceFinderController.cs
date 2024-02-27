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
    private List<Plantable> _plantable = new List<Plantable>();
    private List<StoryHandTarget> _storyHand = new List<StoryHandTarget>();
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
        if (ESP.IsInGame())
        {
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                UpdateResources();
                lastUpdateTime = Time.time;
            }
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateResources()
    {
        /*_plantable = UnityEngine.Object.FindObjectsOfType<Plantable>()
            .Where(obj => obj != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance)
            .ToList();*/
        _storyHand = UnityEngine.Object.FindObjectsOfType<StoryHandTarget>()
            .Where(obj => obj != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance)
            .ToList();
        /*_resourceTrackers = UnityEngine.Object.FindObjectsOfType<ResourceTracker>()
            .Where(obj => obj != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance)
                                  .ToList();*/
    }
    
    public static bool WorldToScreen(Vector3 worldPosition, out Vector3 screenPosition)
    {
        Camera mainCamera = MainCameraV2.main.cam; // Asume que usas la cámara principal para la conversión
        screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
    
        // Verifica si el objeto está detrás de la cámara usando el clip space z
        bool isBehind = screenPosition.z < 0;
    
        // Ajusta la posición de la pantalla para que y sea invertida, Unity usa el origen en la esquina inferior izquierda, pero OnGUI lo usa en la superior izquierda
        screenPosition.y = Screen.height - screenPosition.y;
    
        return !isBehind && screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 && screenPosition.y <= Screen.height;
    }

    private static string GetResourceName(Plantable plant)
    {
        var techType = plant.plantTechType;
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

    private static string GetResourceName(StoryHandTarget storyHand)
    {
        var resourceName = storyHand.primaryTooltip;
        if (!string.IsNullOrEmpty(resourceName))
        {
            return resourceName;
        }

        return "Uknown";
    }

    
    private void OnGUI()
    {
        if (ESP.IsInGame() && _resourceTrackers != null)
        {
            /*foreach (var resource in _resourceTrackers)
            {
                // Added null check for resource itself
                if (resource == null || resource.transform == null) continue;
                if (resource.enabled == false) continue;
                if (!WorldToScreen(resource.transform.position, out var screen) || ESP.CalculateDistanceToPlayer(resource.transform.position) > maxDistance) continue;

                Render.DrawDistanceString(screen, GetResourceName(resource), new RGBAColor(0, 255, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
            }*/
            foreach (var resource in _storyHand)
            {
                // Added null check for resource itself
                if (resource == null || resource.transform == null) continue;
                if (resource.enabled == false) continue;
                if (!WorldToScreen(resource.transform.position, out var screen) || ESP.CalculateDistanceToPlayer(resource.transform.position) > maxDistance) continue;

                Render.DrawDistanceString(screen, GetResourceName(resource), new RGBAColor(0, 255, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
            }
            /*foreach (var resource in _plantable)
            {
                // Added null check for resource itself
                if (resource == null || resource.transform == null) continue;
                if (!WorldToScreen(resource.transform.position, out var screen) || ESP.CalculateDistanceToPlayer(resource.transform.position) > maxDistance) continue;

                Render.DrawDistanceString(screen, GetResourceName(resource), new RGBAColor(255, 255, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
            }*/
            /*foreach (var resource in _storyHand)
            {
                // Added null check for resource itself
                if (resource == null || resource.transform == null) continue;
                if (!WorldToScreen(resource.transform.position, out var screen) || ESP.CalculateDistanceToPlayer(resource.transform.position) > maxDistance) continue;

                Render.DrawDistanceString(screen, GetResourceName(resource), new RGBAColor(255, 255, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
            }*/
        }
        else
        {
            if (_resourceTrackers != null) _resourceTrackers.Clear();
            if (_storyHand != null) _storyHand.Clear();
        }
    }

}