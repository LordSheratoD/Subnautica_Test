using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualHelperESP.RenderHelper;

public class ESP
{
    internal static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
    {
        screen = camera.WorldToViewportPoint(world);
        screen.x *= (float)Screen.width;
        screen.y *= (float)Screen.height;
        screen.y = (float)Screen.height - screen.y;
        return (double)screen.z > 0.0;
    }
    
    
    internal static bool WorldToScreen(Vector3 world, out Vector3 screen)
    {
        screen = MainCamera.camera.WorldToViewportPoint(world);
        screen.x *= (float)Screen.width;
        screen.y *= (float)Screen.height;
        screen.y = (float)Screen.height - screen.y;
        return (double)screen.z > 0.0;
    }

    internal static bool IsInGame()
    {
        return (SceneManager.GetActiveScene().name == "Main" && Player.main != null && MainCamera.camera != null) ;
    }
    
    /*internal static bool IsInGame()
    {
        if (SceneManager.GetActiveScene().name == "Main")
        {
            if (Player.main != null)
            {
                if (MainCamera.camera != null)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("MAIN CAMERA NOT VALID");
                }
            }
            else
            {
                Console.WriteLine("PLAYER NOT VALID");
            }
        }
        else
        {
            
        }

        return false;
    }*/
    
    internal static float CalculateDistanceToPlayer(Vector3 position)
    {
        return (float)Mathf.Round(Vector3.Distance(Player.main.transform.position, position));
    }
}