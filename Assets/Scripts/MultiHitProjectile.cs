using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MultiHitProjectile : Projectile
{
    private Dictionary<Collider2D, int> hitEnemies = new Dictionary<Collider2D, int>();
    [SerializeField] private bool isStuck = false;
    void Update()
    {
        base.Update();
        if (!isStuck)
        {
            direction = transform.right;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.ContainsKey(collision))
        {
            // Call the HitManager to handle the remaining hits
            HitManager.Instance.ApplyDelayedHits(collision, TotalHits, MinDamage, MaxDamage);
        }
    }

    // protected override void OnTriggerStay2D(Collider2D other)
    // {
    //     return;
    // }

    // protected override void OnTriggerExit2D(Collider2D other)
    // {
    //     return;
    // }

}