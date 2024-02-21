using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class RogueSkillController : PlayerMovement
{
    public float MovementSkillForce;
    public float MovementSkillForceLeft;
    public GameObject LevelUpShuriken;

    public GameObject projectile;
    public GameObject projectile2;
    public GameObject ProjectileUltimate;
    [SerializeField] private GameObject SummonShuriken;
    public Transform attackPos;
    public Transform attackPosAirborne;


    public AudioClip ThrowingStarSoundEffect;
    public AudioClip BigShurikenSoundEffect;
    public AudioClip RogueUltimateSoundEffect;
    public AudioClip FlashJumpSoundEffect;

    [HideInInspector]
    public Transform RogueUltPos;
    [HideInInspector]
    public Transform RogueUltPos1;
    [HideInInspector]
    public Transform RogueUltPos2;
    [HideInInspector]
    public Transform RogueUltPos3;
    [HideInInspector]
    public Transform RogueUltPos4;
    [HideInInspector]
    public Transform RogueUltPos5;
    [HideInInspector]
    public Transform RogueUltPos6;
    [HideInInspector]
    public Transform RogueUltPos7;
    [HideInInspector]
    public Transform RogueUltPos8;
    [HideInInspector]
    public Transform RogueUltPos9;
    [HideInInspector]
    public Transform RogueUltPos10;
    [HideInInspector]
    public Transform RogueUltPos11;
    [HideInInspector]
    public Transform RogueUltPos12;
    [HideInInspector]
    public Transform RogueUltPos13;
    [HideInInspector]
    public Transform RogueUltPos14;

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
    public Animator MovementSkillOne;
    public Animator MovementSkillTwo;
    private Renderer rend;
    private Color c;

    private float cooldownTimerS1 = 0.0f;
    private float cooldownTimerS2 = 0.0f;
    private float cooldownTimerS3 = 0.0f;
    private float cooldownTimerSM = 0.0f;
    private float cooldownTimerSU = 0.0f;
    private readonly float cooldownTimer = 0.0f;
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
        maxExp = level * 200;

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

    private void FixedUpdate()
    {
        moveCharacter();
    }
    public override void LevelUp()
    {
        if (level < 60 && shouldLevelUp)
        {
            IncreaseLevel();
            _ = Instantiate(LevelUpShuriken, transform);
            CameraShakeManager.instance.CameraShake(impulseSource);
            shouldLevelUp = false;
            AudioController.instance.PlayLevelUpSound();
            leveledUpAnimator.SetTrigger("LeveledUp");
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
        MovementSkillOne.SetBool("MovementSkillUsed", true);
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

    private IEnumerator ResetMovementSkillAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        MovementSkillOne.SetBool("MovementSkillUsed", false);
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
        if (Time.time > nextFireTimeSkill1 && Input.GetKeyDown(KeyCode.A))
        {

            _ = StartCoroutine(FirstSkill());
            nextFireTimeSkill1 = Time.time + cooldownTimeSkill1;
            //    textCooldownS1.gameObject.SetActive(true);
            cooldownTimerS1 = cooldownTimeSkill1;
            SwipeOne.SetBool("SwipeOne", true);
            SwipeTwo.SetBool("SwipeTwo", true);

        }
    }
    private IEnumerator FirstSkill()
    {
        // Instantiate a fixed attack position GameObject
        GameObject fixedAttackPos = new("FixedAttackPosition");
        if (!isAirborne)
        {
            fixedAttackPos.transform.position = attackPos.position;
            fixedAttackPos.transform.rotation = attackPos.rotation;
        }
        else
        {
            fixedAttackPos.transform.position = attackPosAirborne.position;
            fixedAttackPos.transform.rotation = attackPosAirborne.rotation;
        }




        // First shuriken
        Vector3 topOffset = new(0, 0.2f, 0);
        _ = Instantiate(projectile, fixedAttackPos.transform.position + topOffset, fixedAttackPos.transform.rotation);
        audioSource.pitch = 1.6f;  // Reduced pitch
        audioSource.PlayOneShot(ThrowingStarSoundEffect, 2f);
        yield return new WaitForSeconds(0.05f);

        // Second shuriken
        _ = Instantiate(projectile, fixedAttackPos.transform.position, fixedAttackPos.transform.rotation);
        audioSource.pitch = 1.0f;  // Normal pitch
        audioSource.PlayOneShot(ThrowingStarSoundEffect, 2f);
        yield return new WaitForSeconds(0.05f);

        // Third shuriken
        Vector3 botOffset = new(0, -0.2f, 0);
        _ = Instantiate(projectile, fixedAttackPos.transform.position + botOffset, fixedAttackPos.transform.rotation);
        audioSource.pitch = 1.1f;  // Increased pitch
        audioSource.PlayOneShot(ThrowingStarSoundEffect, 2f);
        yield return new WaitForSeconds(0.05f);

        // Reset pitch to default for other sounds
        audioSource.pitch = 1.0f;

        SwipeOne.SetBool("SwipeOne", false);
        SwipeTwo.SetBool("SwipeTwo", false);

        // Optionally, destroy the fixed position GameObject if it's no longer needed
        Destroy(fixedAttackPos, 0.5f); // Adjust the delay as needed
    }
    public void GetSecondSkillInput()
    {
        if (Time.time > nextFireTimeSkill2 && Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("isAttacking");
            _ = StartCoroutine(secondSkill());
            nextFireTimeSkill2 = Time.time + cooldownTimeSkill2;
            //   textCooldownS2.gameObject.SetActive(true);
            cooldownTimerS2 = cooldownTimeSkill2;
        }
    }

    private IEnumerator secondSkill()
    {
        yield return new WaitForSeconds(0.20f);
        _ = Instantiate(projectile2, attackPos.position, attackPos.rotation);
        audioSource.PlayOneShot(BigShurikenSoundEffect, 0.7f);
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
        for (int i = 0; i < 3; i++)
        {
            _ = Instantiate(ProjectileUltimate, RogueUltPos.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos1.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos2.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos3.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos4.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos5.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos6.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos7.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos8.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos9.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos10.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos11.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos12.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos13.position, RogueUltPos.rotation);
            _ = Instantiate(ProjectileUltimate, RogueUltPos14.position, RogueUltPos.rotation);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
