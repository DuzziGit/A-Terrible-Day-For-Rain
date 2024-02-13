using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public int maxEnemies;
    public static int currentEnemies;
    private PlayerMovement playerMovement;
    private GameObject player;
    private void Awake()
    {
        currentEnemies = 0;
        // Cache the PlayerMovement component at the start

    }

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        if (currentEnemies < maxEnemies && playerMovement != null)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        int randEnemyIndex = Random.Range(0, enemyPrefabs.Length);
        int randSpawnPointIndex = Random.Range(0, spawnPoints.Length);
        int playerLevel = playerMovement.level;
        //        Debug.Log(playerLevel);

        int enemyLevel = Random.Range(playerLevel, playerLevel + 5);
        GameObject enemyInstance = Instantiate(enemyPrefabs[randEnemyIndex], spawnPoints[randSpawnPointIndex].position, Quaternion.identity);

        Enemy enemyComponent = enemyInstance.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            enemyComponent.level = enemyLevel;
            enemyComponent.health = enemyLevel * 150;
            enemyComponent.expValue = enemyLevel * 2;
            // Uncomment and adjust as necessary:
            // enemyComponent.coinValue = enemyLevel * 20;
            // enemyComponent.damage = enemyLevel * 20;
        }

        currentEnemies++;
        //        Debug.Log("Current Enemies updated: " + currentEnemies);
    }
}
