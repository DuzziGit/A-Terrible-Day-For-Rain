using System.Collections;
using UnityEngine;

public class ImpactAnim : MonoBehaviour
{
    private readonly float destroyDelay = 0.5f; // Delay before returning the hit effect to the pool

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            SpriteRenderer enemySprite = GetComponentInChildren<SpriteRenderer>();
            SpriteRenderer projectileSprite = collision.GetComponentInChildren<SpriteRenderer>();

            if (enemySprite == null || projectileSprite == null)
            {
                Debug.Log("Missing SpriteRenderer on either enemy or projectile.");
                return;
            }

            Vector2 impactPoint = enemySprite.bounds.ClosestPoint(
                projectileSprite.transform.position
            );
            Quaternion impactRotation = Quaternion.identity;

            // Get a hit effect from the pool instead of instantiating a new one
            GameObject effect = HitEffectPool.Instance.GetHitEffect();
            effect.transform.position = impactPoint;
            effect.transform.rotation = impactRotation;

            Animator effectAnimator = effect.GetComponent<Animator>();
            if (effectAnimator != null)
            {
                effectAnimator.SetBool("PlayAnimation", true);
            }

            // Return the hit effect to the pool after a delay
            StartCoroutine(ReturnEffectToPool(effect));
        }
    }

    private IEnumerator ReturnEffectToPool(GameObject effect)
    {
        yield return new WaitForSeconds(destroyDelay);
        HitEffectPool.Instance.ReturnHitEffectToPool(effect);
    }
}
