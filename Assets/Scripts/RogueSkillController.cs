using System.Collections;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RogueSkillController : PlayerMovement
{
    public float MovementSkillForce;
    public GameObject basicAttackPrefab;
    public GameObject projectile2;

    [SerializeField]
    private GameObject SummonShuriken;
    public Transform attackPos;
    public Transform attackPosAirborne;
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
    private float cooldownTimeMovement = 1;
    private float nextFireTimeMovement = 0;

    private float AttackSpeed = 1f;
    private float nextFireTimeSkill1 = 0;

    private float cooldownTimeSkill2 = 2;
    private float nextFireTimeSkill2 = 0;

    private static float cooldownTimeSkill3 = 23;
    private float nextFireTimeSkill3 = 0;

    private float cooldownTimeSkill3Upgraded;

    private float cooldownTimeSkillUlt = 2;
    //private float nextFireTimeSkillUlt = 0;

    public Animator SwipeOne;
    public Animator MovementSkillTwo;
    private Renderer rend;
    private Color c;

    private float cooldownTimerS1 = 0.0f;
    private float cooldownTimerS2 = 0.0f;
    private float cooldownTimerS3 = 0.0f;
    private float cooldownTimerSM = 0.0f;
    private float cooldownTimerSU = 0.0f;

    // private readonly float cooldownTimer = 0.0f;

    [SerializeField]
    private float yOffsetSummon;

    [SerializeField]
    private float skillDuration;

    [SerializeField]
    private Transform LevelUpAttackTransform;

    private float tempSpeed;

    [SerializeField]
    private float ForceAirborne;

    [SerializeField]
    private float ForceBackward;

    [SerializeField]
    private float speedWhileThrowing;

    [SerializeField]
    private float FloatTime;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    private void Start()
    {
        HealthBar healthBar = FindObjectOfType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }

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
        tempSpeed = moveSpeed;
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        GetFirstSkillInput();
        GetSecondSkillInput();
        GetThirdSkillInput();
        GetUltimateSkillInput();
    }

    protected new void Update()
    {
        base.Update();
        experienceBar.setMaxExp(maxExp);

        levelUI.text = level.ToString();
        maxHealth = level * 100;
        maxExp = level * 500;
        experienceBar.SetExperience(currentExp);
        cooldownTimeSkill3Upgraded = cooldownTimeSkill3 - skillThreeLevel;
        // skillLevel1Text.text = skillOneLevel.ToString();
        //  skillLevel2Text.text = skillTwoLevel.ToString();
        //  skillLevel3Text.text = skillThreeLevel.ToString();
        //  skillUltText.text = ultSkillLevel.ToString();
        //  HealthDisplayText.text = $"{currentHealth} / {maxHealth}";
        //  coinCount.text = coins.ToString();
        if (GameManager.Instance.playerCanMove)
        {
            //Get player inputs
            setPlayerDirection();
            getPlayerInput();
            GetMovementSkillInput();

            //  horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

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

    public void setAttackSpeed(float attackSpeed){
        AttackSpeed = attackSpeed;
    }
    // Movement Skill
    private void GetMovementSkillInput()
    {
        if (
            Time.time > nextFireTimeMovement
            && isAirborne & combatActions.MovementSkill.action.triggered
        )
        {
            MovementSkill();
            nextFireTimeMovement = Time.time + cooldownTimeMovement;
            //    textCooldownSM.gameObject.SetActive(true);
            cooldownTimerSM = cooldownTimeMovement;
        }
    }

    private void SwitchMovePositionBasedOnMouse(bool isSkillActive)
    {
        if (!isSkillActive)
            return;

        float mousePositionInWorld = combatActions.MousePosition.action.ReadValue<Vector2>().x;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(mousePositionInWorld, 0, 0)
        );
        bool shouldFlip =
            (mouseWorldPosition.x < transform.position.x && facingRight)
            || (mouseWorldPosition.x > transform.position.x && !facingRight);

        if (shouldFlip)
        {
            FlipCharacter();
        }
    }

    public void MovementSkill()
    {
        MovementSkillTwo.SetBool("MovementSkillUsed", true);
        gameObject.layer = LayerMask.NameToLayer("PlayerFallThrough");
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
        _ = StartCoroutine(ResetMovementSkillAnimation());
    }

    private IEnumerator ResetMovementSkillAnimation()
    {
        yield return new WaitForSeconds(0.35f);
        MovementSkillTwo.SetBool("MovementSkillUsed", false);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void ApplyCooldownTracker()
    {
        cooldownTimerS1 -= Time.deltaTime;
        cooldownTimerS2 -= Time.deltaTime;
        cooldownTimerS3 -= Time.deltaTime;
        cooldownTimerSM -= Time.deltaTime;
        cooldownTimerSU -= Time.deltaTime;

        UpdateCooldownTimer(textCooldownS1, imageCooldownS1, cooldownTimerS1, AttackSpeed);
        UpdateCooldownTimer(textCooldownS2, imageCooldownS2, cooldownTimerS2, cooldownTimeSkill2);
        UpdateCooldownTimer(
            textCooldownS3,
            imageCooldownS3,
            cooldownTimerS3,
            cooldownTimeSkill3Upgraded
        );
        UpdateCooldownTimer(textCooldownSU, imageCooldownSU, cooldownTimerSU, cooldownTimeSkillUlt);
        UpdateCooldownTimer(textCooldownSM, imageCooldownSM, cooldownTimerSM, cooldownTimeMovement);
    }

    private void UpdateCooldownTimer(
        TMP_Text textCooldown,
        Image imageCooldown,
        float cooldownTimer,
        float cooldownTime
    )
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
    private void GetFirstSkillInput()
    {
        // Check if it's time to fire again and the skill action is pressed
        if (Time.time > nextFireTimeSkill1 && combatActions.BasicSkill.action.IsPressed())
        {
            if (!isExecutingSkill)
            {
                // GameManager.Instance.playerCanMove = false; // Lock movement when starting skill
                isExecutingSkill = true;
                SwitchMovePositionBasedOnMouse(true);
                setPlayerDirection();
                if (!isAirborne)
                {
                    moveSpeed = speedWhileThrowing;
                }
                StartCoroutine(FirstSkill());
                nextFireTimeSkill1 = Time.time + AttackSpeed;
                cooldownTimerS1 = AttackSpeed;
                SwipeOne.SetTrigger("Attack");
            }
        }
    }

    private IEnumerator FirstSkill()
    {
        Vector3 fixedAttackPosition = !isAirborne ? attackPos.position : attackPosAirborne.position;
        Quaternion fixedAttackRotation = !isAirborne
            ? attackPos.rotation
            : attackPosAirborne.rotation;

        Instantiate(
            basicAttackPrefab,
            fixedAttackPosition,
            fixedAttackRotation,
            ContainerManager.Instance.ProjectileContainer
        );

        yield return new WaitForSeconds(skillDuration); // Ensure player is immobilized for the duration of the skill

        isExecutingSkill = false;
        moveSpeed = tempSpeed;
        //    GameManager.Instance.playerCanMove = true; // Re-enable movement after skill completes
    }

    public void GetSecondSkillInput()
    {
        if (Time.time > nextFireTimeSkill2 && combatActions.AoeSkill.action.IsPressed())
        {
            GameManager.Instance.playerCanMove = false; // Lock movement if starting skill stationary
            isExecutingSkill = true;
            SwitchMovePositionBasedOnMouse(true);
            setPlayerDirection();
            animator.SetTrigger("isAttacking");
            _ = StartCoroutine(secondSkill());
            nextFireTimeSkill2 = Time.time + cooldownTimeSkill2;
            cooldownTimerS2 = cooldownTimeSkill2;

            Vector2 forceDirection = facingRight ? Vector2.left : Vector2.right;

            if (!isAirborne)
            {
                // Apply force backward on the ground
                rb.AddForce(forceDirection * ForceBackward, ForceMode2D.Impulse);
            }
            else
            {
                // Freeze Y velocity while airborne and apply force backward
                StartCoroutine(FreezeYVelocityDuringForce(forceDirection * ForceAirborne));
            }
        }
    }

    private IEnumerator secondSkill()
    {
        yield return new WaitForSeconds(0.20f);
        _ = Instantiate(
            projectile2,
            attackPos.position,
            attackPos.rotation,
            ContainerManager.Instance.ProjectileContainer
        );
        GameManager.Instance.playerCanMove = true; // Lock movement if starting skill stationary
        isExecutingSkill = false;
        moveSpeed = tempSpeed;
    }

    private IEnumerator FreezeYVelocityDuringForce(Vector2 force)
    {
        // Store the current Y velocity
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        // Apply the force with Y velocity set to 0
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
        // Wait for a short duration while the force is applied
        yield return new WaitForSeconds(FloatTime); // Adjust the duration based on the desired effect
        rb.gravityScale = originalGravity;
    }

    // Third Skill
    public void GetThirdSkillInput()
    {
        if (
            Time.time > nextFireTimeSkill3
            && combatActions.SummonSkill.action.IsPressed()
            && !isAirborne
        )
        {
            _ = StartCoroutine(ThirdSkillEnum());
            nextFireTimeSkill3 = Time.time + cooldownTimeSkill3Upgraded;
            //     textCooldownS3.gameObject.SetActive(true);
            cooldownTimerS3 = cooldownTimeSkill3Upgraded;
        }
    }

    private IEnumerator ThirdSkillEnum()
    {
        rb.velocity = Vector2.zero;
        GameManager.Instance.playerCanMove = false;
        yield return new WaitForSeconds(1f);
        GameObject SummonSkillParent = new("SummonSkill");
        SummonSkillParent.transform.position =
            gameObject.transform.position + new Vector3(0, yOffsetSummon, 0);
        _ = Instantiate(
            SummonShuriken,
            SummonSkillParent.transform.position,
            SummonSkillParent.transform.rotation,
            SummonSkillParent.transform
        );
        GameManager.Instance.playerCanMove = true;
    }

    // Ultimate Skill
    public void GetUltimateSkillInput()
    {
        // if (Time.time > nextFireTimeSkillUlt && Input.GetKeyDown(KeyCode.F))
        // {
        //     _ = StartCoroutine(UltimateSkillEnum());
        //     nextFireTimeSkillUlt = Time.time + cooldownTimeSkillUlt;
        //     //   textCooldownSU.gameObject.SetActive(true);
        //     cooldownTimerSU = cooldownTimeSkillUlt;
        // }
    }

    private IEnumerator UltimateSkillEnum()
    {
        _ = Instantiate(
            LevelUpShuriken,
            LevelUpAttackTransform.position,
            Quaternion.identity,
            gameObject.transform
        );
        yield return new WaitForSeconds(0.15f);
    }
}
