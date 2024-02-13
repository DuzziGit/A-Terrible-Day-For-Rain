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
    public float moveDirection;
    private int jumpDirection = 0;


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
    private readonly string portalToTeleportTo;

    [Header("Health Settings")]
    public int currentHealth;
    public int maxHealth;
    private const int jumpSpeed = 10;

    /*    [Header("Shopkeeper and Options Menu Settings")]
        private bool isNearShopKeeper = false;
        private bool isNearOptionsMenu = true;
    */
    [Header("Coins")]
    public int coins;

    public AudioSource audioSource;
    private bool isPlaying;
    [SerializeField] protected Animator animator;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

    }

    private void Start()
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

    private void Update()
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
        moveCharacter();


    }
    private void PlayerDeath()
    {
        GetComponent<Rigidbody2D>().gravityScale = 0.85f;
        GetComponent<Transform>().position = new Vector3(-2, -1, 0);
    }

    private void FlipCharacter()
    {
        // Flip the character's facing direction without changing the momentum
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    public void getPlayerInput()
    {
        moveDirection = Input.GetAxis("Horizontal");
        animator.SetFloat("VerticalSpeed", rb.velocity.y);

        if (!isAirborne)
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJumping = true;
                isAirborne = true;
            }
        }
        // You can still flip the character without affecting the momentum.
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            FlipCharacter();
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
        _ = StartCoroutine(LevelUpDelay());
        Debug.Log("Level Up! Player Level is now: " + level);
        maxExp = level * 23;
        currentHealth = maxHealth + 100;
    }

    private IEnumerator LevelUpDelay()
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
            // Apply movement normally on the ground.
            rb.velocity = new Vector3(moveDirection * moveSpeed, rb.velocity.y);
            animator.SetBool("isAirborne", false);
            animator.SetFloat("Speed", Mathf.Abs(moveDirection));
            if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow))
            {
                jumpDirection = 1;
            }
            if (Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftArrow))
            {
                jumpDirection = -1;
            }
            if (isJumping)
            {

                // Store the direction at the start of the jump.
                Jump();
            }
        }
        else
        {
            // While airborne, maintain horizontal momentum but allow for facing direction changes.
            if (isJumping)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {

        // Check if there's horizontal input to determine the jump direction
        float horizontalVelocity = jumpDirection * jumpSpeed;

        // Apply the jump force along with the horizontal velocity to maintain forward momentum.
        rb.velocity = new Vector3(horizontalVelocity, jumpForce);

        AudioController.instance.PlayJumpSound();
        isJumping = false;
        animator.SetBool("isAirborne", true);
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (playerHasDied)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
            currentHealth = maxHealth / 4;
            healthBar.SetMaxHealth(maxHealth);
            playerHasDied = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "deathBox")
        {
            playerHasDied = true;
            PlayerDeath();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("World"))
        {
            isAirborne = false;
            isGrounded = true;
            animator.SetTrigger("isLanded");

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag is "World" or "Platform")
        {
            isGrounded = false;
        }

    }
}
