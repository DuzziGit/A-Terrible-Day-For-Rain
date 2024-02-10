using System.Collections;
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
    public bool isTouchingPlayer = false;
    private HealthBar healthBar;
    public Animator animFeedback;
    public Animator animFeedback2;
    private int maxHealth;

    // Magic numbers replaced with constants
    private const float xOffset = 0f;
    private const float yOffset = 0.0f;
    private const float damageDisplayDelay = 0.1f;
    private const float resetTriggerDelay = 0.2f;

    void Start()
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

    void Update()
    {
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);
        StartCoroutine(DamageDisplay(damage));
        if (healthBar != null)
        {
            healthBar.SetHealth(health);
        }
        AudioController.instance.PlayMonsterHurtSound();
        animator.SetBool("takingDamage", true);
        animFeedback.SetBool("takingDamage", true);
        animFeedback2.SetBool("takingDamage", true);

        StartCoroutine(ResetTakeDamageTrigger());
    }

    IEnumerator DamageDisplay(int damage)
    {
        Vector3 positionOffset = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset + Random.Range(1.0f, 3.0f), transform.position.z);
        GameObject text = Instantiate(CanvasDamageNum, positionOffset, Quaternion.identity);

        DamageNumController controller = text.GetComponentInChildren<DamageNumController>();

        controller.SetDamageNum(damage);
        yield return new WaitForSeconds(damageDisplayDelay);
    }

    private IEnumerator ResetTakeDamageTrigger()
    {
        yield return new WaitForSeconds(resetTriggerDelay);
        animator.SetBool("takingDamage", false);
        animFeedback.SetBool("takingDamage", false);
        animFeedback2.SetBool("takingDamage", false);
    }
}
