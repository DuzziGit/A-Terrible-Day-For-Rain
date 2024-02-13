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
    private GameObject damageCanvas;// Track the canvas instance
    public bool isDisplayingDamage = false;// Flag to track if damage is currently being displayed


    // Magic numbers replaced with constants
    private const float xOffset = 0f;
    private const float yOffset = 0.4f;
    private const float damageDisplayDelay = 0.1f;
    private const float resetTriggerDelay = 0.2f;
    private readonly List<int> DamageTaken = new();

    private void Start()
    {
        rb.velocity = new Vector3(speed, 0, 0);
        enemyLevel.text = level.ToString();
        enemyDamage = level * 5;

        animator = GetComponent<Animator>();
        healthBar = GetComponentInChildren<HealthBar>();
        maxHealth = health;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    private void Update()
    {
    }

    public void TakeDamage(int damage)
    {

        health = Mathf.Max(0, health - damage);

        DamageTaken.Add(damage);

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

        while (DamageTaken.Count > 0)
        {
            int damage = DamageTaken[0];
            GameObject text = Instantiate(DamageNumText, new Vector3(transform.position.x, tempBounds + yOffset, transform.position.z), Quaternion.identity, canvas.transform);
            DamageNumController controller = text.GetComponent<DamageNumController>();
            controller.SetDamageNum(damage);
            DamageTaken.RemoveAt(0);
            yield return new WaitForSeconds(damageDisplayDelay);
        }
        isDisplayingDamage = false;

        // Check if there are no more damage numbers to display and if canvas and its DamageText component still exist
        if (DamageTaken.Count == 0 && canvas != null)
        {
            DamageText damageTextScript = canvas.GetComponent<DamageText>();
            if (damageTextScript != null) // Check if the DamageText component is not null
            {
                // StartCoroutine(damageTextScript.ShowAndDestroy());
            }
        }
    }





    private IEnumerator ResetTakeDamageTrigger()
    {
        yield return new WaitForSeconds(resetTriggerDelay);
        animator.SetBool("takingDamage", false);

    }
}
