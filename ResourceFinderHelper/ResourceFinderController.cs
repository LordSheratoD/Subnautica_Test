using System.Linq;
using UnityEngine;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.ResourceFinderHelper;

public class ResourceFinderController : MonoBehaviour
{
    private EnvironmentalObjectManager environmentalObjectManager;
    private ResourceTrackerManager resourceTrackerManager;

    private void Start()
    {
        if (resourceTrackerManager == null)
        {
            var rtManagerObject = new GameObject("ResourceTrackerManager");
            resourceTrackerManager = rtManagerObject.AddComponent<ResourceTrackerManager>();
            DontDestroyOnLoad(rtManagerObject); // Moved here after GameObject is instantiated
            //Console.WriteLine("CREATING ResourceTrackerManager");
        }

        if (environmentalObjectManager == null)
        {
            var envManagerObject = new GameObject("EnvironmentalObjectManager");
            environmentalObjectManager = envManagerObject.AddComponent<EnvironmentalObjectManager>();
            DontDestroyOnLoad(envManagerObject); // Moved here after GameObject is instantiated
            // Console.WriteLine("CREATING EnvironmentalObjectManager");
        }
    }

    private void OnGUI()
    {
        if (ESP.IsInGame())
        {
            // Comprobación de nulidad para resourceTrackerManager y sus resourceTrackers
            if (resourceTrackerManager != null)
            {
                /*if (resourceTrackerManager.resourceTrackers != null)
                {
                    foreach (var resource in resourceTrackerManager.resourceTrackers)
                    {
                        if (resource == null || resource.transform == null) continue;
                        RenderResource(resource);
                    }
                }*/

                if (resourceTrackerManager.allResource != null)
                {
                    foreach (var resource in resourceTrackerManager.allResource)
                    {
                        if (resource == null || resource.transform == null) continue;
                        
                        RenderTest();
                    }
                }
            }

            // Comprobación de nulidad para environmentalObjectManager, plantables y storyHandTargets
            if (environmentalObjectManager != null)
            {
                /*if (environmentalObjectManager.plantables != null)
                    foreach (var plantable in environmentalObjectManager.plantables)
                    {
                        if (plantable == null || plantable.transform == null) continue;
                        RenderPlantable(plantable);
                    }

                if (environmentalObjectManager.storyHandTargets != null)
                    foreach (var storyHand in environmentalObjectManager.storyHandTargets)
                    {
                        if (storyHand == null || storyHand.transform == null) continue;
                        RenderStoryHand(storyHand);
                    }*/

                /*if (environmentalObjectManager.fruitPlants != null)
                    foreach (var fruit in environmentalObjectManager.fruitPlants)
                    {
                        if (fruit == null || fruit.transform == null) continue;
                        RenderFruit(fruit);
                    }*/
            }
        }
    }

    private void RenderTest(Pickupable resource)
    {
        if (ESP.CalculateDistanceToPlayer(resource.transform.position) > resourceTrackerManager.maxDistance) return;
        if (!ESP.WorldToScreen(resource.transform.position, out var screen)) return;
        Render.DrawDistanceString(screen, resourceTrackerManager.GetTestName(resource),
            new RGBAColor(255, 0, 0, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
    }


    private void RenderFruit(FruitPlant fruit)
    {
        if (ESP.CalculateDistanceToPlayer(fruit.transform.position) >
            environmentalObjectManager.fruitPlantMaxDistance) return;
        if (!ESP.WorldToScreen(fruit.transform.position, out var screen)) return;
        float validFruit = fruit.fruits.Where(obj => obj.isActiveAndEnabled).ToList().Count;
        if (validFruit <= 0) return;
        //var fruitPos = fruit.
        Render.DrawDistanceString(screen,
            $"{environmentalObjectManager.GetFruitName(fruit.fruits[0])} [{validFruit}/{fruit.fruits.Length}]",
            new RGBAColor(255, 255, 255, 255), ESP.CalculateDistanceToPlayer(fruit.transform.position));
    }

    private void RenderResource(ResourceTracker resource)
    {
        if (ESP.CalculateDistanceToPlayer(resource.transform.position) > resourceTrackerManager.maxDistance) return;
        if (!ESP.WorldToScreen(resource.transform.position, out var screen)) return;
        Render.DrawDistanceString(screen, resourceTrackerManager.GetResourceName(resource),
            new RGBAColor(0, 0, 255, 255), ESP.CalculateDistanceToPlayer(resource.transform.position));
    }

    private void RenderPlantable(Plantable plantable)
    {
        if (ESP.CalculateDistanceToPlayer(plantable.transform.position) >
            environmentalObjectManager.plantableMaxDistance) return;
        if (!ESP.WorldToScreen(plantable.transform.position, out var screen)) return;
        Render.DrawDistanceString(screen, environmentalObjectManager.GetResourceName(plantable),
            new RGBAColor(255, 255, 0, 255), ESP.CalculateDistanceToPlayer(plantable.transform.position));
    }

    private void RenderStoryHand(StoryHandTarget storyHand)
    {
        if (ESP.CalculateDistanceToPlayer(storyHand.transform.position) >
            environmentalObjectManager.storyHandMaxDistance) return;
        if (!ESP.WorldToScreen(storyHand.transform.position, out var screen)) return;
        Render.DrawDistanceString(screen, environmentalObjectManager.GetResourceName(storyHand),
            new RGBAColor(255, 0, 0, 255), ESP.CalculateDistanceToPlayer(storyHand.transform.position));
    }
}