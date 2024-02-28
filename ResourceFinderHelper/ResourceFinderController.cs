using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceFinderController : MonoBehaviour
{
    private ResourceTrackerManager resourceTrackerManager;
    private EnvironmentalObjectManager environmentalObjectManager;

    private void Start()
    {
        // Inicializa ResourceTrackerManager
        resourceTrackerManager = FindObjectOfType<ResourceTrackerManager>();
        if (resourceTrackerManager == null)
        {
            GameObject rtManagerObject = new GameObject("ResourceTrackerManager");
            resourceTrackerManager = rtManagerObject.AddComponent<ResourceTrackerManager>();
        }
        
        // Inicializa EnvironmentalObjectManager
        environmentalObjectManager = FindObjectOfType<EnvironmentalObjectManager>();
        if (environmentalObjectManager == null)
        {
            GameObject envManagerObject = new GameObject("EnvironmentalObjectManager");
            environmentalObjectManager = envManagerObject.AddComponent<EnvironmentalObjectManager>();
        }
    }
    
    private void OnGUI()
    {
        if (ESP.IsInGame())
        {
            foreach (var resource in resourceTrackerManager.resourceTrackers)
            {
                float distance = ESP.CalculateDistanceToPlayer(resource.transform.position);
                if (distance > resourceTrackerManager.maxDistance) continue;
                if (!ESP.WorldToScreen(resource.transform.position, out var screen)) continue;

                Render.DrawDistanceString(screen, resourceTrackerManager.GetResourceName(resource), new RGBAColor(55, 255, 0, 255), distance);
            }

            foreach (var plantable in environmentalObjectManager.plantables)
            {
                float distance = ESP.CalculateDistanceToPlayer(plantable.transform.position);
                if (distance > environmentalObjectManager.plantableMaxDistance) continue;
                if (!ESP.WorldToScreen(plantable.transform.position, out var screen)) continue;

                Render.DrawDistanceString(screen, environmentalObjectManager.GetResourceName(plantable), new RGBAColor(55, 255, 0, 255), distance);
            }

            foreach (var storyHand in environmentalObjectManager.storyHandTargets)
            {
                float distance = ESP.CalculateDistanceToPlayer(storyHand.transform.position);
                if (distance > environmentalObjectManager.storyHandMaxDistance) continue;
                if (!ESP.WorldToScreen(storyHand.transform.position, out var screen)) continue;

                Render.DrawDistanceString(screen, environmentalObjectManager.GetResourceName(storyHand), new RGBAColor(55, 255, 0, 255), distance);
            }
        }
    }
}