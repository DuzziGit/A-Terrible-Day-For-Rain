using System.Collections.Generic;
using UnityEngine;

public class scrio : MonoBehaviour
{
    public int maxPlatforms = 10;
    public GameObject[] platformPrefabs;
    public float xPadding = 15;
    public float yPadding = 20;
    public Vector2 minSpawnPosition = new(-100, 0);
    public Vector2 maxSpawnPosition = new(100, 120);
    public float circleRadius = 50f;
    public LayerMask platformLayer;

    private readonly List<GameObject> platforms = new();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(maxSpawnPosition.x - minSpawnPosition.x, maxSpawnPosition.y - minSpawnPosition.y, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleRadius);
    }

    private void Start()
    {

        float angleStep = 360f / maxPlatforms;
        float currentAngle = 0;

        for (int i = 0; i < maxPlatforms; i++)
        {
            GameObject prefab = platformPrefabs[Random.Range(0, platformPrefabs.Length)];

            Vector2 size = prefab.GetComponent<BoxCollider2D>().bounds.size;
            size += new Vector2(xPadding * 2, yPadding * 2);

            Vector2 position = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * circleRadius;

            position.x = Mathf.Clamp(position.x, minSpawnPosition.x + (size.x / 2), maxSpawnPosition.x - (size.x / 2));
            position.y = Mathf.Clamp(position.y, minSpawnPosition.y + (size.y / 2), maxSpawnPosition.y - (size.y / 2));

            if (Physics2D.OverlapBox(position, size, 0, platformLayer) == null)
            {
                GameObject platform = Instantiate(prefab, position, Quaternion.identity);
                platforms.Add(platform);
            }

            currentAngle += angleStep;
        }
    }
}