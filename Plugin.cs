using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private void Start()
    {
    }

    private void Update()
    {
        var keyDown = Input.GetKeyDown((KeyCode)47);
        if (keyDown) Logger.LogInfo($"Ubication: {Player.main.transform.position}");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckAndAddResourceFinder();
    }

    private void CheckAndAddResourceFinder()
    {
        if (FindObjectOfType<ResourceFinderController>() == null)
        {
            Logger.LogInfo("##  ##        Loaded ResourceFinderController        ##  ##");
            AddResourceFinder();
        }
    }

    private void AddResourceFinder()
    {
        // Crea un nuevo GameObject para el mini mapa
        var resFinderObject = new GameObject("ResourceFinderController");
        // Asegura que el GameObject no sea destruido al cargar una nueva escena
        DontDestroyOnLoad(resFinderObject);
        // Añade el script del mini mapa al GameObject
        resFinderObject.AddComponent<ResourceFinderController>();
    }
}