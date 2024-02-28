using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.MapHelper;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class EnvironmentalObjectManager : MonoBehaviour
{
    public List<Plantable> plantables = new List<Plantable>();
    public List<StoryHandTarget> storyHandTargets = new List<StoryHandTarget>();
    
    private float plantableUpdateInterval = 2f;
    public float plantableMaxDistance = 30f;
    private float storyHandUpdateInterval = 2f;
    public float storyHandMaxDistance = 100f;
    
    private float lastPlantableUpdateTime = 0f;
    private float lastStoryHandUpdateTime = 0f;

    void Update()
    {
        if (ESP.IsInGame())
        {
            if (Time.time - lastPlantableUpdateTime >= plantableUpdateInterval)
            {
                UpdatePlantables();
                lastPlantableUpdateTime = Time.time;
            }
            if (Time.time - lastStoryHandUpdateTime >= storyHandUpdateInterval)
            {
                UpdateStoryHandTargets();
                lastStoryHandUpdateTime = Time.time;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdatePlantables()
    {
        plantables = UnityEngine.Object.FindObjectsOfType<Plantable>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= plantableMaxDistance)
            .ToList();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateStoryHandTargets()
    {
        storyHandTargets = UnityEngine.Object.FindObjectsOfType<StoryHandTarget>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null && ESP.CalculateDistanceToPlayer(obj.transform.position) <= storyHandMaxDistance)
            .ToList();
    }
    
    internal string GetResourceName(Plantable plant)
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
    
    internal string GetResourceName(StoryHandTarget storyHand)
    {
        var resourceName = storyHand.primaryTooltip;
        if (!string.IsNullOrEmpty(resourceName))
        {
            return resourceName;
        }

        return "Uknown";
    }
}