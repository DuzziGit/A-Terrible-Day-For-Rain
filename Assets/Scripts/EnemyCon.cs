using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCon : Enemy
{

    public int enemyDamage;
    public TMP_Text damageDisplay;
    [SerializeField] private float KnockbackStr;
    public TMP_Text enemyLevel;
    public GameObject CanvasDamageNum;
    public new bool isTouchingPlayer = false;
    private HealthBar healthBar;
    private int maxHealth;
    [SerializeField] private GameObject DamageNumText;
    [SerializeField] private GameObject DamageNumTextCrit;
    private float lastNormalizedTime;
    private GameObject damageCanvas;// Track the canvas instance
    public bool isDisplayingDamage = false;// Flag to track if damage is currently being displayed
    private const int baseEnemyHealth = 10000;
    private const float healthGrowthRate = 1.1f; // This can be adjusted

    // Magic numbers replaced with constants
    private const float xOffset = 0f;
    [SerializeField] private float yOffsetText = 0.2f;
    private const float damageDisplayDelay = 0.1f;
    private const float resetTriggerDelay = 0.2f;
    private List<(int Damage, bool IsCrit, string AttackId)> DamageTaken = new List<(int Damage, bool IsCrit, string AttackId)>();
    private Dictionary<string, GameObject> attackCanvases = new Dictionary<string, GameObject>();

    private bool isInHitAnimation = false; // Flag to indicate if the hit animation is currently playing

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

    private void Update()
    {
    }

    public void TakeDamage(int damage, bool isCrit, string attackId)
    {

        health = Mathf.Max(0, health - damage);

        if (!isDisplayingDamage)
        {
            ProcessDamage(attackId);
        }


        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        PlayHitAnimation();
        if (!attackCanvases.ContainsKey(attackId))
        {
            GameObject newCanvas = Instantiate(CanvasDamageNum, transform.position, Quaternion.identity);
            attackCanvases[attackId] = newCanvas;
        }

        // Add damage to the list with attackId
        DamageTaken.Add((damage, isCrit, attackId)); // Note: You need to update DamageTaken to include attackId

        if (!isDisplayingDamage)
        {
            StartCoroutine(ProcessDamage(attackId)); // Use StartCoroutine to call Coroutines
        }
    }

    // ProcessDamage is called whenever damage is taken, even if a display is ongoing.
    private IEnumerator ProcessDamage(string attackId)
    {
        isDisplayingDamage = true;
        GameObject canvas = attackCanvases.ContainsKey(attackId) ? attackCanvases[attackId] : null;

        if (canvas == null)
        {
            canvas = Instantiate(CanvasDamageNum, transform.position, Quaternion.identity);
            attackCanvases[attackId] = canvas;
        }

        // Filter for current attackId damages
        var damagesForAttack = DamageTaken.FindAll(d => d.AttackId == attackId);
        foreach (var damageInfo in damagesForAttack)
        {
            DamageDisplay(canvas, damageInfo);
            DamageTaken.Remove(damageInfo); // Remove after displaying
            yield return new WaitForSeconds(damageDisplayDelay); // Wait for delay between damage numbers
        }

        isDisplayingDamage = false;
    }

    // Updated DamageDisplay coroutine to continuously monitor and display damage.
    private void DamageDisplay(GameObject canvas, (int Damage, bool IsCrit, string AttackId) damageInfo)
    {
        float dynamicYOffset = yOffsetText * canvas.transform.childCount;
        GameObject textPrefab = damageInfo.IsCrit ? DamageNumTextCrit : DamageNumText;
        GameObject text = Instantiate(
            textPrefab,
            new Vector3(transform.position.x + xOffset, transform.position.y + dynamicYOffset, transform.position.z),
            Quaternion.identity,
            canvas.transform
        );

        DamageNumController controller = text.GetComponent<DamageNumController>();
        controller.SetDamageNum(damageInfo.Damage);
    }


    private void PlayHitAnimation()
    {
        if (isInHitAnimation)
            return; // Exit if we're already in the hit animation

        isInHitAnimation = true; // Set the flag to true since we're going to play the hit animation

        // Get the current frame of the Move animation
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        lastNormalizedTime = stateInfo.normalizedTime % 1; // Use modulus to loop between 0 and 1

        // Play the Hit animation at the exact same frame as the Move animation
        animator.Play("Hit", 0, lastNormalizedTime);
        Knockback();
        // Schedule to return to the Move animation at the same frame in the next frame
        StartCoroutine(ResetToMoveAnimation());
    }

    private void Knockback()
    {
        Vector2 knockbackDirection = rb.velocity.x >= 0 ? Vector2.left : Vector2.right;
        // Consider storing the original moving direction before applying knockback
        bool originallyMovingRight = movingRight;

        // Apply knockback
        rb.velocity = Vector2.zero;
        rb.AddForce(knockbackDirection * KnockbackStr, ForceMode2D.Impulse);

        // Pass the original direction to the reset coroutine
        StartCoroutine(ResetEnemyMovementAfterKnockback(originallyMovingRight));
    }

    private IEnumerator ResetEnemyMovementAfterKnockback(bool originallyMovingRight)
    {
        yield return new WaitForSeconds(0.2f); // Adjust as needed

        // Restore the original moving direction
        movingRight = originallyMovingRight;

        // Reset velocity according to the original direction
        Vector3 correctedVelocityDirection = movingRight ? Vector3.right : Vector3.left;
        rb.velocity = correctedVelocityDirection * speed;

        // Correctly flip the sprite according to the restored direction
        enemySprite.flipX = !movingRight;
    }
    private IEnumerator ResetToMoveAnimation()
    {
        // Define the duration in seconds for how long you want the hit animation to stay
        float timeToWait = 0.8f; // Adjust this to match the desired hit animation time

        // Wait for the defined duration
        yield return new WaitForSeconds(timeToWait);

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

