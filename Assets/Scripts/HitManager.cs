using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;

public class HitManager : MonoBehaviour
{
    private static HitManager _instance;
    public static HitManager Instance { get { return _instance; } }
    [SerializeField] private PlayerMovement PlayerLevel;
    [SerializeField] private int baseMinDamage;
    [SerializeField] private int baseMaxDamage;
    [SerializeField] private float damageGrowthRate;
    [SerializeField] private int critMultiplier;
    [SerializeField] private int critChance;



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public void ApplyDelayedHits(Collider2D enemy, int totalHits, float hitCooldown)
    {
        StartCoroutine(DelayedHitsCoroutine(enemy, totalHits, hitCooldown));
    }
    private int CalculateDamageForLevel()
    {
        int plevel = PlayerLevel.level;
        int minDamageAtLevel = Mathf.FloorToInt(baseMinDamage * Mathf.Pow(damageGrowthRate, (plevel - 1)));
        int maxDamageAtLevel = Mathf.FloorToInt(baseMaxDamage * Mathf.Pow(damageGrowthRate, (plevel - 1)));

        return Random.Range(minDamageAtLevel, maxDamageAtLevel + 1);
    }
    private IEnumerator DelayedHitsCoroutine(Collider2D enemy, int totalHits, float hitCooldown)
    {
        int hitsApplied = 0;
        while (hitsApplied < totalHits)
        {
            if (enemy != null)
            {
                // Calculate damage and critical hit status here
                int damage = CalculateDamageForLevel(); // Assuming this method is now accessible here
                bool isCrit = Random.value < (critChance / 100f);
                if (isCrit)
                {
                    damage = Mathf.FloorToInt(damage * critMultiplier);
                }

                enemy.GetComponent<EnemyCon>().TakeDamage(damage, isCrit);
                hitsApplied++;
                yield return new WaitForSeconds(hitCooldown);
            }
            else
            {
                break;
            }
        }
    }
}
