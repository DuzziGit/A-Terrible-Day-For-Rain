using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCon : Enemy
{
    public AudioSource audioSource;
    public AudioClip skeletonHitSound;
    public AudioClip deathSound;

    public int enemyDamage;
    public TMP_Text damageDisplay;
    public TMP_Text enemyLevel;
    public GameObject CanvasDamageNum;
    public new bool isTouchingPlayer = false;
    private HealthBar healthBar;
    private int maxHealth;
    [SerializeField] private GameObject DamageNumText;
    [SerializeField] private GameObject DamageNumTextCrit;

    private GameObject damageCanvas;// Track the canvas instance
    public bool isDisplayingDamage = false;// Flag to track if damage is currently being displayed
    private const int baseEnemyHealth = 10000;
    private const float healthGrowthRate = 1.1f; // This can be adjusted

    // Magic numbers replaced with constants
    private const float xOffset = 0f;
    [SerializeField] private float yOffset = 0.2f;
    private const float damageDisplayDelay = 0.1f;
    private const float resetTriggerDelay = 0.2f;
    private List<(int Damage, bool IsCrit)> DamageTaken = new List<(int Damage, bool IsCrit)>();

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

    public void TakeDamage(int damage, bool isCrit)
    {

        health = Mathf.Max(0, health - damage);

        DamageTaken.Add((damage, isCrit));

        if (!isDisplayingDamage)
        {
            ProcessDamage();
        }


        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        AudioController.instance.PlayMonsterHurtSound();
        animator.SetBool("takingDamage", true);

        _ = StartCoroutine(ResetTakeDamageTrigger());
    }

    // ProcessDamage is called whenever damage is taken, even if a display is ongoing.
    private void ProcessDamage()
    {
        if (damageCanvas == null)
        {
            damageCanvas = Instantiate(CanvasDamageNum, transform.position, Quaternion.identity);
        }

        // Always attempt to start the DamageDisplay coroutine, but ensure it's designed to handle being called multiple times.
        DamageDisplay(damageCanvas);
    }

    // Updated DamageDisplay coroutine to continuously monitor and display damage.
    private void DamageDisplay(GameObject canvas)
    {
        isDisplayingDamage = true;

        while (DamageTaken.Count > 0)
        {
            // Calculate yOffset based on the number of currently active damage numbers.
            // This assumes each damage number needs a certain vertical space (e.g., yOffset).
            float dynamicYOffset = yOffset * canvas.transform.childCount;
            var damageInfo = DamageTaken[0];
            DamageTaken.RemoveAt(0); // Remove the processed damage info immediately

            GameObject textPrefab = damageInfo.IsCrit ? DamageNumTextCrit : DamageNumText;
            GameObject text = Instantiate(
                textPrefab,
                new Vector3(transform.position.x + xOffset, transform.position.y + dynamicYOffset, transform.position.z),
                Quaternion.identity,
                canvas.transform
            );

            DamageNumController controller = text.GetComponent<DamageNumController>();
            controller.SetDamageNum(damageInfo.Damage);

            // Here, no delay is needed between instantiations, as the yOffset handles spacing.
        }

        isDisplayingDamage = false;
    }


    private IEnumerator ResetTakeDamageTrigger()
    {
        yield return new WaitForSeconds(resetTriggerDelay);
        animator.SetBool("takingDamage", false);

    }
}
