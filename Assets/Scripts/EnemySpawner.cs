using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    private int maxEnemies;
    public int currentEnemies; // Not static, so it's specific to this instance
    [SerializeField] private float padding = 1.0f; // Adjustable padding distance
    private int lastSpawnIndex = -1; // Track the last spawn point used
    [SerializeField] private int minLevel; // Minimum enemy level for this map
    [SerializeField] private int maxLevel; // Maximum enemy level for this map
    public delegate void CurrentEnemiesSpawned();
    public static CurrentEnemiesSpawned currentEnemiesSpawned;

    private IEnumerator Start()
    {
        while (GameController.instance == null)
        {
            yield return null; // Wait until GameController.instance is not null
        }
        GameController.instance.RegisterSpawner(this);
    }

    private void OnDestroy()
    {
        if (GameController.instance != null) // Check if GameController instance exists
        {
            GameController.instance.UnregisterSpawner(this);
        }
    }

    public void SetMaxEnemies(int max)
    {
        maxEnemies = max;
    }

    public void SpawnEnemy()
    {
        int playerLevel = GameController.instance.playerMovement.level;
        // Ensure enemy level scales with player level but stays within minLevel and maxLevel
        int enemyLevel = Mathf.Clamp(playerLevel + Random.Range(-2, 3), minLevel, maxLevel);

        while (currentEnemies < maxEnemies)
        {
            lastSpawnIndex = (lastSpawnIndex + 1) % spawnPoints.Length;
            Transform spawnPoint = spawnPoints[lastSpawnIndex];
            Vector3 paddedPosition = spawnPoint.position + new Vector3(Random.Range(-padding, padding), Random.Range(-padding, padding), 0);

            // Spawn enemy
            GameObject enemyInstance = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], paddedPosition, Quaternion.identity, GameController.instance.enemiesContainer);
            Enemy enemyComponent = enemyInstance.GetComponent<Enemy>();

            if (enemyComponent != null)
            {
                enemyInstance.GetComponent<Enemy>().MySpawner = this;
                enemyComponent.level = enemyLevel;
                enemyComponent.health = enemyLevel * 150;
                enemyComponent.expValue = enemyLevel * 2;
                // enemyComponent.damage = enemyLevel * 20;
            }
            currentEnemies++;
        }
    }

    public void OnEnemyDestroyed()
    {
        currentEnemies--;
        currentEnemies = Mathf.Max(0, currentEnemies); // Ensure it doesn't go below 0

        if (currentEnemies == 0 && currentEnemiesSpawned != null)
        {
            currentEnemiesSpawned.Invoke();
            Debug.Log("enemiesCleared");
        }
    }
}
