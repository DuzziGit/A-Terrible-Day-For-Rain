using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float lifeTime;
    public float detectionRange;
    public float maxDistance;
    public LayerMask enemyLayer;
    public int minDamage; // Minimum damage
    public int maxDamage; // Maximum damage
    public bool hasDamaged;
    public int skillLevel;
    public float critChance;
    public float critMultiplier;
    public float targetingToleranceAngle = 5f;

    private Vector3 initialPosition;
    public Vector3 direction;
    public Transform closestEnemy;
    private Rigidbody2D rb;
    public int damage; // Actual damage inflicted

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        initialPosition = transform.position;
        Invoke("DestroyProjectile", lifeTime);
        hasDamaged = false;
        direction = transform.right;
    }

    public void Update()
    {
        skillLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().skillOneLevel;

        // Scaling damage with skill level
        minDamage = Mathf.FloorToInt(100 * Mathf.Pow(2, (skillLevel - 1) / 4));
        maxDamage = Mathf.FloorToInt(125 * Mathf.Pow(2, (skillLevel - 1) / 4));

        // Implementing critical hits
        bool isCrit = Random.value < critChance;
        damage = Random.Range(minDamage, maxDamage + 1);
        if (isCrit)
            damage = Mathf.FloorToInt(damage * critMultiplier);

        // Check if an enemy is within detection range
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, enemyLayer);

        if (colliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            closestEnemy = null;

            foreach (Collider2D collider in colliders)
            {
                float directionToEnemy = Vector2.Dot(transform.right, (collider.transform.position - transform.position).normalized);
                if (directionToEnemy < 0)
                    continue;

                float distance = Vector2.Distance(transform.position, collider.transform.position);
                float angleToEnemy = Vector2.Angle(transform.right, collider.transform.position - transform.position);

                if (distance < closestDistance && angleToEnemy <= targetingToleranceAngle)
                {
                    closestDistance = distance;
                    closestEnemy = collider.transform;
                }
            }

            if (closestEnemy != null)
            {
                direction = (closestEnemy.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            direction = transform.right;
        }

        rb.velocity = direction * speed;

        float distanceTraveled = Vector3.Distance(transform.position, initialPosition);
        if (distanceTraveled >= maxDistance)
        {
            DestroyProjectile();
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("Projectile collided with: " + collision.gameObject.name);
        if (!hasDamaged && collision.transform == closestEnemy)
        {
            /*      Debug.Log("Projectile collided with: " + collision.gameObject.name);
                  Debug.Log("Enemy tag: " + collision.tag);
                  Debug.Log("Collided object layer: " + LayerMask.LayerToName(collision.gameObject.layer));

                 */
            if (collision.CompareTag("Enemy"))
            {
                //   Debug.Log("ENEMY MUST TAKE DAMAGE !" + damage);
                collision.GetComponent<EnemyCon>().TakeDamage(damage);
            }

            hasDamaged = true;
            DestroyProjectile();
            return; // Exit the method after hitting the enemy
        }
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
        //   Debug.Log(skillLevel);
    }
}