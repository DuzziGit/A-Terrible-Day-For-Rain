using UnityEngine;

public class dontDestroy : MonoBehaviour
{
    public static dontDestroy instance; // Singleton instance

    private void Awake()
    {
        // Check for an existing instance of BackgroundManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // This object will not be destroyed when loading a new scene
        }
        else if (instance != this)
        {
            Destroy(gameObject); // Destroy any other instances that are not the original
        }
    }
}
