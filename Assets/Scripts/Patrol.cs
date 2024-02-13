using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float distance;

    private bool movingRight = true;
    public Transform groundDetection;
    public Transform wallDetection;

    private SpriteRenderer spriteRenderer;  // Reference to the sprite renderer component
    private Rigidbody2D rb;

    private void Awake()
    {
        // Get the required component references
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Set the initial facing direction of the enemy sprite based on the movingRight variable
        spriteRenderer.flipX = !movingRight;
    }

    private void Update()
    {
        // Move the enemy horizontally at the specified speed
        rb.velocity = new Vector2(speed * (movingRight ? 1 : -1), rb.velocity.y);

        // Check if there's ground or a wall ahead of the enemy using raycasts
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, LayerMask.GetMask("World"));
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, movingRight ? Vector2.right : Vector2.left, distance, LayerMask.GetMask("World"));

        // Visualize the raycasts in the Scene view
        Debug.DrawRay(groundDetection.position, Vector2.down * distance, Color.green);
        Debug.DrawRay(wallDetection.position, (movingRight ? Vector2.right : Vector2.left) * distance, Color.blue);

        // If the enemy is about to fall off a ledge or hit a wall, flip its facing direction
        if (groundInfo.collider == false || wallInfo.collider == true)
        {
            movingRight = !movingRight;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }
}