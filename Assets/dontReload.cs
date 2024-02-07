using UnityEngine;
using UnityEngine.SceneManagement;

public class dontReload : MonoBehaviour
{
    // Static boolean to track if the scene has been loaded before
    private static bool hasLoadedBefore = false;

    private void Awake()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasLoadedBefore)
        {
            // Disable the game object if the scene has been loaded before
            gameObject.SetActive(false);
        }
        else
        {
            // Mark that the scene has been loaded at least once
            hasLoadedBefore = true;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when this object is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
