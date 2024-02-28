using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceTrackerManager : MonoBehaviour
{
    public List<ResourceTracker> resourceTrackers = new();
    public float maxDistance = 25f;
    private float lastUpdateTime;
    private readonly float updateInterval = 1.5f;

    private void Update()
    {
        if (ESP.IsInGame())
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                //Console.WriteLine("UPDATING RESOURCES");
                UpdateResourceTrackers();
                lastUpdateTime = Time.time;
            }
    } // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateResourceTrackers()
    {
        resourceTrackers = FindObjectsOfType<ResourceTracker>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance)
            .ToList();
    }

    internal string GetResourceName(ResourceTracker tracker)
    {
        var techType = tracker.techType;
        if (techType != null)
        {
            var resourceName = Language.main.Get(techType);
            if (!string.IsNullOrEmpty(resourceName)) return resourceName;
        }

        return "Uknown";
    }
}