using UnityEngine;

public class enemySpawnGen : MonoBehaviour
{
    public Transform[] spawnPoints;     // Array of spawn points
    public GameObject[] enemies;        // Array of enemy game objects
    public int maxEnemies = 10;         // Maximum number of enemies to spawn
    public int minLevel = 1;            // Minimum enemy level
    public int maxLevel = 5;            // Maximum enemy level

    private int currentEnemies = 0;     // Current number of enemies

    void Update()
    {
        // If we haven't reached the max number of enemies, spawn a new one
        if (currentEnemies < maxEnemies)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = Random.Range(0, enemies.Length);

            // Instantiate the enemy and set its level
            GameObject newEnemy = Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
            int level = Random.Range(minLevel, maxLevel + 1);
            //        newEnemy.GetComponent<enemycontroller>().SetLevel(level);

            currentEnemies++;
        }
    }
}