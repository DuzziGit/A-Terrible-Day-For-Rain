using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerManager : MonoBehaviour
{
    public static ContainerManager Instance;
    public Transform enemiesContainer;
    public Transform DamageNumContainer;
    public Transform ProjectileContainer;
    public Transform ImpactContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        InitializeTextContainersContainer();
        InitializeProjectileContainer();
        InitializeEnemiesContainer();
        InitializeImpactContainer();
    }

    private void InitializeEnemiesContainer()
    {
        // Check if the container already exists (useful in case of scene reloads)
        GameObject existingContainer = GameObject.Find("EnemiesContainer");
        if (existingContainer != null)
        {
            enemiesContainer = existingContainer.transform;
        }
        else
        {
            // Create a new GameObject to act as the container for all enemies
            GameObject container = new GameObject("EnemiesContainer");
            enemiesContainer = container.transform;
            enemiesContainer.position = new Vector3(
                enemiesContainer.position.x,
                enemiesContainer.position.y,
                -5
            );
        }
    }

    private void InitializeTextContainersContainer()
    {
        GameObject textContainersContainer = GameObject.Find("TextContainers");
        if (textContainersContainer == null)
        {
            textContainersContainer = new GameObject("TextContainers");
            textContainersContainer.transform.position = new Vector3(0, 0, -5); // Set the Z position
        }
        DamageNumContainer = textContainersContainer.transform;
    }

    private void InitializeProjectileContainer()
    {
        // Look for an existing ProjectileContainer GameObject in the scene
        GameObject projectileContainer = GameObject.Find("ProjectileContainer");
        if (projectileContainer == null)
        {
            // If it doesn't exist, create a new GameObject named "ProjectileContainer"
            projectileContainer = new GameObject("ProjectileContainer");
            // Set its position with a Z value of -5 to keep projectiles organized in a specific layer
            projectileContainer.transform.position = new Vector3(0, 0, -5);
        }
        // Store the Transform of ProjectileContainer for later use when instantiating projectiles
        ProjectileContainer = projectileContainer.transform;
    }

    private void InitializeImpactContainer()
    {
        GameObject impactContainer = GameObject.Find("ImpactContainer");
        if (impactContainer == null)
        {
            impactContainer = new GameObject("ImpactContainer");
            impactContainer.transform.position = new Vector3(0, 0, -5);
        }
        ImpactContainer = impactContainer.transform;
    }
}
