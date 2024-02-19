using UnityEngine;

public class ImpactAnim : MonoBehaviour
{
    public GameObject hitEffect; // Reference to the first hit effect prefab
    public Animator animation1;
    private readonly float destroyDelay = 0.5f; // Delay before destroying the projectile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            Vector2 collisionPoint = collision.transform.position;
            Quaternion enemyRotation = collision.transform.rotation;

            GameObject effect1 = Instantiate(hitEffect, collisionPoint, enemyRotation);

            Animator effect1Animator = effect1.GetComponent<Animator>();

            if (effect1Animator != null)
            {
                effect1Animator.SetBool("PlayAnimation", true);
            }

            Destroy(effect1, destroyDelay);
        }
    }

}
