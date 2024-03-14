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
    private PlayerData playerData;

    [SerializeField]
    private float damageGrowthRate;

    [SerializeField]
    private int critMultiplier;

    [SerializeField]
    private int critChance;

    [SerializeField]
    private float TimeToWait;

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
        int skillModifier,
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
                skillModifier,
                attackId,
                hitPosition,
                enemyTransform,
                KnockbackStr
            )
        );
    }

    public int CalculateDamage(float weaponDamage, int skillModifierPercent)
    {
        // Define a multiplier for the damage range. For example, a 20% variance gives a range from 80% to 120% of the weapon damage.
        float variance = 0.2f;

        // Calculate min and max damage using the variance
        int minDamage = (int)Math.Round(weaponDamage * (1 - variance));
        int maxDamage = (int)Math.Round(weaponDamage * (1 + variance));

        // Apply the skill modifier to both min and max to adjust the range
        // The skillModifierPercent is expected to be a whole number (e.g., 150 for 150%).
        minDamage = Mathf.FloorToInt((int)Math.Round(minDamage * (skillModifierPercent / 100.0)));
        maxDamage = Mathf.FloorToInt((int)Math.Round(maxDamage * (skillModifierPercent / 100.0)));

        // Generate a random damage value within the range
        return UnityEngine.Random.Range(minDamage, maxDamage + 1);
    }

    private IEnumerator DelayedHitsCoroutine(
        Collider2D enemy,
        int totalHits,
        int skillModifier,
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
                int damage = CalculateDamage(playerData.AttackDamage, skillModifier);
                bool isCrit = UnityEngine.Random.value < (playerData.CriticalRate / 100f);
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
                yield return new WaitForSeconds(TimeToWait);
            }
            else
            {
                break;
            }
        }
    }
}
