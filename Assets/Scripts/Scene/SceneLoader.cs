using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Attach this script to an object in your Base (main) Scene.
/// It provides static methods to load and unload sub-scenes additively.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    #region Singleton stuff
    public static SceneLoader Instance;
    public void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Two SceneLoader are not allowed, crackhead");
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    public static bool IsMapVisible => Instance.uiObjects.First().activeSelf;

    #region Input Section
    public InputActionReference toggleMapAction;
    private void OnEnable()
    {
        // Enable the action so we can read values from it
        if (toggleMapAction != null)
        {
            toggleMapAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        // Disable the action when this script isn't active
        if (toggleMapAction != null)
        {
            toggleMapAction.action.Disable();
        }
    }
    #endregion

    private void Update()
    {
        if (toggleMapAction != null && toggleMapAction.action.WasPressedThisFrame() && currentSubScene != null && !DialogDisplay.IsDialogVisible)
        {            
            ToggleUI(!uiObjects.First().activeSelf);
        }
    }

    public List<GameObject> uiObjects = new();
    public Transform witchCharacter;

    // Optionally track the currently loaded sub-scene if you only want ONE sub-scene at a time
    private string currentSubScene;
    private bool isLoading = false;

    /// <summary>
    /// Loads a sub-scene additively on top of the Base (main) Scene.
    /// If you already have a sub-scene loaded and want to replace it, 
    /// you could optionally unload it before or after loading the new one.
    /// </summary>
    /// <param name="sceneName">The name of the sub-scene to load.</param>
    public void LoadSubSceneAdditive(string sceneName)
    {
        var loadAction = new Action(() =>
        {
            var loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            loadOp.completed += (asyncOp) =>
            {
                currentSubScene = sceneName;
                // Store the name of the currently loaded sub-scene if you only allow one at a time
                Debug.Log($"Sub-scene '{sceneName}' loaded additively.");
                witchCharacter.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                ToggleUI(false);
            };
        });

        // If you only ever want one sub-scene loaded at a time, you can choose to unload the current one first:
        if (!string.IsNullOrEmpty(currentSubScene) && currentSubScene != sceneName)
        {
            UnloadSubScene(currentSubScene, loadAction);
        } else
        {
            loadAction();
        }
    }

    private void ToggleUI(bool visible)
    {
        if (DialogDisplay.IsPromptVisible)
            DialogDisplay.Instance.SuppressPrompt(visible);

        uiObjects.ForEach(obj => obj.SetActive(visible));
    }

    /// <summary>
    /// Unloads the given sub-scene (if loaded).
    /// </summary>
    /// <param name="sceneName">The name of the sub-scene to unload.</param>
    public void UnloadSubScene(string sceneName, Action followUp)
    {
        Scene sceneToUnload = SceneManager.GetSceneByName(sceneName);

        // Ensure the scene is actually loaded before unloading
        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            var unloadOp = SceneManager.UnloadSceneAsync(sceneName);
            unloadOp.completed += (asyncOp) =>
            {
                if (sceneName == currentSubScene) currentSubScene = null;
                Debug.Log($"Sub-scene '{sceneName}' unloaded.");

                followUp();
            };
        }
        else
        {
            Debug.LogWarning($"Cannot unload scene '{sceneName}' because it is not currently loaded.");
        }
    }
}
