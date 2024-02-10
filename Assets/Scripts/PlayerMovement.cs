using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    public int level;
    [HideInInspector]
    public int healthPotions;
    [HideInInspector]
    public int maxHealthPotions;
    [HideInInspector]
    public int healthPotionValue;
    [HideInInspector]
    public int expValue;
    [HideInInspector]
    public int currentExp;
    [HideInInspector]
    public int maxExp;
    [HideInInspector]
    public float moveDirection;
    [HideInInspector]
    public int skillOneLevel = 1;
    [HideInInspector]
    public int skillTwoLevel = 1;
    [HideInInspector]
    public int skillThreeLevel = 1;
    [HideInInspector]
    public int ultSkillLevel = 1;
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;
    public float flyForce = 5f;

    [Header("Player State")]
    [HideInInspector]
    public bool playerIsNearPortal = false;
    [HideInInspector]
    public bool isAirborne = false;
    [HideInInspector]
    public bool isPressingUp = false;
    [HideInInspector]
    public bool isPressingInteract = false;
    [HideInInspector]
    public bool isPressingDrop = false;
    [HideInInspector]
    public bool isHoldingObject = false;
    [HideInInspector]
    public bool isWalking = false;
    [HideInInspector]
    public bool playerHasDied = false;
    [HideInInspector]
    public int DamageRecieved = 0;
    [HideInInspector]
    public bool CanMove = false;
    [HideInInspector]
    public bool startup = true;

    [Header("References")]
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public TMP_Text playerLevel;
    public TMP_Text levelUI;
    public TMP_Text skillLevel1Text;
    public TMP_Text skillLevel2Text;
    public TMP_Text skillLevel3Text;
    public TMP_Text skillUltText;
    public TMP_Text HealthDisplayText;
    public GameObject upgradeButtons;
    public Animator leveledUpAnimator;
    public HealthBar healthBar;
    public ExperienceBar experienceBar;
    public GameObject shopKeeperCanvas;
    public AudioClip fall;
    public GameObject optionsMenuCanvas;
    public TMP_Text coinCount;

    [Header("Player State")]
    [HideInInspector]
    public bool facingRight = true;
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool shouldLevelUp = false;

    [Header("Portal Settings")]
    public int expToBeGained;
    public string destination = "";
    private Vector3 portalDestinationPosition;
    private string portalToTeleportTo;

    [Header("Health Settings")]
    public int currentHealth;
    public int maxHealth;

    /*    [Header("Shopkeeper and Options Menu Settings")]
        private bool isNearShopKeeper = false;
        private bool isNearOptionsMenu = true;
    */
    [Header("Coins")]
    public int coins;

    public AudioSource audioSource;
    public Animator LandingDust;
    private bool isPlaying;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        playerLevel = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<TMP_Text>();
        experienceBar.setMaxExp(maxExp);
        GainExperience(0);
        isPlaying = false;

    }

    public void GainExperience(int gainedExp)
    {
        currentExp += gainedExp;

        if (currentExp >= maxExp)
        {
            shouldLevelUp = true;
            currentExp -= maxExp;
        }

    }

    public void UpdateHealth(int mod)
    {
        currentHealth += mod;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
        }
        else if (currentHealth <= 0)
        {
            playerHasDied = true;
            PlayerDeath();
        }
    }

    void Update()
    {
        if (healthPotions > maxHealthPotions)
        {
            healthPotions = maxHealthPotions;
        }


        getPlayerInput();
        playerInteractInput();
        animate();


    }

    private void FixedUpdate()
    {
        OnTriggerEnter2D(bc);
        moveCharacter();
    }

    private void PlayerDeath()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().gravityScale = 0.85f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = new Vector3(-2, -1, 0);
    }

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    public void getPlayerInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
       var jumpDirection =rb.velocity.y;
        GetComponent<Animator>().SetFloat("VerticalSpeed",rb.velocity.y);
        Debug.Log(rb.velocity.y);
        if (!isAirborne)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                isAirborne = true;
                isGrounded = true;
            }
        }
        if (isAirborne && Input.GetButton("Jump"))
        {
            rb.velocity = new Vector3(moveDirection * moveSpeed, rb.velocity.y + flyForce * Time.deltaTime);
        }
    }

    public void playerInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            EnterPortal();
            //  OpenShopKeeperUI();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (healthPotions > 0)
            {
                UpdateHealth(+healthPotionValue);
                healthPotions--;
            }
            isPressingInteract = true;
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            isPressingInteract = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            isPressingDrop = true;
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isPressingDrop = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //   OpenOptionMenuUI();
            isPressingDrop = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isPressingDrop = false;
        }
    }

    public void LevelUp()
    {
        if (level < 60 && shouldLevelUp)
        {
            IncreaseLevel();
            Debug.Log("Level Up! Player Level is now: " + level);
            shouldLevelUp = false;
            AudioController.instance.PlayLevelUpSound();
        }
    }

    public void IncreaseLevel()
    {
        level++;
        StartCoroutine(LevelUpDelay());
        Debug.Log("Level Up! Player Level is now: " + level);
        maxExp = level * 23;
        currentHealth = maxHealth + 100;
    }

    IEnumerator LevelUpDelay()
    {
        playerLevel.text = "Level Up!";
        yield return new WaitForSeconds(2);
        playerLevel.text = "";
        leveledUpAnimator.SetBool("LeveledUp", false);
    }

    public void animate()
    {
        if (moveDirection > 0 && !facingRight)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingRight)
        {
            FlipCharacter();
        }
    }

    public void moveCharacter()
    {
        if (!isAirborne)
        {
            rb.velocity = new Vector3(moveDirection * moveSpeed, rb.velocity.y);
            GetComponent<Animator>().SetBool("isAirborne", false);
        }
        jumpCharacter();
    }
    public void jumpCharacter()
    {
        if (isJumping)
        {
            rb.velocity = new Vector3(moveDirection * moveSpeed, jumpForce);
            AudioController.instance.PlayJumpSound();
            isJumping = false;
            GetComponent<Animator>().SetBool("isAirborne",true);
        }
    }
    public void jumpHoldCharacter()
    {
        if (isJumping && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector3(moveDirection * moveSpeed, jumpForce);

        }
    }

    public void EnterPortal()
    {
        if (playerIsNearPortal)
        {
            if (!facingRight)
            {
                FlipCharacter();
            }
            Input.ResetInputAxes();
            SceneManager.LoadScene(destination);
            Debug.Log("Should Enter Portal");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerHasDied)
        {
            PositionPlayer("HW-B");
            currentHealth = maxHealth / 4;
            healthBar.SetMaxHealth(maxHealth);
            playerHasDied = false;
        }
        else
        {
            PositionPlayer(portalToTeleportTo);
        }
    }

    private void PositionPlayer(string portalName)
    {
        //GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position = GameObject.FindGameObjectWithTag(portalName).GetComponent<Transform>().position;
    }

    /*    public void OpenShopKeeperUI()
        {
            if (!shopKeeperCanvas.activeSelf && isNearShopKeeper)
            {
                shopKeeperCanvas.SetActive(true);
            }
            else
            {
                shopKeeperCanvas.SetActive(false);
            }
        }*/

    /*    public void OpenOptionMenuUI()
        {
            if (!optionsMenuCanvas.activeSelf && isNearOptionsMenu)
            {
                optionsMenuCanvas.SetActive(true);
            }
            else
            {
                optionsMenuCanvas.SetActive(false);
            }
        }*/
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "deathBox")
        {
            playerHasDied = true;
            PlayerDeath();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("World"))
        {
            GetComponent<Animator>().SetTrigger("isLanded");
            isAirborne = false;
            isGrounded = true;
            if (!isPlaying)
            {
                StartCoroutine(PlayAndResetAnimation());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "World" || collision.gameObject.tag == "Platform")
        {
            isGrounded = false;
        }

    }

    private IEnumerator PlayAndResetAnimation()
    {
        isPlaying = true;

        // Play the animation
        LandingDust.Play("s500");
        audioSource.PlayOneShot(fall, 1f);

        // Wait for the next frame to ensure the animation starts playing
        yield return null;

        // Wait until the animation is finished
        while (LandingDust.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 || LandingDust.IsInTransition(0))
        {
            yield return null;
        }

        // Trigger the transition to the idle state
        LandingDust.SetTrigger("JustLanded");

        isPlaying = false;
    }
}