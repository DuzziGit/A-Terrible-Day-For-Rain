using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RogueSkillController : PlayerMovement
{
    public float MovementSkillForce;
    public float MovementSkillForceLeft;

    public GameObject projectile;
    public GameObject projectile2;
    public GameObject ProjectileUltimate;
    public Transform attackPos;


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

     private Image imageCooldownS1;
     private TMP_Text textCooldownS1;
    private Image imageCooldownS2;
     private TMP_Text textCooldownS2;
    private Image imageCooldownS3;
     private TMP_Text textCooldownS3;
     private Image imageCooldownSM;
    private TMP_Text textCooldownSM;
     private Image imageCooldownSU;
     private TMP_Text textCooldownSU;
     private TMP_Text healthPotionText;

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

    public Animator animator;
    Renderer rend;
    Color c;

    private float cooldownTimerS1 = 0.0f;
    private float cooldownTimerS2 = 0.0f;
    private float cooldownTimerS3 = 0.0f;
    private float cooldownTimerSM = 0.0f;
    private float cooldownTimerSU = 0.0f;
    private float cooldownTimer = 0.0f;

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
    }

    private void Update()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale = 2f;

        experienceBar.setMaxExp(maxExp);

        levelUI.text = level.ToString();
        maxHealth = level * 100;
        healthPotionValue = level * 10;
        maxExp = level * 60;

        experienceBar.SetExperience(currentExp);
        cooldownTimeSkill3Upgraded = cooldownTimeSkill3 - GetComponent<PlayerMovement>().skillThreeLevel;
        // skillLevel1Text.text = skillOneLevel.ToString();
        //  skillLevel2Text.text = skillTwoLevel.ToString();
        //  skillLevel3Text.text = skillThreeLevel.ToString();
        //  skillUltText.text = ultSkillLevel.ToString();
        //  HealthDisplayText.text = $"{currentHealth} / {maxHealth}";
        //  coinCount.text = coins.ToString();

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
    }

    private void FixedUpdate()
    {
        OnTriggerEnter2D(bc);
        //Move Player
        moveCharacter();
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

    public void moveCharacter()
    {
        if (!isAirborne)
        {
            rb.velocity = new Vector3(moveDirection * moveSpeed, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            GetComponent<Animator>().SetBool("isAirborne",false);
        }
        jumpCharacter();
        GetComponent<Animator>().SetBool("isAirborne",true);
    }

    public void MovementSkill()
    {
        MovementSkillOne.SetBool("MovementSkillUsed", true);
        MovementSkillTwo.SetBool("MovementSkillUsed", true);

        float force = facingRight ? MovementSkillForce : MovementSkillForceLeft;
        rb.velocity = new Vector3(moveDirection * moveSpeed + force, jumpForce);

        StartCoroutine(ResetMovementSkillAnimation());
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
            StartCoroutine(FirstSkill());
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
    GameObject fixedAttackPos = new GameObject("FixedAttackPosition");
    fixedAttackPos.transform.position = attackPos.position;
    fixedAttackPos.transform.rotation = attackPos.rotation;

    // First shuriken
    Vector3 topOffset = new Vector3(0, 0.2f, 0);
    Instantiate(projectile, fixedAttackPos.transform.position + topOffset, fixedAttackPos.transform.rotation);
    audioSource.pitch = 1.6f;  // Reduced pitch
    audioSource.PlayOneShot(ThrowingStarSoundEffect, 2f);
    yield return new WaitForSeconds(0.05f);

    // Second shuriken
    Instantiate(projectile, fixedAttackPos.transform.position, fixedAttackPos.transform.rotation);
    audioSource.pitch = 1.0f;  // Normal pitch
    audioSource.PlayOneShot(ThrowingStarSoundEffect, 2f);
    yield return new WaitForSeconds(0.05f);

    // Third shuriken
    Vector3 botOffset = new Vector3(0, -0.2f, 0);
    Instantiate(projectile, fixedAttackPos.transform.position + botOffset, fixedAttackPos.transform.rotation);
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
            secondSkill();
            nextFireTimeSkill2 = Time.time + cooldownTimeSkill2;
            //   textCooldownS2.gameObject.SetActive(true);
            cooldownTimerS2 = cooldownTimeSkill2;
        }
    }

    public void secondSkill()
    {
        Instantiate(projectile2, attackPos.position, attackPos.rotation);
        audioSource.PlayOneShot(BigShurikenSoundEffect, 0.7f);
    }

    // Third Skill
    public void GetThirdSkillInput()
    {
        if (Time.time > nextFireTimeSkill3 && Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(ThirdSkillEnum());
            nextFireTimeSkill3 = Time.time + cooldownTimeSkill3Upgraded;
            //     textCooldownS3.gameObject.SetActive(true);
            cooldownTimerS3 = cooldownTimeSkill3Upgraded;
        }
    }

    private IEnumerator ThirdSkillEnum()
    {
        Physics2D.IgnoreLayerCollision(7, 11, true);
        yield return new WaitForSeconds(2.5f);
        Physics2D.IgnoreLayerCollision(7, 11, false);
    }

    // Ultimate Skill
    public void GetUltimateSkillInput()
    {
        if (Time.time > nextFireTimeSkillUlt && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(UltimateSkillEnum());
            nextFireTimeSkillUlt = Time.time + cooldownTimeSkillUlt;
            //   textCooldownSU.gameObject.SetActive(true);
            cooldownTimerSU = cooldownTimeSkillUlt;
        }
    }

    private IEnumerator UltimateSkillEnum()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(ProjectileUltimate, RogueUltPos.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos1.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos2.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos3.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos4.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos5.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos6.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos7.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos8.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos9.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos10.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos11.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos12.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos13.position, RogueUltPos.rotation);
            Instantiate(ProjectileUltimate, RogueUltPos14.position, RogueUltPos.rotation);
            yield return new WaitForSeconds(0.15f);
        }
    }
}
