using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public int health;
    public int damage;
    public int level;
    public float speed;
    public float distance;
    public Transform groundDetection;
    public Transform wallDetection;

    public int expValue;
    public ExperienceController expObject;

    public static bool isAggroed = false;
    public static bool isPatroling = true;
    public static bool isTouchingPlayer = false;
    private bool movingRight = true;

    public float agroRange;

    private float timeBetweenDmg;
    public float startTimeBetweenDmg;
    public SpriteRenderer enemySprite;
    private readonly float moveSpeed;
    public CapsuleCollider2D bc;
    public Rigidbody2D rb;

    public Color bigEnemy = Color.red;
    public Color medEnemy = Color.yellow;
    public Color smallEnemy = Color.green;
    public Color tutEnemy = new(0, 1f, 1f, 1f);

    public Animator animator;
    public EnemySpawner MySpawner;

    private void Start()
    {

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isTouchingPlayer)
        {
            if (timeBetweenDmg <= 0)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().UpdateHealth(-damage);
                timeBetweenDmg = startTimeBetweenDmg;
            }
            else
            {
                timeBetweenDmg -= Time.deltaTime;
            }
        }

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().currentHealth <= 0)
        {
            isTouchingPlayer = false;
        }
    }

    public void HoverPlayerX()
    {
        _ = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, LayerMask.GetMask("World"));
        _ = Physics2D.Raycast(wallDetection.position, Vector2.right, distance, LayerMask.GetMask("World"));
        RaycastHit2D enemyWall = Physics2D.Raycast(wallDetection.position, Vector2.right, distance, LayerMask.GetMask("EnemyOnlyWall"));



        if (transform.position.x - GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x < 0.2f)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        else if (transform.position.x > GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x && enemyWall.collider == false)
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            enemySprite.flipX = true;
            movingRight = false;
        }
        else if (transform.position.x < GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.x && enemyWall.collider == false)
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
            enemySprite.flipX = false;
            movingRight = true;
        }

    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            if (GetComponent<EnemyCon>().isDisplayingDamage)
            {
                rb.freezeRotation = true;
                GetComponent<SpriteRenderer>().enabled = false;
                Animator[] animators = GetComponentsInChildren<Animator>();
                // Loop through each Animator and stop its animations, then disable it
                foreach (Animator animator in animators)
                {
                    // Stop the current animation by setting its speed to 0
                    animator.Rebind();

                    // Now, disable the Animator component
                    animator.enabled = false;
                }
                bc.enabled = false;
            }
            else if (GetComponent<EnemyCon>().isDisplayingDamage == false)
            {

                Die();
            }
        }

        float distanceToPlayer = Vector2.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position);

        if (distanceToPlayer < agroRange)
        {
            isAggroed = true;
            isPatroling = false;
            if (isAggroed)
            {
                chasePlayer();
            }
        }
        else
        {
            if (distanceToPlayer > agroRange)
            {
                isAggroed = false;
                isPatroling = true;
                if (isAggroed)
                {
                    chasePlayer();
                }
            }
            Patrol();
            isAggroed = false;
            isPatroling = true;
        }
    }
    private void Die()
    {
        MySpawner.OnEnemyDestroyed();
        if (GameController.instance.playerMovement != null)
        {
            GameController.instance.playerMovement.GainExperience(expValue);
        }
        Destroy(gameObject);
    }
    private void Patrol()
    {
        // Check the direction to cast the ray for walls
        Vector2 direction = movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, LayerMask.GetMask("World", "Platform"));
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, direction, distance, LayerMask.GetMask("World", "Platform"));
        RaycastHit2D enemyWall = Physics2D.Raycast(wallDetection.position, direction, distance, LayerMask.GetMask("EnemyOnlyWall"));

        if (!groundInfo.collider || wallInfo.collider || enemyWall.collider)
        {
            FlipDirection();
        }
    }

    private void FlipDirection()
    {
        movingRight = !movingRight;
        rb.velocity = new Vector3(movingRight ? speed : -speed, rb.velocity.y, 0);
        enemySprite.flipX = !movingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision) // Corrected argument type
    {
        if (collision.collider.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    private void chasePlayer()
    {
        HoverPlayerX();
    }
}
