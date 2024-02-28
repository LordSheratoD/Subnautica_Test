using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class EnvironmentalObjectManager : MonoBehaviour
{
    public List<Plantable> plantables = new();
    public List<StoryHandTarget> storyHandTargets = new();
    public float plantableMaxDistance = 30f;
    public float storyHandMaxDistance = 100f;

    private float lastPlantableUpdateTime;
    private float lastStoryHandUpdateTime;

    private readonly float plantableUpdateInterval = 2f;
    private readonly float storyHandUpdateInterval = 2f;

    private void Update()
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
    } // ReSharper disable Unity.PerformanceAnalysis
    private void UpdatePlantables()
    {
        plantables = FindObjectsOfType<Plantable>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= plantableMaxDistance)
            .ToList();
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateStoryHandTargets()
    {
        storyHandTargets = FindObjectsOfType<StoryHandTarget>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= storyHandMaxDistance)
            .ToList();
    }

    internal string GetResourceName(Plantable plant)
    {
        var techType = plant.plantTechType;
        if (techType != null)
        {
            var resourceName = Language.main.Get(techType);
            if (!string.IsNullOrEmpty(resourceName)) return resourceName;
        }

        return "Uknown";
    }

    internal string GetResourceName(StoryHandTarget storyHand)
    {
        var resourceName = storyHand.primaryTooltip;
        if (!string.IsNullOrEmpty(resourceName)) return resourceName;

        return "Uknown";
    }
}