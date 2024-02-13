using System.Collections.Generic;
using Unity.VisualScripting;
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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision))
        {
            DealDamage(collision);
            hitEnemies.Add(collision);
        }
    }
    protected override void OnTriggerExit2D(Collider2D other)
    {
        return;

    }
    protected override void OnTriggerStay2D(Collider2D other)
    {
        return;
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