using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public bool CanSpawn = true;
    public int TotalMaxEnemies = 30;

    [SerializeField]
    private float _spawnTimer = 8f; // Timer for spawning enemies every 8 seconds

    [SerializeField]
    private float _timeSinceLastSpawn = 0f; // Time since last spawn
    private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();
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
        _timeSinceLastSpawn += Time.deltaTime;

        // Check if it's time to spawn due to timer or all enemies are dead
        if (_timeSinceLastSpawn >= _spawnTimer || AllEnemiesDead())
        {
            SpawnEnemies();
            _timeSinceLastSpawn = 0f; // Reset the timer
        }
    }

    private void SpawnEnemies()
    {
        if (CanSpawn)
        {
            foreach (EnemySpawner spawner in _enemySpawners)
            {
                spawner.SpawnEnemy();
            }
            Debug.Log("Enemies spawned");
            CanSpawn = false;
            StartCoroutine(SpawnTimer());
        }
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        if (!_enemySpawners.Contains(spawner))
        {
            _enemySpawners.Add(spawner);
            UpdateSpawnerLimits();
        }
    }

    public void UnregisterSpawner(EnemySpawner spawner)
    {
        if (_enemySpawners.Remove(spawner))
        {
            UpdateSpawnerLimits();
        }
    }

    private void UpdateSpawnerLimits()
    {
        int spawnersCount = _enemySpawners.Count;
        if (spawnersCount > 0)
        {
            int maxPerSpawner = TotalMaxEnemies / spawnersCount;
            foreach (EnemySpawner spawner in _enemySpawners)
            {
                spawner.SetMaxEnemies(maxPerSpawner);
            }
        }
    }

    private void SpawnWave()
    {
        CanSpawn = true;
    }

    private IEnumerator SpawnTimer()
    {
        yield return new WaitForSeconds(8f);
        CanSpawn = true;
    }

    private bool AllEnemiesDead()
    {
        foreach (EnemySpawner spawner in _enemySpawners)
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
