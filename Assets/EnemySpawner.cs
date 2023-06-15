using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
  public Transform[] spawnPoints;
  public GameObject[] enemyPrefabs;
  public int maxEnemies;
  public static int currentEnemies;


void Start(){
currentEnemies = 0;
}
    // Update is called once per frame
    void FixedUpdate()
    {
        if ( currentEnemies < maxEnemies) {
            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpawnPoint = Random.Range(0, spawnPoints.Length);
            Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation);
            currentEnemies++;
             Debug.Log("Current Enemies updated" + currentEnemies);
        }
    }
}



//This comment serves no purpose
//Using this comment to generate a fake commit to Test plastic scm