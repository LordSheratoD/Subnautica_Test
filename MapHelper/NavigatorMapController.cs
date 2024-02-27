using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VisualHelperESP.RenderHelper;

namespace VisualHelperESP.MapHelper;

public class NavigatorMapController : MonoBehaviour
{

    private void Awake()
    {
        Ubications.InitializeLocations();
    }
    
    internal static bool WorldToScreen(Vector3 world, out Vector3 screen)
    {
        screen = MainCamera.camera.WorldToViewportPoint(world);
        screen.x *= (float)Screen.width;
        screen.y *= (float)Screen.height;
        screen.y = (float)Screen.height - screen.y;
        return (double)screen.z > 0.0;
    }

    
    private void OnGUI()
    {
        if (ESP.IsInGame())
        {
            //Console.WriteLine($"Distance: {Vector3.Distance(location, Player.main.transform.position)}");
            foreach (var location in Ubications.locations)
            {
                if (!WorldToScreen(location.position, out var screen)) continue;
                //Render(screen, location.name, new RGBAColor(0, 255, 0, 255), ESP.CalculateDistanceToPlayer(location.position));
                Render.DrawDistanceString(screen, location.name, new RGBAColor(0, 255, 0, 255), ESP.CalculateDistanceToPlayer(location.position));
                Vector2 startPoint = new Vector2(Screen.width / 2f, Screen.height/ 2f);
                Render.DrawLine(startPoint, new Vector2(screen.x, screen.y), new RGBAColor(255, 255, 255, 255), 1);
            }
        }
    }

}