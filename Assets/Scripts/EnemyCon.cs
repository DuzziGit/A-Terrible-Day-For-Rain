using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyCon : Enemy
{

    public int enemyDamage;
    public TMP_Text enemyLevel;
    public GameObject TextParentPrefab;
    private HealthBar healthBar;
    private int maxHealth;
    [SerializeField] private GameObject textContainerPrefab;
    [SerializeField] private GameObject DamageNumText;
    [SerializeField] private GameObject DamageNumTextCrit;
    private float lastNormalizedTime;

    public bool isDisplayingDamage = false;// Flag to track if damage is currently being displayed
    private const int baseEnemyHealth = 10000;
    private const float healthGrowthRate = 1.1f; // This can be adjusted

    [SerializeField] private float baseOffsetY = 0.2f;
    [SerializeField] private float incrementalOffsetY = 0.3f;
    private List<(int Damage, bool IsCrit, string AttackId)> damageTaken = new List<(int Damage, bool IsCrit, string AttackId)>();
    private Dictionary<string, GameObject> damageTextCanvases = new Dictionary<string, GameObject>();

    private bool isInHitAnimation = false; // Flag to indicate if the hit animation is currently playing
    private float lastHitTime = -1f; // Timestamp of the last hit
    private float hitAnimationCooldown = 0.5f; // Cooldown duration in seconds
    private Vector3 initialPosition;
    private Dictionary<string, int> damageNumberCounts = new Dictionary<string, int>();

    private void Start()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        enemyLevel.text = level.ToString();
        enemyDamage = level * 5;
        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        maxHealth = Mathf.FloorToInt(baseEnemyHealth * Mathf.Pow(healthGrowthRate, (level - 1)));
        health = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void TakeDamage(int damage, bool isCrit, string attackId, Vector2 hitDirection, float KnockbackStr)
    {
        Knockback(hitDirection, KnockbackStr);
        health = Mathf.Max(0, health - damage);
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }

        // Instantiate TextParent for damage numbers if not already done for this attack
        if (!damageTextCanvases.ContainsKey(attackId))
        {
            GameObject textContainer = Instantiate(textContainerPrefab, transform.position, Quaternion.identity);
            GameObject textParent = Instantiate(TextParentPrefab, textContainer.transform.position + new Vector3(0, baseOffsetY, 0), Quaternion.identity, textContainer.transform);
            damageTextCanvases[attackId] = textParent;
        }

        damageTaken.Add((damage, isCrit, attackId));
        ProcessDamage(attackId);
    }

    private void HandleDeath()
    {
        // Ensure that any ongoing processes are stopped or completed
        if (isDisplayingDamage)
        {
            StopAllCoroutines();
        }

    }

    // ProcessDamage is called whenever damage is taken, even if a display is ongoing.
    private void ProcessDamage(string attackId)
    {
        isDisplayingDamage = true;
        GameObject textParent = damageTextCanvases[attackId];

        if (!damageNumberCounts.ContainsKey(attackId))
        {
            damageNumberCounts[attackId] = 0;
        }

        var damagesForAttack = damageTaken.FindAll(d => d.AttackId == attackId);
        foreach (var damageInfo in damagesForAttack)
        {
            float dynamicYOffset = damageNumberCounts[attackId] == 0 ? baseOffsetY : baseOffsetY + incrementalOffsetY * damageNumberCounts[attackId];
            GameObject textPrefab = damageInfo.IsCrit ? DamageNumTextCrit : DamageNumText;
            GameObject textObject = Instantiate(textPrefab, textParent.transform.position + new Vector3(0, dynamicYOffset, 0), Quaternion.identity, textParent.transform);
            TMP_Text textComponent = textObject.GetComponent<TMP_Text>();
            textComponent.text = damageInfo.Damage.ToString();

            damageNumberCounts[attackId]++;
            damageTaken.Remove(damageInfo);
        }
        isDisplayingDamage = false;
    }
    private void DamageDisplay(GameObject canvas, (int Damage, bool IsCrit, string AttackId) damageInfo, Vector3 initialPosition)
    {
        // Check if the attack ID already has a count, if not initialize to 0
        if (!damageNumberCounts.ContainsKey(damageInfo.AttackId))
        {
            damageNumberCounts[damageInfo.AttackId] = 0;
        }

        // Calculate dynamicYOffset based on the count of damage numbers already displayed
        float dynamicYOffset = baseOffsetY * damageNumberCounts[damageInfo.AttackId];
        GameObject textPrefab = damageInfo.IsCrit ? DamageNumTextCrit : DamageNumText;
        GameObject text = Instantiate(
            textPrefab,
            new Vector3(initialPosition.x, initialPosition.y + dynamicYOffset, initialPosition.z),
            Quaternion.identity,
            canvas.transform
        );

        DamageNumController controller = text.GetComponent<DamageNumController>();
        controller.SetDamageNum(damageInfo.Damage);

        // Increment the count for this attack ID
        damageNumberCounts[damageInfo.AttackId]++;
    }
    private void Knockback(Vector2 hitDirection, float KnockbackStr)
    {
        if (Time.time - lastHitTime < hitAnimationCooldown)
        {
            // If we're within the cooldown period, exit the method without replaying the animation
            return;
        }

        // Determine knockback direction based on hitDirection rather than the enemy's current velocity
        Vector2 knockbackDirection = hitDirection.x < 0 ? Vector2.right : Vector2.left;
        bool originallyMovingRight = movingRight;

        PlayHitAnimation();

        // Apply knockback
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * KnockbackStr, ForceMode2D.Impulse);
        StartCoroutine(ResetEnemyMovementAfterKnockback(originallyMovingRight));
        initialPosition = transform.position + new Vector3(0, 0.25f, 0);

    }

    private IEnumerator ResetEnemyMovementAfterKnockback(bool originallyMovingRight)
    {
        yield return new WaitForSeconds(0.1f); // Adjust as needed

        // Restore the original moving direction
        movingRight = originallyMovingRight;

        // Reset velocity according to the original direction
        Vector3 correctedVelocityDirection = movingRight ? Vector3.right : Vector3.left;
        rb.velocity = correctedVelocityDirection * speed;

        // Correctly flip the sprite according to the restored direction
        enemySprite.flipX = !movingRight;
    }

    private void PlayHitAnimation()
    {
        if (health <= 0 && !isDying)
        {
            Die();
            return;
        }
        // Update the timestamp of the last hit
        lastHitTime = Time.time;

        // Now we proceed with playing the hit animation as before
        if (isInHitAnimation)
            return; // Still exit if we're already in the hit animation to avoid other issues

        isInHitAnimation = true; // Set the flag to true since we're going to play the hit animation

        // Pause the animator by setting its speed to 0
        animator.speed = 0;

        // Get the current frame of the Move animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        lastNormalizedTime = stateInfo.normalizedTime % 1; // Use modulus to loop between 0 and 1

        // Play the Hit animation at the exact same frame as the Move animation
        animator.Play("Hit", 0, lastNormalizedTime);


        // Schedule to return to the Move animation at the same frame in the next frame
        StartCoroutine(ResetToMoveAnimation());
    }

    private IEnumerator ResetToMoveAnimation()
    {
        // Define the duration in seconds for how long you want the hit animation to stay
        float timeToWait = 0.3f;

        // Wait for the defined duration
        yield return new WaitForSeconds(timeToWait);
        animator.speed = 1;

        // Reset the takingDamage flag and return to Move animation
        animator.SetBool("takingDamage", false);
        isInHitAnimation = false; // Reset the flag

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // Make sure we're still in the Hit animation before switching back
        if (stateInfo.IsName("Hit"))
        {
            lastNormalizedTime = stateInfo.normalizedTime % 1; // Use modulus to loop between 0 and 1
            animator.Play("Move", 0, lastNormalizedTime);
        }
    }

}

