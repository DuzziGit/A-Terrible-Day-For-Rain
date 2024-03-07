using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    private static HitManager _instance;
    public static HitManager Instance
    {
        get { return _instance; }
    }

    [SerializeField]
    private PlayerMovement PlayerLevel;

    [SerializeField]
    private float damageGrowthRate;

    [SerializeField]
    private int critMultiplier;

    [SerializeField]
    private int critChance;

    [SerializeField]
    private float hitCooldown;

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

    public void ApplyDelayedHits(
        Collider2D enemy,
        int totalHits,
        int baseMinDamage,
        int baseMaxDamage,
        string attackId,
        Vector2 hitPosition,
        Transform enemyTransform,
        float KnockbackStr
    )
    {
        StartCoroutine(
            DelayedHitsCoroutine(
                enemy,
                totalHits,
                baseMinDamage,
                baseMaxDamage,
                attackId,
                hitPosition,
                enemyTransform,
                KnockbackStr
            )
        );
    }

    private int CalculateDamageForLevel(int baseMinDamage, int baseMaxDamage)
    {
        int plevel = PlayerLevel.level;
        int minDamageAtLevel = Mathf.FloorToInt(
            baseMinDamage * Mathf.Pow(damageGrowthRate, (plevel - 1))
        );
        int maxDamageAtLevel = Mathf.FloorToInt(
            baseMaxDamage * Mathf.Pow(damageGrowthRate, (plevel - 1))
        );

        return UnityEngine.Random.Range(minDamageAtLevel, maxDamageAtLevel + 1);
    }

    private IEnumerator DelayedHitsCoroutine(
        Collider2D enemy,
        int totalHits,
        int baseMinDamage,
        int baseMaxDamage,
        string attackId,
        Vector2 hitPosition,
        Transform enemyTransform,
        float KnockbackStr
    )
    {
        int hitsApplied = 0;
        while (hitsApplied < totalHits)
        {
            if (enemy != null)
            {
                // Calculate damage and critical hit status here
                int damage = CalculateDamageForLevel(baseMinDamage, baseMaxDamage);
                bool isCrit = UnityEngine.Random.value < (critChance / 100f);
                if (isCrit)
                {
                    damage *= critMultiplier;
                }

                // Calculate hit direction
                Vector2 hitDirection = (hitPosition - (Vector2)enemyTransform.position).normalized;

                enemy
                    .GetComponent<EnemyCon>()
                    .TakeDamage(damage, isCrit, attackId, hitDirection, KnockbackStr);
                hitsApplied++;
                //                Debug.Log(attackId + " Hit enemy for " + damage);
                yield return new WaitForSeconds(hitCooldown);
            }
            else
            {
                break;
            }
        }
    }
}
