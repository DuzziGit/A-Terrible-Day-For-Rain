using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public bool canSpawn = true;
    public int totalMaxEnemies = 30;
    [SerializeField] private float spawnTimer = 8f; // Timer for spawning enemies every 8 seconds
    [SerializeField] private float timeSinceLastSpawn = 0f; // Time since last spawn
    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public static EnemyManager Instance;

    void OnEnable()
    {
        EnemySpawner.currentEnemiesSpawned += SpawnWave;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn due to timer or all enemies are dead
        if (timeSinceLastSpawn >= spawnTimer || AllEnemiesDead())
        {
            SpawnEnemies();
            timeSinceLastSpawn = 0f; // Reset the timer
        }
    }





    private void SpawnEnemies()
    {
        if (canSpawn)
        {
            foreach (EnemySpawner spawner in enemySpawners)
            {
                spawner.SpawnEnemy();
            }
            Debug.Log("Enemies spawned");
            canSpawn = false;
            StartCoroutine(SpawnTimer());
        }
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        if (!enemySpawners.Contains(spawner))
        {
            enemySpawners.Add(spawner);
            UpdateSpawnerLimits();
        }
    }

    public void UnregisterSpawner(EnemySpawner spawner)
    {
        if (enemySpawners.Remove(spawner))
        {
            UpdateSpawnerLimits();
        }
    }

    private void UpdateSpawnerLimits()
    {
        int spawnersCount = enemySpawners.Count;
        if (spawnersCount > 0)
        {
            int maxPerSpawner = totalMaxEnemies / spawnersCount;
            foreach (EnemySpawner spawner in enemySpawners)
            {
                spawner.SetMaxEnemies(maxPerSpawner);

            }
        }
    }

    private void SpawnWave()
    {
        canSpawn = true;
    }

    private IEnumerator SpawnTimer()
    {

        yield return new WaitForSeconds(8f);
        canSpawn = true;
    }

    private bool AllEnemiesDead()
    {
        foreach (EnemySpawner spawner in enemySpawners)
        {
            if (spawner.currentEnemies > 0)
            {
                return false; // If any spawner has enemies, return false
            }
        }
        return true; // All spawners have 0 enemies
    }
    private void OnDisable()
    {
        EnemySpawner.currentEnemiesSpawned += SpawnWave;
    }
}
