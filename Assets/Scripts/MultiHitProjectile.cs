using System.Collections.Generic;
using UnityEngine;

public class MultiHitProjectile : Projectile
{
    public int totalHits = 2;
    private int currentHits = 0;
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    private void Update()
    {
        base.Update();
        direction = transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision))
        {
            DealDamage(collision);
            hitEnemies.Add(collision);
        }
    }

    private void DealDamage(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //   Debug.Log("ENEMY MUST TAKE DAMAGE !" + damage);
            collision.GetComponent<EnemyCon>().TakeDamage(damage);
            collision.GetComponent<EnemyCon>().TakeDamage(damage);

        }

    }
}