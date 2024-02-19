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
    protected bool facingRight = true;
    protected bool isJumping = false;
    protected bool isGrounded = false;
    protected bool shouldLevelUp = false;

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
    private bool isPlaying;
    [SerializeField] protected Animator animator;

    public Collider2D playerCollider;
    private float disableCollisionTime = 0.1f; // Time to disable collision
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
        if (Input.GetKey(KeyCode.K) && Input.GetButtonDown("Jump"))
        {
            StartCoroutine(FallThrough());
            Debug.Log("Fell Through Platform");
        }
        if (!isAirborne && !Input.GetKey(KeyCode.K))
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
    private IEnumerator FallThrough()
    {
        if (isFallingThrough) yield break;
        isFallingThrough = true;

        int originalLayer = gameObject.layer;

        // Change the player's layer to NoPlatformCollision to start ignoring collisions
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
            AudioController.instance.PlayLevelUpSound();
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
        if (collision.gameObject.tag is "World" or "Platform")
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
