using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MultiHitProjectile : Projectile
{
    [SerializeField] private bool Stationary = false;
    [SerializeField] private int HitCap = 3;
    [SerializeField] private bool DestroyAfterHitCap = false;
    [SerializeField] private bool Cosmetic = false;
    private int hitCount;
    private Dictionary<Collider2D, int> hitEnemies = new Dictionary<Collider2D, int>();
    void Update()
    {
        base.Update();
        if (hitCount >= HitCap && DestroyAfterHitCap)
        {
            DestroyProjectile();

        }
        if (!Stationary)
        {
            direction = transform.right;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.ContainsKey(collision) && hitCount < HitCap)
        {
            if (!Cosmetic)
            {
                HitManager.Instance.ApplyDelayedHits(collision, TotalHits, MinDamage, MaxDamage, UniqueAttackId);
            }
            // Call the HitManager to handle the remaining hits
            hitCount++;
        }
    }


}