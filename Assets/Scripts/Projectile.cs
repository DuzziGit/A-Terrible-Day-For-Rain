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
    public int playeLevel;
    public float critChance;
    public float critMultiplier;
    public float targetingToleranceAngle = 5f;

    private Vector3 initialPosition;
    public Vector3 direction;
    public Transform closestEnemy;
    private Rigidbody2D rb;
    protected int damage;
    protected bool isCrit = false;
    private const int baseMinDamage = 950;
    private const int baseMaxDamage = 1050;
    private const float damageGrowthRate = 1.1f;
    int playerLevel;
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
    public int CalculateDamageForLevel(int playerLevel)
    {
        int minDamageAtLevel = Mathf.FloorToInt(baseMinDamage * Mathf.Pow(damageGrowthRate, (playerLevel - 1)));
        int maxDamageAtLevel = Mathf.FloorToInt(baseMaxDamage * Mathf.Pow(damageGrowthRate, (playerLevel - 1)));

        return Random.Range(minDamageAtLevel, maxDamageAtLevel + 1);
    }
    public void Update()
    {
        playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().level;


        damage = CalculateDamageForLevel(playerLevel);

        // Implementing critical hits
        isCrit = Random.value < (critChance / 100f);
        if (isCrit)
        {
            damage = Mathf.FloorToInt(damage * critMultiplier); // Apply crit multiplier to the base damage
        }


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
                {
                    continue;
                }

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
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Projectile collided with: " + collision.gameObject.name);
        // if (!hasDamaged && collision.transform == closestEnemy)
        if (!hasDamaged && collision.gameObject.CompareTag("Enemy"))
        {
            /*      Debug.Log("Projectile collided with: " + collision.gameObject.name);
                  Debug.Log("Enemy tag: " + collision.tag);
                  Debug.Log("Collided object layer: " + LayerMask.LayerToName(collision.gameObject.layer));

                 */
            if (collision.CompareTag("Enemy"))
            {
                //   Debug.Log("ENEMY MUST TAKE DAMAGE !" + damage);
                collision.GetComponent<EnemyCon>().TakeDamage(damage, isCrit);
            }

            hasDamaged = true;
            DestroyProjectile();
            return; // Exit the method after hitting the enemy

        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
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
                collision.GetComponent<EnemyCon>().TakeDamage(damage, isCrit);
            }

            hasDamaged = true;
            DestroyProjectile();
            return; // Exit the method after hitting the enemy

        }
    }
    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}