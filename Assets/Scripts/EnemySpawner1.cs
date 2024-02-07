using UnityEngine;

public class EnemySpawner1 : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public int maxEnemies;
    public static int currentEnemies;
    private int enemyMinLevel;
    private int enemyMaxLevel;

    private void Start()
    {
        currentEnemies = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentEnemies < maxEnemies)
        {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpawnPoint = Random.Range(0, spawnPoints.Length);
            int playerLevel = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().level;
            Debug.Log(playerLevel);
            enemyMinLevel = playerLevel;
            enemyMaxLevel = playerLevel + 5;
            int randomLevel = Random.Range(enemyMinLevel, enemyMaxLevel);
            Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation);
            enemyPrefabs[randEnemy].GetComponent<Enemy>().level = randomLevel;
            enemyPrefabs[randEnemy].GetComponent<Enemy>().health = randomLevel * 150;
            enemyPrefabs[randEnemy].GetComponent<Enemy>().expValue = randomLevel * 2;
            // enemyPrefabs[randEnemy].GetComponent<Enemy>().coinValue = randomLevel * 20;
            // enemyPrefabs[randEnemy].GetComponent<Enemy>().damage = randomLevel * 20;
            currentEnemies++;
            Debug.Log("Current Enemies updated: " + currentEnemies);
        }
    }
}
