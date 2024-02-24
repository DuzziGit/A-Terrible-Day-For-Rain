using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Unity.Mathematics;
public class RogueSkillController : PlayerMovement
{
    public float MovementSkillForce;
    public float MovementSkillForceLeft;
    public GameObject LevelUpShuriken;
    [SerializeField] private GameObject JumpShuriken;
    [SerializeField] private GameObject FixedJumpAttackPos;
    public GameObject basicAttackPrefab;
    public GameObject projectile2;
    public GameObject ProjectileUltimate;
    [SerializeField] private GameObject SummonShuriken;
    public Transform attackPos;
    public Transform attackPosAirborne;

    public float horizontalMove = 0f;
    public float runSpeed = 40f;

    private readonly Image imageCooldownS1;
    private readonly TMP_Text textCooldownS1;
    private readonly Image imageCooldownS2;
    private readonly TMP_Text textCooldownS2;
    private readonly Image imageCooldownS3;
    private readonly TMP_Text textCooldownS3;
    private readonly Image imageCooldownSM;
    private readonly TMP_Text textCooldownSM;
    private readonly Image imageCooldownSU;
    private readonly TMP_Text textCooldownSU;

    public bool isInvincible;
    public float cooldownTimeMovement = 2;
    private float nextFireTimeMovement = 0;

    public float cooldownTimeSkill1 = 2;
    private float nextFireTimeSkill1 = 0;

    public float cooldownTimeSkill2 = 2;
    private float nextFireTimeSkill2 = 0;

    public static float cooldownTimeSkill3 = 23;
    private float nextFireTimeSkill3 = 0;

    public float cooldownTimeSkill3Upgraded;

    public float cooldownTimeSkillUlt = 2;
    private float nextFireTimeSkillUlt = 0;

    public Animator SwipeOne;
    public Animator SwipeTwo;
    public Animator MovementSkillTwo;
    private Renderer rend;
    private Color c;

    private float cooldownTimerS1 = 0.0f;
    private float cooldownTimerS2 = 0.0f;
    private float cooldownTimerS3 = 0.0f;
    private float cooldownTimerSM = 0.0f;
    private float cooldownTimerSU = 0.0f;
    // private readonly float cooldownTimer = 0.0f;
    private CinemachineImpulseSource impulseSource;
    [SerializeField] private float yOffsetSummon;

    private void Start()
    {
        HealthBar healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        impulseSource = GetComponent<CinemachineImpulseSource>();

        //   textCooldownS1.gameObject.SetActive(false);
        //   imageCooldownS1.fillAmount = 0.0f;
        //   textCooldownS2.gameObject.SetActive(false);
        //  imageCooldownS2.fillAmount = 0.0f;
        // textCooldownS3.gameObject.SetActive(false);
        //  imageCooldownS3.fillAmount = 0.0f;
        //  textCooldownSM.gameObject.SetActive(false);
        //  imageCooldownSM.fillAmount = 0.0f;
        //  textCooldownSU.gameObject.SetActive(false);
        //  imageCooldownSU.fillAmount = 0.0f;
        currentExp = 0;

        rend = GetComponent<Renderer>();
        c = rend.material.color;
    }

    private void Update()
    {

        experienceBar.setMaxExp(maxExp);

        levelUI.text = level.ToString();
        maxHealth = level * 100;
        maxExp = level * 500;

        experienceBar.SetExperience(currentExp);
        cooldownTimeSkill3Upgraded = cooldownTimeSkill3 - GetComponent<PlayerMovement>().skillThreeLevel;
        // skillLevel1Text.text = skillOneLevel.ToString();
        //  skillLevel2Text.text = skillTwoLevel.ToString();
        //  skillLevel3Text.text = skillThreeLevel.ToString();
        //  skillUltText.text = ultSkillLevel.ToString();
        //  HealthDisplayText.text = $"{currentHealth} / {maxHealth}";
        //  coinCount.text = coins.ToString();
        if (GameController.instance.playerCanMove)
        {
            //Get player inputs
            getPlayerInput();
            playerInteractInput();

            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            GetMovementSkillInput();
            GetFirstSkillInput();
            GetSecondSkillInput();
            GetThirdSkillInput();
            GetUltimateSkillInput();
            LevelUp();

            //Animate
            animate();
            ApplyCooldownTracker();

            if (!isAirborne)
            {
                nextFireTimeMovement = 0;
            }
        }
    }

    public override void LevelUp()
    {
        if (level < 60 && shouldLevelUp)
        {
            IncreaseLevel();
            _ = Instantiate(LevelUpShuriken, transform);
            CameraShakeManager.instance.CameraShake(impulseSource);
            shouldLevelUp = false;
        }
    }
    // Movement Skill
    private void GetMovementSkillInput()
    {
        if (Time.time > nextFireTimeMovement && Input.GetKeyDown(KeyCode.LeftControl) && isAirborne)
        {
            MovementSkill();
            nextFireTimeMovement = Time.time + cooldownTimeMovement;
            //    textCooldownSM.gameObject.SetActive(true);
            cooldownTimerSM = cooldownTimeMovement;

        }
    }

    public void MovementSkill()
    {
        MovementSkillTwo.SetBool("MovementSkillUsed", true);

        // Determine the force direction based on facing direction
        float forceDirection = facingRight ? 1.0f : -1.0f;

        // Reset vertical velocity to 0 or set to a specific value before applying the impulse
        rb.velocity = new Vector2(0, 0); // This line neutralizes any existing vertical motion

        // Apply horizontal force directly to control direction more precisely
        Vector2 horizontalForce = new(forceDirection * MovementSkillForce, 0);
        rb.AddForce(horizontalForce, ForceMode2D.Impulse);

        // Apply vertical force separately to ensure it's consistent
        Vector2 verticalForce = new(0, jumpForce);
        rb.AddForce(verticalForce, ForceMode2D.Impulse);

        _ = StartCoroutine(ResetMovementSkillAnimation());
    }

    private void GetMovementSkillUpInput()
    {
        if (Time.time > nextFireTimeMovement && Input.GetKeyDown(KeyCode.LeftShift) && isAirborne)
        {
            MovementSkillUpwards();
            nextFireTimeMovement = Time.time + cooldownTimeMovement;
            //    textCooldownSM.gameObject.SetActive(true);
            cooldownTimerSM = cooldownTimeMovement;

        }
    }
    public void MovementSkillUpwards()
    {
        rb.velocity = new Vector2(0, 0);
        // Apply vertical force separately to ensure it's consistent
        Vector2 verticalForce = new(0, jumpForce);
        rb.AddForce(verticalForce, ForceMode2D.Impulse);
        Instantiate(JumpShuriken, FixedJumpAttackPos.transform.position, FixedJumpAttackPos.transform.rotation);
        _ = StartCoroutine(ResetMovementSkillAnimation());
    }

    private IEnumerator ResetMovementSkillAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        MovementSkillTwo.SetBool("MovementSkillUsed", false);
    }

    public void ApplyCooldownTracker()
    {
        cooldownTimerS1 -= Time.deltaTime;
        cooldownTimerS2 -= Time.deltaTime;
        cooldownTimerS3 -= Time.deltaTime;
        cooldownTimerSM -= Time.deltaTime;
        cooldownTimerSU -= Time.deltaTime;

        UpdateCooldownTimer(textCooldownS1, imageCooldownS1, cooldownTimerS1, cooldownTimeSkill1);
        UpdateCooldownTimer(textCooldownS2, imageCooldownS2, cooldownTimerS2, cooldownTimeSkill2);
        UpdateCooldownTimer(textCooldownS3, imageCooldownS3, cooldownTimerS3, cooldownTimeSkill3Upgraded);
        UpdateCooldownTimer(textCooldownSU, imageCooldownSU, cooldownTimerSU, cooldownTimeSkillUlt);
        UpdateCooldownTimer(textCooldownSM, imageCooldownSM, cooldownTimerSM, cooldownTimeMovement);
    }

    private void UpdateCooldownTimer(TMP_Text textCooldown, Image imageCooldown, float cooldownTimer, float cooldownTime)
    {
        if (cooldownTimer < 0.0f)
        {
            //      textCooldown.gameObject.SetActive(false);
            //        imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            //      textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            //      imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    // First Skill
    public void GetFirstSkillInput()
    {
        if (Time.time > nextFireTimeSkill1 && Input.GetKey(KeyCode.A))
        {
            isExecutingSkill = true;
            if (!isAirborne)
            {
                rb.velocity = Vector2.zero; // Stop any current movement
                moveDirection = 0;
                GameController.instance.playerCanMove = false; // Disable movement input
            }
            else
            {
                GameController.instance.playerCanMove = false; // Disable movement input

            }

            StartCoroutine(FirstSkill());
            nextFireTimeSkill1 = Time.time + cooldownTimeSkill1;
            //    textCooldownS1.gameObject.SetActive(true);
            cooldownTimerS1 = cooldownTimeSkill1;
            SwipeOne.SetTrigger("Attack");
        }
    }
    private IEnumerator FirstSkill()
    {


        // If the player is airborne, don't modify their horizontal velocity,
        // allowing them to continue moving with their current momentum.

        // Determine the fixed position for the attack based on whether the player is airborne or not
        Vector3 fixedAttackPosition = !isAirborne ? attackPos.position : attackPosAirborne.position;
        Quaternion fixedAttackRotation = !isAirborne ? attackPos.rotation : attackPosAirborne.rotation;

        // Instantiate the attack prefab at the calculated position and rotation
        Instantiate(basicAttackPrefab, fixedAttackPosition, fixedAttackRotation);

        // Wait for a short duration before continuing
        yield return new WaitForSeconds(0.4f); // Adjust based on skill animation length

        isExecutingSkill = false;
        GameController.instance.playerCanMove = true;
    }


    public void GetSecondSkillInput()
    {
        if (Time.time > nextFireTimeSkill2 && Input.GetKeyDown(KeyCode.S))
        {
            rb.isKinematic = true;
            animator.SetTrigger("isAttacking");
            _ = StartCoroutine(secondSkill());
            nextFireTimeSkill2 = Time.time + cooldownTimeSkill2;
            //   textCooldownS2.gameObject.SetActive(true);
            cooldownTimerS2 = cooldownTimeSkill2;
            rb.isKinematic = false;
        }
    }

    private IEnumerator secondSkill()
    {
        yield return new WaitForSeconds(0.20f);
        _ = Instantiate(projectile2, attackPos.position, attackPos.rotation);
    }

    // Third Skill
    public void GetThirdSkillInput()
    {
        if (Time.time > nextFireTimeSkill3 && Input.GetKeyDown(KeyCode.D))
        {
            _ = StartCoroutine(ThirdSkillEnum());
            nextFireTimeSkill3 = Time.time + cooldownTimeSkill3Upgraded;
            //     textCooldownS3.gameObject.SetActive(true);
            cooldownTimerS3 = cooldownTimeSkill3Upgraded;
        }
    }

    private IEnumerator ThirdSkillEnum()
    {
        GameObject SummonSkillParent = new("SummonSkill");
        SummonSkillParent.transform.position = gameObject.transform.position + new Vector3(0, yOffsetSummon, 0);
        _ = Instantiate(SummonShuriken, SummonSkillParent.transform.position, SummonSkillParent.transform.rotation, SummonSkillParent.transform);
        Physics2D.IgnoreLayerCollision(7, 11, true);
        yield return new WaitForSeconds(2.5f);
        Physics2D.IgnoreLayerCollision(7, 11, false);
    }

    // Ultimate Skill
    public void GetUltimateSkillInput()
    {
        if (Time.time > nextFireTimeSkillUlt && Input.GetKeyDown(KeyCode.F))
        {
            _ = StartCoroutine(UltimateSkillEnum());
            nextFireTimeSkillUlt = Time.time + cooldownTimeSkillUlt;
            //   textCooldownSU.gameObject.SetActive(true);
            cooldownTimerSU = cooldownTimeSkillUlt;
        }
    }

    private IEnumerator UltimateSkillEnum()
    {
        _ = Instantiate(LevelUpShuriken, transform);
        yield return new WaitForSeconds(0.15f);
    }
}
