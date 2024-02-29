using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceFinderEngine : MonoBehaviour
{
    enum ResourceType
    {
        PLANT,
        PDA,
        RESOURCE,
        FRUIT
    }
    
    public List<Pickupable> plantables = new();
    public List<Pickupable> resources = new();
    public List<StoryHandTarget> storyHandTargets = new();
    public List<FruitPlant> fruitPlants = new ();
    public List<Pickupable> allResource = new();

    public float maxDistance = 50f;
    
    private void UpdateAllResourcesTrackers()
    {
        plantables.Clear(); 
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
                    if (obj.GetComponentInParent<Plantable>() != null)
                    {
                        plantables.Add(obj);
                    }
                    if (obj.GetComponentInParent<ResourceTracker>() != null)
                    {
                        resources.Add(obj);
                    }
                }
            }
        }
        allResource = resources;
    }
    
    private void UpdateFruitPlants()
    {
        fruitPlants = FindObjectsOfType<FruitPlant>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= maxDistance && 
                          ESP.CalculateDistanceToPlayer(obj.transform.position) > 0 && obj.fruits.Length > 0)
            .ToList();
    }
    
    internal string GetResourceName(Pickupable pickupable)
    {
        var techType = pickupable.GetTechType();
        if (techType != null)
        {
            var resourceName = Language.main.Get(techType);
            if (!string.IsNullOrEmpty(resourceName)) return resourceName;
        }

        return "Uknown";
    }
}