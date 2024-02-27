using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using VisualHelperESP.MapHelper;
using VisualHelperESP.ResourceFinderHelper;

namespace VisualHelperESP;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.snmodding.nautilus")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; private set; }

    private static Assembly Assembly { get; } = Assembly.GetExecutingAssembly();

    private void Awake()
    {
        Logger = base.Logger;
        Harmony.CreateAndPatchAll(Assembly, $"{PluginInfo.PLUGIN_GUID}");
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        SceneManager.sceneLoaded += OnSceneLoaded;
        AddResourceFinder();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckAndAddResourceFinder();
    }

    private void CheckAndAddResourceFinder()
    {
        if (FindObjectOfType<ResourceFinderController>() == null)
        {
            AddResourceFinder();
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        var keyDown = Input.GetKeyDown((KeyCode)47);
        if (keyDown)
        {
            Logger.LogInfo($"Ubication: {Player.main.transform.position}");
        }
    }
    
    private void AddMiniMap()
    {
        // Crea un nuevo GameObject para el mini mapa
        GameObject miniMapObject = new GameObject("MiniMapController");
        // Asegura que el GameObject no sea destruido al cargar una nueva escena
        DontDestroyOnLoad(miniMapObject);
        // Añade el script del mini mapa al GameObject
        miniMapObject.AddComponent<MiniMapController>();
        // Configura las propiedades necesarias, como la referencia al jugador
        MiniMapController miniMapController = miniMapObject.GetComponent<MiniMapController>();
        
    }
    
    private void AddNavigationMap()
    {
        // Crea un nuevo GameObject para el mini mapa
        GameObject navigationObject = new GameObject("NavigatorMapController");
        // Asegura que el GameObject no sea destruido al cargar una nueva escena
        DontDestroyOnLoad(navigationObject);
        // Añade el script del mini mapa al GameObject
        navigationObject.AddComponent<NavigatorMapController>();
        
    }
    
    private void AddResourceFinder()
    {
        // Crea un nuevo GameObject para el mini mapa
        GameObject resFinderObject = new GameObject("ResourceFinderController");
        // Asegura que el GameObject no sea destruido al cargar una nueva escena
        DontDestroyOnLoad(resFinderObject);
        // Añade el script del mini mapa al GameObject
        resFinderObject.AddComponent<ResourceFinderController>();
        
    }

}