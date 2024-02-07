using UnityEngine;

public class ImpactAnim : MonoBehaviour
{
    public GameObject hitEffect; // Reference to the first hit effect prefab
    public GameObject hitEffect2; // Reference to the second hit effect prefab
    public Animator animation1;
    public Animator animation2;
    private float destroyDelay = 0.5f; // Delay before destroying the projectile

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            //Debug.Log("hit");

            // Instantiate the hit effects at the impact point
            Vector2 collisionPoint = collision.transform.position;

            // Get the enemy's rotation
            Quaternion enemyRotation = collision.transform.rotation;

            // Instantiate the hit effects with the enemy's position and the original scale
            GameObject effect1 = Instantiate(hitEffect, collisionPoint, enemyRotation, this.transform);
            GameObject effect2 = Instantiate(hitEffect2, collisionPoint, enemyRotation, this.transform);

            // Play the animations for the hit effects
            if (animation1 != null)
            {
                animation1.SetBool("PlayAnimation", true);
                //Debug.Log("Animation 1 Played");
            }
            if (animation2 != null)
            {
                animation2.SetBool("PlayAnimation", true);
                //Debug.Log("Animation 2 Played");
            }

            Destroy(effect1, destroyDelay); // Destroy the first hit effect after the delay
            Destroy(effect2, destroyDelay); // Destroy the second hit effect after the delay


        }
    }
}
