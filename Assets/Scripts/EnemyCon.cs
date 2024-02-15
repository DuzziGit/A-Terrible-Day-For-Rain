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

    private void ProcessDamage()
    {
        if (damageCanvas == null)
        {
            damageCanvas = Instantiate(CanvasDamageNum, transform.position, Quaternion.identity);
        }

        if (!isDisplayingDamage)
        {
            _ = StartCoroutine(DamageDisplay(damageCanvas));
        }
    }

    private IEnumerator DamageDisplay(GameObject canvas)
    {
        isDisplayingDamage = true;
        float tempBounds = bc.bounds.max.y;
        float localYOffset = 0f; // Initialize localYOffset before the loop

        // Loop backwards through the list so you can remove items without affecting the loop index
        for (int i = DamageTaken.Count - 1; i >= 0; i--)
        {
            int damage = DamageTaken[i].Damage;
            bool isCrit = DamageTaken[i].IsCrit;

            GameObject textPrefab = isCrit ? DamageNumTextCrit : DamageNumText;
            GameObject text = Instantiate(
                textPrefab,
                new Vector3(transform.position.x, tempBounds + yOffset + localYOffset, transform.position.z),
                Quaternion.identity,
                canvas.transform
            );

            DamageNumController controller = text.GetComponent<DamageNumController>();
            controller.SetDamageNum(damage);

            localYOffset += 1.2f; // Increment the offset after each damage number

            DamageTaken.RemoveAt(i); // Safe to remove since we are not using foreach
            yield return new WaitForSeconds(damageDisplayDelay);
        }

        isDisplayingDamage = false;

        // After exiting the loop, reset the localYOffset for the next time damage is taken
        localYOffset = 0f;
    }




    private IEnumerator ResetTakeDamageTrigger()
    {
        yield return new WaitForSeconds(resetTriggerDelay);
        animator.SetBool("takingDamage", false);

    }
}
