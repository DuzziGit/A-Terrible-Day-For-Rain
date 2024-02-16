using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MultiHitProjectile : Projectile
{
    public int totalHits = 4;
    private Dictionary<Collider2D, int> hitEnemies = new Dictionary<Collider2D, int>();
    private Dictionary<Collider2D, float> lastHitTime = new Dictionary<Collider2D, float>();
    private float hitCooldown = 0.01f; // Time in seconds before the same enemy can be hit again
    int currentDamage;
    int pLevel;

    void Update()
    {
        base.Update();
        direction = transform.right;
        pLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>().level; // Update player level
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.ContainsKey(collision))
        {
            // Call the HitManager to handle the remaining hits
            HitManager.Instance.ApplyDelayedHits(collision, totalHits - 1, hitCooldown); // Adjust totalHits as needed
        }
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        return;
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        return;
    }

    public void DealDamage(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Recalculate damage each time before applying
            int currentDamage = CalculateDamageForLevel(pLevel); // Assuming pLevel is accessible

            // Implementing critical hits
            bool currentIsCrit = Random.value < (critChance / 100f);
            if (currentIsCrit)
            {
                currentDamage = Mathf.FloorToInt(currentDamage * critMultiplier); // Apply crit multiplier
            }

            collision.GetComponent<EnemyCon>().TakeDamage(currentDamage, currentIsCrit);
        }
    }
}