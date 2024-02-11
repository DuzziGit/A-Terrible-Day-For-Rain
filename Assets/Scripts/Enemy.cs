using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    float moveSpeed;
    public BoxCollider2D bc;
    public Rigidbody2D rb;

    public Color bigEnemy = Color.red;
    public Color medEnemy = Color.yellow;
    public Color smallEnemy = Color.green;
    public Color tutEnemy = new Color(0, 1f, 1f, 1f);

    public Animator animator;

    void Start()
    {
      
    }
    private void Awake()
    {
          rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isTouchingPlayer = false;
        }
    }

    void Update()
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
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, LayerMask.GetMask("World"));
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, distance, LayerMask.GetMask("World"));
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
            if(GetComponent<EnemyCon>().isDisplayingDamage){
                rb.freezeRotation = true;
                GetComponent<SpriteRenderer>().enabled = false;
                bc.enabled = false;
            }
            Die();
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
        EnemySpawner.currentEnemies--;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.GainExperience(expValue);
            }
        }


        Destroy(gameObject);
    }

    private void Patrol()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance, LayerMask.GetMask("World"));
        RaycastHit2D wallInfo = Physics2D.Raycast(wallDetection.position, Vector2.right, distance, LayerMask.GetMask("World"));
        RaycastHit2D enemyWall = Physics2D.Raycast(wallDetection.position, Vector2.right, distance, LayerMask.GetMask("EnemyOnlyWall"));

        if (groundInfo.collider == false || wallInfo.collider == true || enemyWall.collider == true)
        {
            if (movingRight == true)
            {
                rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
                enemySprite.flipX = true;
                movingRight = false;
            }
            else
            {
                rb.velocity = new Vector3(speed, rb.velocity.y, 0);
                enemySprite.flipX = false;
                movingRight = true;
            }
        }
    }

    void chasePlayer()
    {
        HoverPlayerX();
    }
}
