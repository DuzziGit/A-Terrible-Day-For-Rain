using UnityEngine;
using System.Collections;
using Unity.Mathematics;
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

    public bool isAggroed = false;
    public bool isPatroling = true;
    public bool isTouchingPlayer = false;
    protected bool movingRight = true;

    public float agroRange;

    private float timeBetweenDmg;
    public float startTimeBetweenDmg;
    public SpriteRenderer enemySprite;
    private readonly float moveSpeed;
    public CapsuleCollider2D bc;
    public Rigidbody2D rb;
    public Animator animator;
    public EnemySpawner MySpawner;
    protected bool canFlip = true;
    protected float flipCooldown = 0.2f;
    protected bool isDying = false;

    private void Start()
    {

    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<CapsuleCollider2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        gameObject.layer = LayerMask.NameToLayer("Enemy");
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
        if (health <= 0 && !isDying)
        {
            Die();

        }
    }

    public void HoverPlayerX()
    {
        //    _ = Physics2D.Raycast(transform.position, Vector2.down, distance, LayerMask.GetMask("World"));
        _ = Physics2D.Raycast(transform.position, Vector2.right, distance, LayerMask.GetMask("World"));
        Vector2 castDirection = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D enemyWall = Physics2D.Raycast(wallDetection.position, castDirection, distance, LayerMask.GetMask("EnemyOnlyWall"));


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
        if (health > 0)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, GameController.instance.Player.transform.position);

            if (distanceToPlayer < agroRange)
            {
                isAggroed = true;
                isPatroling = false;
                if (isAggroed)
                {
                    HoverPlayerX();
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
                        HoverPlayerX();
                    }
                }
                Patrol();
                isAggroed = false;
                isPatroling = true;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Patrol()
    {
        if (!canFlip) return;

        Vector2 direction = movingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(transform.localPosition, direction, 2, LayerMask.GetMask("EnemyOnlyWall", "Platform", "World"));
        if (wallHit.collider != null)
        {
            FlipDirection();
        }
    }


    private void FlipDirection()
    {
        if (!canFlip) return;
        movingRight = !movingRight;
        rb.velocity = new Vector3(movingRight ? speed : -speed, rb.velocity.y, 0);
        enemySprite.flipX = !movingRight;
        StartCoroutine(FlipCooldownRoutine());
    }
    private IEnumerator FlipCooldownRoutine()
    {
        canFlip = false;
        yield return new WaitForSeconds(flipCooldown);
        canFlip = true;
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

    protected void Die()
    {
        StopAllCoroutines();
        animator.Play("Death");
        animator.SetTrigger("Dead");
        animator.speed = 1;
        isDying = true;
        rb.isKinematic = true;
        bc.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        LootBag lootBag = GetComponent<LootBag>();
        Loot droppedItem = lootBag.GetDroppedItem();
        if (droppedItem != null && droppedItem.itemPrefab != null)
        {
            Instantiate(droppedItem.itemPrefab, transform.position, Quaternion.identity); // Spawn at monster's position
            Debug.Log("Item Dropped: " + droppedItem.lootName);
        }
        GetComponentInChildren<Canvas>().enabled = false;
        MySpawner.OnEnemyDestroyed();
        if (GameController.instance.playerMovement != null)
        {
            GameController.instance.playerMovement.GainExperience(expValue);
        }


    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
