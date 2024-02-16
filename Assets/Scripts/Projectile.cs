using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 20;
    [SerializeField] protected float lifeTime = 0.6f;
    [SerializeField] protected float detectionRange = 3;
    [SerializeField] protected float maxDistance = 13;
    protected LayerMask enemyLayer = 1 << 6;
    protected bool hasDamaged = false;
    protected float targetingToleranceAngle = 5f;
    protected Vector3 initialPosition;
    protected Vector3 direction;
    protected Transform closestEnemy;
    protected Rigidbody2D rb;
    [SerializeField] private int totalHits;
    protected int TotalHits
    {
        get { return totalHits; }
    }
    [SerializeField] private int minDamage;
    protected int MinDamage
    {
        get { return minDamage; }
        set { minDamage = value; }
    }
    [SerializeField] private int maxDamage;
    protected int MaxDamage
    {
        get { return maxDamage; }
        set { maxDamage = value; }
    }
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        initialPosition = transform.position;
        Invoke("DestroyProjectile", lifeTime);
        hasDamaged = false;
        direction = transform.right;
    }
    protected void Update()
    {
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
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasDamaged && collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Enemy"))
            {
                HitManager.Instance.ApplyDelayedHits(collision, totalHits, MinDamage, MaxDamage);
                DestroyProjectile();
            }
            hasDamaged = true;

            return; // Exit the method after hitting the enemy

        }
    }

    // protected virtual void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (!hasDamaged && collision.transform == closestEnemy)
    //     {
    //         if (collision.CompareTag("Enemy"))
    //         {
    //             HitManager.Instance.ApplyDelayedHits(collision, totalHits - 1, hitCooldown, baseMinDamage, baseMaxDamage);
    //         }

    //         hasDamaged = true;
    //         DestroyProjectile();
    //         return; // Exit the method after hitting the enemy

    //     }
    // }
    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}