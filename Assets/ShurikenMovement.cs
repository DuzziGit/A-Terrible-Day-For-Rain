using UnityEngine;

public class ShurikenMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    protected float maxDistance = 13;
    private Vector3 initialPosition;

    public void SetMovement(Vector3 direction, float speed, Vector3 startPosition)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;
        initialPosition = startPosition; // Use the startPosition as the initial position
    }

    private void Start() { }

    private void Update()
    {
        float distanceTraveled = Vector3.Distance(transform.position, initialPosition);
        if (distanceTraveled >= maxDistance)
        {
            //Debug.Log("exceeded max distance per shuriken");
            DestroyProjectile();
        }
    }

    protected void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
