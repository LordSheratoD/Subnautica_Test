using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class EnvironmentalObjectManager : MonoBehaviour
{
    public List<Plantable> plantables = new();
    public List<StoryHandTarget> storyHandTargets = new();
    public List<FruitPlant> fruitPlants = new ();
    
    
    public float plantableMaxDistance = 30f;
    public float storyHandMaxDistance = 100f;
    public float fruitPlantMaxDistance = 30f;

    private float _lastPlantableUpdateTime;
    private float _lastStoryHandUpdateTime;
    private float _lastFruitPlantUpdateTime;

    private const float PlantableUpdateInterval = 2f;
    private const float StoryHandUpdateInterval = 2f;
    private const float FruitPlantUpdateInterval = 2f;

    private void Update()
    {
        if (ESP.IsInGame())
        {
            /*if (Time.time - _lastPlantableUpdateTime >= PlantableUpdateInterval)
            {
                UpdatePlantables();
                _lastPlantableUpdateTime = Time.time;
            }

            if (Time.time - _lastStoryHandUpdateTime >= StoryHandUpdateInterval)
            {
                UpdateStoryHandTargets();
                _lastStoryHandUpdateTime = Time.time;
            }*/
            
            /*if (Time.time - _lastFruitPlantUpdateTime >= FruitPlantUpdateInterval)
            {
                UpdateFruitPlants();
                _lastFruitPlantUpdateTime = Time.time;
            }*/
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateFruitPlants()
    {
        fruitPlants = FindObjectsOfType<FruitPlant>()
            .Where(obj => obj != null && obj.enabled && obj.transform != null &&
                          ESP.CalculateDistanceToPlayer(obj.transform.position) <= fruitPlantMaxDistance && obj.fruits.Length > 0)
            .ToList();
    }

    internal Vector3 GetFruitPosition(FruitPlant plant, PickPrefab[] fruits)
    {
        var fruitZ = fruits[0].transform.localPosition.z;
        Vector3 fruitPos = new Vector3(0, 0, fruitZ);
        Vector3 plantPos = plant.transform.position;
        Vector3 newPos = plantPos + fruitPos;
        return newPos;
    }
    
    /*internal string GetPlantName(FruitPlant plant)
    {
        var techType = plant
    }*/
    
    internal string GetFruitName(PickPrefab fruit)
    {
        var techType = fruit.pickTech;
        if (techType != null)
        {
            var resourceName = Language.main.Get(techType);
            if (!string.IsNullOrEmpty(resourceName)) return resourceName;
        }

        return "Uknown";
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
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