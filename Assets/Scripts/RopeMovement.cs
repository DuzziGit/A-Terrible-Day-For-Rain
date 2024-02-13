using UnityEngine;

public class RopeMovement : MonoBehaviour
{
    [Header("Attributes")]
    public float climbSpeed = 4f;
    public float exitBoostForce = 80f;

    private Rigidbody2D playerRb;
    private bool isOnLadder;
    private float climbInput;
    private bool jumpRequested;
    private float originalGravityScale;
    private GameObject currentRope;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Start()
    {
        originalGravityScale = playerRb.gravityScale;
    }

    private void Update()
    {
        climbInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(KeyCode.I) || Input.GetKey(KeyCode.UpArrow))
        {
            climbInput = isOnLadder ? 1f : 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isOnLadder)
        {
            if (jumpRequested)
            {
                // Jump logic here
            }
            else if (climbInput != 0f)
            {
                playerRb.gravityScale = 0f;
                playerRb.velocity = new Vector2(0f, climbInput * climbSpeed);
                playerRb.position = new Vector2(currentRope.transform.position.x, playerRb.position.y);
            }
            else
            {
                playerRb.velocity = Vector2.zero;
            }
        }
        else
        {
            playerRb.gravityScale = originalGravityScale;
            if (jumpRequested)
            {
                jumpRequested = false;
            }
        }
    }
    private void Jump(float launchDirection)
    {
        playerRb.velocity = new Vector2(playerRb.velocity.x, exitBoostForce);
        playerRb.AddForce(new Vector2(launchDirection * exitBoostForce, 0f), ForceMode2D.Impulse);
        playerRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        isOnLadder = false;
        currentRope = null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            if (Input.GetKey(KeyCode.I) || Input.GetKey(KeyCode.UpArrow))
            {
                isOnLadder = true;
                currentRope = collision.gameObject;
                playerRb.position = new Vector2(currentRope.transform.position.x, playerRb.position.y);
                if (jumpRequested)
                {
                    jumpRequested = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            isOnLadder = false;
            playerRb.gravityScale = originalGravityScale;
            currentRope = null;
        }
    }
}
