using UnityEngine;

public class ImpactAnim : MonoBehaviour
{
    public GameObject hitEffect; // Reference to the hit effect prefab
    private readonly float destroyDelay = 0.5f; // Delay before destroying the hit effect

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            // Getting the SpriteRenderer components from the children of both GameObjects
            SpriteRenderer enemySprite = GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer projectileSprite = collision.GetComponentInChildren<SpriteRenderer>();

            if (enemySprite == null || projectileSprite == null)
            {
                Debug.Log("Missing SpriteRenderer on either enemy or projectile.");
                return; // Exit if either SpriteRenderer is missing
            }

            // Using the enemy sprite's bounds to find the closest point to the projectile sprite's position
            Vector2 impactPoint = enemySprite.bounds.ClosestPoint(projectileSprite.transform.position);

            // Instantiate the hit effect at the calculated impact point
            Quaternion impactRotation = Quaternion.identity; // Adjust rotation as needed
            GameObject effect = Instantiate(hitEffect, impactPoint, impactRotation);

            Animator effectAnimator = effect.GetComponent<Animator>();
            if (effectAnimator != null)
            {
                effectAnimator.SetBool("PlayAnimation", true);
            }

            Destroy(effect, destroyDelay);
        }
    }
}
