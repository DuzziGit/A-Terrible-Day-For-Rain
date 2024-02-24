using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Stats")]
    [HideInInspector]
    public int level;
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
    [HideInInspector]
    public float moveDirection;
    private int jumpDirection = 0;


    [Header("Player State")]
    [HideInInspector]
    public bool playerIsNearPortal = false;
    [HideInInspector]
    public bool isAirborne = false;
    [HideInInspector]
    public bool isPressingInteract = false;
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
    protected Rigidbody2D rb;
    [SerializeField] protected CapsuleCollider2D cc;
    [SerializeField] protected TMP_Text playerLevelTextText;
    [SerializeField] protected TMP_Text levelUI;
    public Animator leveledUpAnimator;
    public HealthBar healthBar;
    public ExperienceBar experienceBar;
    public GameObject optionsMenuCanvas;

    [Header("Player State")]
    [HideInInspector]
    public bool facingRight = true;
    protected bool isJumping = false;
    protected bool isGrounded = false;
    protected bool shouldLevelUp = false;
    protected bool shouldJump = false; // Flag to indicate jump input


    [Header("Portal Settings")]
    [HideInInspector]
    public int expToBeGained;
    [HideInInspector]
    public string destination = "";
    private Vector3 portalDestinationPosition;
    private readonly string portalToTeleportTo;

    [Header("Health Settings")]
    public int currentHealth;
    public int maxHealth;
    private const int jumpSpeed = 10;

    public AudioSource audioSource;
    [SerializeField] protected Animator animator;

    public Collider2D playerCollider;
    private bool isFallingThrough = false;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //    playerLevelTextText = GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<TMP_Text>();
        experienceBar.setMaxExp(maxExp);
        GainExperience(0);

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
        if (GameController.instance.playerCanMove)
        {
            getPlayerInput();
            playerInteractInput();
            animate();
        }
    }
    private void FixedUpdate()
    {
        if (GameController.instance.playerCanMove)
        {
            moveCharacter();
            if (shouldJump)
            {
                Jump();
            }
        }

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
        // Existing code to handle flipping character...
        moveDirection = Input.GetAxis("Horizontal");
        animator.SetFloat("VerticalSpeed", rb.velocity.y);

        // Force the player to stop when 'K' is pressed and only if the player is grounded
        if (Input.GetKey(KeyCode.K) && isGrounded && !isFallingThrough && rb.velocity.y == 0)
        {
            // Stop the player by setting velocity to zero
            rb.velocity = Vector2.zero;
            moveDirection = 0; // Ensure moveDirection is set to 0 to stop movement logic
        }
        else
        {
            // Resume movement by allowing moveDirection to dictate player velocity in FixedUpdate
            if (!isAirborne)
            {
                if (Input.GetButtonDown("Jump") && !isFallingThrough)
                {
                    shouldJump = true; // Set flag to true to handle in FixedUpdate
                }
            }
        }

        // Check for jump through platform
        if (Input.GetKey(KeyCode.K) && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(FallThrough());
        }
        // You can still flip the character without affecting the momentum.
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            FlipCharacter();
        }


    }
    private IEnumerator FallThrough()
    {
        if (isFallingThrough) yield break;
        isFallingThrough = true;

        // Lock the player's X velocity to 0
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(0, currentVelocity.y);

        // Change the player's layer to NoPlatformCollision to start ignoring collisions
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerFallThrough");

        // Wait a bit to ensure the player starts falling through
        yield return new WaitForSeconds(0.27f);

        // Change back to the original layer
        gameObject.layer = originalLayer;

        isFallingThrough = false;
    }
    public void playerInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            EnterPortal();
            //  OpenShopKeeperUI();
        }
        isPressingInteract = true;
    }


    public virtual void LevelUp()
    {
        if (level < 60 && shouldLevelUp)
        {
            IncreaseLevel();
            Debug.Log("Level Up! Player Level is now: " + level);
            shouldLevelUp = false;
            leveledUpAnimator.SetTrigger("LeveledUp");

        }
    }

    public void IncreaseLevel()
    {
        level++;
        //   _ = StartCoroutine(LevelUpDelay());
        Debug.Log("Level Up! Player Level is now: " + level);
        maxExp = level * 23;
        currentHealth = maxHealth + 100;
    }

    private IEnumerator LevelUpDelay()
    {
        // playerLevelText.text = "Level Up!";
        yield return new WaitForSeconds(2);
        //  playerLevelText.text = "";
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
            // Only apply horizontal movement if not falling through a platform
            if (!isFallingThrough)
            {
                rb.velocity = new Vector3(moveDirection * moveSpeed, rb.velocity.y);
            }

            animator.SetBool("isAirborne", false);
            animator.SetFloat("Speed", Mathf.Abs(moveDirection));

            if ((Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow)) && !isFallingThrough)
            {
                jumpDirection = 1;
            }
            else if ((Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.LeftArrow)) && !isFallingThrough)
            {
                jumpDirection = -1;
            }

            if (isJumping)
            {
                Jump();
            }
        }
        else
        {
            // While airborne, maintain horizontal momentum but allow for facing direction changes if not falling through
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
        shouldJump = false;
        isAirborne = true;
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
        if (collision.gameObject.tag is "World" or "Platform")
        {
            isAirborne = false;
            isGrounded = true;
            animator.SetTrigger("isLanded");
            if (isFallingThrough) isFallingThrough = false;


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
