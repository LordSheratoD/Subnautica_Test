using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceTrackerManager : MonoBehaviour
{
    public List<ResourceTracker> resourceTrackers = new();
    public List<Pickupable> allResource = new();
    public float maxDistance = 25f;
    private float lastUpdateTime;
    private readonly float updateInterval = 1.5f;

    private void Update()
    {
        if (ESP.IsInGame())
        {
            /*if (Time.time - lastUpdateTime >= updateInterval)
            {
                //Console.WriteLine("UPDATING RESOURCES");
                UpdateResourceTrackers();
                lastUpdateTime = Time.time;
            }*/
            if (Time.time - lastUpdateTime >= updateInterval)
            {
                //Console.WriteLine("UPDATING RESOURCES");
                UpdateAllResourcesTrackers();
                lastUpdateTime = Time.time;
            }
        }
    }
    
    internal string GetTestName(Pickupable pickupable)
    {
        var techType = pickupable.GetTechType();
        if (techType != null)
        {
            var resourceName = Language.main.Get(techType);
            if (!string.IsNullOrEmpty(resourceName)) return resourceName;
        }

        return "Uknown";
    }

    // ReSharper disable Unity.PerformanceAnalysis

    private void UpdateAllResourcesTrackers()
    {
        /*allResource = FindObjectsOfType<Pickupable>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance && ESP.CalculateDistanceToPlayer(obj.transform.position) > 0)
            .ToList();*/
        
        
        
        List<Pickupable> resources = new List<Pickupable>();
        resources.Clear();
        foreach (var obj in FindObjectsOfType<Pickupable>())
        {
            //Valid Object
            if (obj != null && obj.enabled && obj.transform != null)
            {
                //Valid Distance
                float distance = ESP.CalculateDistanceToPlayer(obj.transform.position);
                if (distance <= maxDistance && distance > 0)
                {
                    //Not Fish
                    /*var techType = obj.GetTechType();
                    if (!(techType.ToString().ToLower()).Contains("fish") && obj.name.ToLower().Contains("fish"))
                    {
                        resources.Add(obj);
                    }*/
                    if (obj.GetComponentInParent<Plantable>() != null)
                    {
                        resources.Add(obj);
                    }
                }
            }
        }

        allResource = resources;

        //var rand = allResource[0].GetComponentInParent<ResourceTracker>();
    }

    // ReSharper disable Unity.PerformanceAnalysis
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