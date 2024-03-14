using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int HitCap = 3;

    [SerializeField]
    private bool DestroyAfterHitCap = false;
    private int hitCount;
    private Dictionary<Collider2D, int> hitEnemies = new Dictionary<Collider2D, int>();

    [SerializeField]
    protected float speed = 20;

    [SerializeField]
    protected float lifeTime = 0.6f;

    [SerializeField]
    protected float detectionRange = 3;

    [SerializeField]
    protected float maxDistance = 13;

    [SerializeField]
    private bool AutolockOn = false;

    [SerializeField]
    private bool ShouldMove = false;

    [SerializeField]
    private bool GenerateAttackId = true;

    [SerializeField]
    private int totalHits;

    [SerializeField]
    protected float targetingToleranceAngle = 5f;
    protected LayerMask enemyLayer = 1 << 6;
    protected bool hasDamaged = false;
    protected Vector3 initialPosition;
    protected Vector3 direction;
    protected Transform closestEnemy;
    protected Rigidbody2D rb;
    public string UniqueAttackId;

    [SerializeField]
    private float knockbackStr;

    protected float KnockbackStr
    {
        get { return knockbackStr; }
    }

    protected int TotalHits
    {
        get { return totalHits; }
    }

    [SerializeField]
    private int skillModifier;
    protected int SkillModifier => skillModifier;
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
        if (GenerateAttackId)
        {
            UniqueAttackId = HitManager.GenerateSkillActivationGuid();
        }
    }

    protected void Update()
    {
        if (AutolockOn)
        {
            // Check if an enemy is within detection range
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position,
                detectionRange,
                enemyLayer
            );

            if (colliders.Length > 0)
            {
                float closestDistance = Mathf.Infinity;
                closestEnemy = null;

                foreach (Collider2D collider in colliders)
                {
                    float directionToEnemy = Vector2.Dot(
                        transform.right,
                        (collider.transform.position - transform.position).normalized
                    );
                    if (directionToEnemy < 0)
                    {
                        continue;
                    }

                    float distance = Vector2.Distance(
                        transform.position,
                        collider.transform.position
                    );
                    float angleToEnemy = Vector2.Angle(
                        transform.right,
                        collider.transform.position - transform.position
                    );

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
        }
        float distanceTraveled = Vector3.Distance(transform.position, initialPosition);
        if (distanceTraveled >= maxDistance || hitCount >= HitCap && DestroyAfterHitCap)
        {
            DestroyProjectile();
        }
    }

    private void FixedUpdate()
    {
        if (ShouldMove)
        {
            rb.velocity = direction * speed * Time.fixedDeltaTime;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.CompareTag("Enemy")
            && !hitEnemies.ContainsKey(collision)
            && hitCount < HitCap
        )
        {
            Vector2 hitPosition = transform.position; // Position of the projectile at the time of collision
            Transform enemyTransform = collision.transform;

            HitManager.Instance.ApplyDelayedHits(
                collision,
                TotalHits,
                SkillModifier,
                UniqueAttackId,
                hitPosition,
                enemyTransform,
                KnockbackStr
            );
            hitCount++;
        }
    }

    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
