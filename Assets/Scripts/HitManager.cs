using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class HitManager : MonoBehaviour
{
    private static HitManager _instance;
    public static HitManager Instance { get { return _instance; } }
    [SerializeField] private PlayerMovement PlayerLevel;
    [SerializeField] private float damageGrowthRate;
    [SerializeField] private int critMultiplier;
    [SerializeField] private int critChance;
    [SerializeField] private float hitCooldown;


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
    public static string GenerateSkillActivationGuid()
    {
        return Guid.NewGuid().ToString();
    }

    public void ApplyDelayedHits(Collider2D enemy, int totalHits, int baseMinDamage, int baseMaxDamage, string attackId)
    {
        StartCoroutine(DelayedHitsCoroutine(enemy, totalHits, baseMinDamage, baseMaxDamage, attackId));
    }
    private int CalculateDamageForLevel(int baseMinDamage, int baseMaxDamage)
    {
        int plevel = PlayerLevel.level;
        int minDamageAtLevel = Mathf.FloorToInt(baseMinDamage * Mathf.Pow(damageGrowthRate, (plevel - 1)));
        int maxDamageAtLevel = Mathf.FloorToInt(baseMaxDamage * Mathf.Pow(damageGrowthRate, (plevel - 1)));

        return UnityEngine.Random.Range(minDamageAtLevel, maxDamageAtLevel + 1);
    }
    private IEnumerator DelayedHitsCoroutine(Collider2D enemy, int totalHits, int baseMinDamage, int baseMaxDamage, string attackId)
    {
        int hitsApplied = 0;
        while (hitsApplied < totalHits)
        {
            if (enemy != null)
            {
                // Calculate damage and critical hit status here
                int damage = CalculateDamageForLevel(baseMinDamage, baseMaxDamage); // Assuming this method is now accessible here
                bool isCrit = UnityEngine.Random.value < (critChance / 100f);
                if (isCrit)
                {
                    damage = Mathf.FloorToInt(damage * critMultiplier);
                }

                enemy.GetComponent<EnemyCon>().TakeDamage(damage, isCrit, attackId);
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
