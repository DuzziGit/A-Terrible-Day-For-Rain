using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PulsingSummon : MonoBehaviour
{
    [SerializeField] private float pulseTimer = 2f;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float offsetY;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int hitCap = 3;
    [SerializeField] private bool destroyAfterHitCap = false;
    [SerializeField] private int totalHits;
    [SerializeField] private int minDamage;
    [SerializeField] private float lifetime;
    protected int TotalHits
    {
        get { return totalHits; }
    }
    protected int MinDamage
    {
        get { return minDamage; }
        set { minDamage = value; }
    }
    [SerializeField] private int maxDamage;
    protected int MaxDamage
    {
        get { return maxDamage; }
        set { maxDamage = value; }
    }
    private int hitCount = 0;
    private Dictionary<Collider2D, int> hitEnemies = new Dictionary<Collider2D, int>();

    void Start()
    {
        StartCoroutine(PulseDamage());
        Invoke("Destroy", lifetime);
    }

    private void Update()
    {
        transform.localPosition = Vector3.zero;

    }
    public IEnumerator PulseDamage()
    {
        while (hitCount < hitCap || !destroyAfterHitCap)
        {
            Collider2D[] results = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, offsetY, 0), detectionRadius, enemyLayer);
            foreach (var result in results)
            {
                if (!hitEnemies.ContainsKey(result))
                {
                    HitManager.Instance.ApplyDelayedHits(result, TotalHits, MinDamage, MaxDamage);
                    hitCount++;
                    hitEnemies[result] = 1; // Track this enemy as hit

                    if (hitCount >= hitCap && destroyAfterHitCap)
                    {
                        Destroy(gameObject);
                        yield break; // Exit the coroutine
                    }
                }
            }
            yield return new WaitForSeconds(pulseTimer);
            hitEnemies.Clear();
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, offsetY, 0), detectionRadius);
    }
}