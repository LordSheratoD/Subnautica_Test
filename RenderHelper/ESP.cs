using UnityEngine;
using UnityEngine.SceneManagement;

namespace VisualHelperESP.RenderHelper;

public class ESP
{
    internal static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
    {
        screen = camera.WorldToViewportPoint(world);
        screen.x *= Screen.width;
        screen.y *= Screen.height;
        screen.y = Screen.height - screen.y;
        return screen.z > 0.0;
    }


    /*internal static bool WorldToScreen(Vector3 world, out Vector3 screen)
    {
        screen = MainCameraV2.main.cam.WorldToViewportPoint(world);
        screen.x *= (float)Screen.width;
        screen.y *= (float)Screen.height;
        screen.y = (float)Screen.height - screen.y;
        return (double)screen.z > 0.0;
    }*/

    public static bool WorldToScreen(Vector3 worldPosition, out Vector3 screenPosition)
    {
        var mainCamera =
            MainCameraV2.main.cam != null
                ? MainCameraV2.main.cam
                : MainCamera.camera; // Asume que usas la cámara principal para la conversión
        screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Verifica si el objeto está detrás de la cámara usando el clip space z
        var isBehind = screenPosition.z < 0;

        // Ajusta la posición de la pantalla para que y sea invertida, Unity usa el origen en la esquina inferior izquierda, pero OnGUI lo usa en la superior izquierda
        screenPosition.y = Screen.height - screenPosition.y;

        return !isBehind && screenPosition.x >= 0 && screenPosition.x <= Screen.width && screenPosition.y >= 0 &&
               screenPosition.y <= Screen.height;
    }

    internal static bool IsInGame()
    {
        return SceneManager.GetActiveScene().name == "Main" && Player.main != null && MainCamera.camera != null;
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
        return Mathf.Round(Vector3.Distance(Player.main.transform.position, position));
    }
}