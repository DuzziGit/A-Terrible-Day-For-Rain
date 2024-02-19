using Cinemachine;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    public Image blackSquare;

    public GameObject gameControlsUi;
    public CinemachineVirtualCamera cinemachineCam;

    public static GameController instance;
    public int totalMaxEnemies = 30;
    private List<EnemySpawner> enemySpawners = new List<EnemySpawner>();
    public PlayerMovement playerMovement;
    public GameObject Player;
    public bool canSpawn = true;
    private float spawnTimer = 8f; // Timer for spawning enemies every 8 seconds
    private float timeSinceLastSpawn = 0f; // Time since last spawn



    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        EnemySpawner.currentEnemiesSpawned += SpawnWave;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        blackSquare.color = new Color(blackSquare.color.r, blackSquare.color.g, blackSquare.color.b, 1f);
        _ = StartCoroutine(DelayedFade());

    }
    private IEnumerator DelayedFade()
    {
        yield return new WaitForSeconds(0.9f); // Wait to make it a total of 1 second with the HandleSceneSetup delay
        _ = StartCoroutine(FadeBlackInSquare());
    }


    public IEnumerator FadeBlackInSquare(float fadeSpeed = 0.2f)
    {
        Color objectColor = blackSquare.color;
        float fadeAmount;

        while (blackSquare.color.a > 0)
        {
            fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            blackSquare.color = objectColor;
            yield return null;
        }
    }

    public IEnumerator FadeBlackOutSquare(bool fadeToBlack = true, float fadeSpeed = .2f)
    {
        Color objectColor = blackSquare.color;
        float fadeAmount;

        if (fadeToBlack)
        {
            objectColor.a = 0;
            blackSquare.color = objectColor;

            while (blackSquare.color.a < 1)
            {
                fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.color = objectColor;
                yield return null;
            }
        }
        else
        {
            while (blackSquare.color.a > 0)
            {
                fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);
                objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
                blackSquare.color = objectColor;
                yield return null;
            }
        }
    }
    private void OnDisable()
    {
        EnemySpawner.currentEnemiesSpawned += SpawnWave;
    }
    private void OnDestroy()
    {
        // Make sure to unregister the OnSceneLoaded method when this object is destroyed.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
