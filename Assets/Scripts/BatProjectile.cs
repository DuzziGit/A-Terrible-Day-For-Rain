using UnityEngine;

public class BatProjectile : MonoBehaviour
{

    public Vector2 target;
    int level;
    public float speed;
    public float lifeTime;
    public float distance;
    public LayerMask whatIsSolid;
    public int projectileDamage;
    public GameObject RogueLoot1;
    public bool hasDmged = false;


    private void Start()
    {

        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        level = GameObject.FindGameObjectWithTag("Bat").GetComponent<Enemy>().level;
        projectileDamage = level * 6;

        target = new Vector2(player.position.x, player.position.y);
        hasDmged = false;
        Invoke("DestroyProjectile", lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (transform.position.x == target.x && transform.position.y == target.y)
        {

            DestroyProjectile();

        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!hasDmged)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().UpdateHealth(-projectileDamage);
                Debug.Log("Hit Player");
                hasDmged = true;

            }
            DestroyProjectile();
        }
    }


    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}


