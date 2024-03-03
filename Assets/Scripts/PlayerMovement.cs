using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
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
    public bool isGrounded = false;
    protected bool shouldLevelUp = false;
    protected bool shouldJump = false; // Flag to indicate jump input

    public float gizmoRayLength = 0.1f;

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
    private const int jumpSpeed = 5;

    public AudioSource audioSource;
    [SerializeField] protected Animator animator;
    private bool isFallingThrough = false;
    protected bool isExecutingSkill = false;
    [SerializeField] private Transform feetPos;
    public LayerMask platformLayerMask;
    private bool isTouchingPlatform = false;

    [SerializeField] protected InputActionReference MoveInput, JumpInput, BasicSkillInput, AoeSkillInput, SummonSkillInput, MovementSkillInput;
    protected float VertDirection = 0;

    private bool isSitting;

    protected virtual void OnEnable()
    {
        JumpInput.action.performed += OnJumpPerformed;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected virtual void OnDisable()
    {
        JumpInput.action.performed -= OnJumpPerformed;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!isAirborne && !isFallingThrough && !isExecutingSkill && !isSitting)
        {
            Jump();
        }
    }
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
        isGrounded = IsGrounded();
        if (GameController.instance.playerCanMove)
        {
            getPlayerInput();
            playerInteractInput();
            animate();
        }


    }
    private void FixedUpdate()
    {
        if (GameController.instance.playerCanMove && !isExecutingSkill)
        {

            moveCharacter();
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

    protected void getPlayerInput()
    {
        if (!GameController.instance.playerCanMove && !isExecutingSkill)
        {
            moveDirection = 0; // Reset movement direction to ensure no movement occurs
            rb.velocity = Vector2.zero;
            return; // Skip processing input if movement is disabled
        }
        moveDirection = MoveInput.action.ReadValue<Vector2>().x;
        animator.SetFloat("VerticalSpeed", rb.velocity.y);
        VertDirection = MoveInput.action.ReadValue<Vector2>().y;
        // Force the player to stop when 'K' is pressed and only if the player is grounded
        if (VertDirection < 0 && isGrounded && !isFallingThrough && rb.velocity.y == 0)
        {
            animator.SetBool("isHoldingDown", true);
            // Stop the player by setting velocity to zero
            rb.velocity = Vector2.zero;
            moveDirection = 0; // Ensure moveDirection is set to 0 to stop movement logic
            isSitting = true;
        }
        else
        {
            animator.SetBool("isHoldingDown", false);
            isSitting = false;
        }

        // Check for jump through platform
        if (VertDirection < 0 && Input.GetButtonDown("Jump") && isTouchingPlatform)
        {
            StartCoroutine(FallThrough());
        }
        // You can still flip the character without affecting the momentum.
        if ((moveDirection > 0 && !facingRight) || (moveDirection < 0 && facingRight))
        {
            FlipCharacter();
        }
    }

    bool IsGrounded()
    {
        if (isFallingThrough)
        {
            return false;
        }

        // Cast a ray straight down from the feet position
        RaycastHit2D hit = Physics2D.Raycast(feetPos.position, Vector2.down, gizmoRayLength, platformLayerMask);

        // Visualize the raycast in the editor
        Debug.DrawRay(feetPos.position, Vector2.down * gizmoRayLength, Color.green);

        // If it hits something, you're grounded
        if (hit.collider != null)
        {
            return true;
        }

        return false;
    }
    private IEnumerator FallThrough()
    {
        if (isFallingThrough) yield break;
        isFallingThrough = true;
        animator.SetTrigger("isFallingDown");
        // Lock the player's X velocity to 0
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(0, currentVelocity.y);

        // Change the player's layer to NoPlatformCollision to start ignoring collisions
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("PlayerFallThrough");

        // Wait a bit to ensure the player starts falling through
        yield return new WaitForSeconds(0.4f);

        // Change back to the original layer
        gameObject.layer = originalLayer;

    }
    public void playerInteractInput()
    {
        if (VertDirection > 0)
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

            if (facingRight && !isFallingThrough)
            {
                jumpDirection = 1;
            }
            else if (!facingRight && !isFallingThrough)
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
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isTouchingPlatform = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            isTouchingPlatform = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag is "World" or "Platform")
        {
            isGrounded = false;
        }
    }

    // void OnDrawGizmos()
    // {
    //     float characterWidth = cc.bounds.size.x;
    //     float characterHeight = cc.bounds.size.y;
    //     Vector2 rayStartPoint = new Vector2(transform.position.x - characterWidth / 2, transform.position.y - characterHeight / 2);
    //     float distanceBetweenRays = characterWidth / (gizmoNumberOfRays - 1);

    //     Gizmos.color = gizmoRayColor;

    //     for (int i = 0; i < gizmoNumberOfRays; i++)
    //     {
    //         Vector2 rayStart = rayStartPoint + Vector2.right * distanceBetweenRays * i;
    //         Gizmos.DrawLine(rayStart, rayStart + Vector2.down * gizmoRayLength);
    //     }
    // }

}
