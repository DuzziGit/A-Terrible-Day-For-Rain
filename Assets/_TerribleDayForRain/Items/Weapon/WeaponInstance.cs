using UnityEngine;

public class WeaponInstance
{
    public float AttackDamage;
    public float AttackSpeed;
    public float CriticalRate;
    public float CriticalDamage;
    public string Title;

    // Define arrays to hold the maximum multipliers for each rarity
    private float[] rarityMultipliers = new float[] { 0.25f, 0.5f, 0.75f, 1f }; // Assuming Common, Rare, Epic, Legendary
    private readonly Vector2[] attackSpeedRanges =
    {
        new Vector2(1.0f, 0.9f), // Common
        new Vector2(0.9f, 0.75f), // Rare
        new Vector2(0.75f, 0.5f), // Epic
        new Vector2(0.5f, 0.2f) // Legendary
    };

    public WeaponInstance(WeaponLoot baseWeapon, Rarity rarity, int characterLevel)
    {
        InitializeWeapon(baseWeapon, rarity, characterLevel);
    }

    private void InitializeWeapon(WeaponLoot baseWeapon, Rarity rarity, int characterLevel)
    {
        float critRateMin = baseWeapon.baseCriticalRate.x;
        float critDamageMin = baseWeapon.baseCriticalDamage.x;
        float attackDamageMin = baseWeapon.baseAttackDamage.x;

        int rarityIndex = (int)rarity;
        if (rarityIndex != 0)
        {
            critRateMin =
                (baseWeapon.baseCriticalRate.x * rarityMultipliers[rarityIndex - 1])
                + baseWeapon.baseCriticalRate.x;

                  critDamageMin =
                (baseWeapon.baseCriticalDamage.x * rarityMultipliers[rarityIndex - 1])
                + baseWeapon.baseCriticalDamage.x;

                  attackDamageMin =
                (baseWeapon.baseAttackDamage.x * rarityMultipliers[rarityIndex - 1])
                + baseWeapon.baseAttackDamage.x;
        }
        float critRateCap =
            (baseWeapon.baseCriticalRate.x * rarityMultipliers[rarityIndex])
            + baseWeapon.baseCriticalRate.x;
        CriticalRate = Random.Range(critRateMin, critRateCap);

        // CriticalDamage
        float critDamageCap =
            (baseWeapon.baseCriticalDamage.x * rarityMultipliers[rarityIndex])
            + baseWeapon.baseCriticalDamage.x;

        CriticalDamage = Random.Range(critDamageMin, critDamageCap);

        // AttackDamage
        float attackDamageCap =
            (baseWeapon.baseAttackDamage.x * rarityMultipliers[rarityIndex])
            + baseWeapon.baseAttackDamage.x;

        AttackDamage = Random.Range( attackDamageMin, attackDamageCap);

        AttackSpeed = CalculateAttackSpeed(attackSpeedRanges[rarityIndex]);

    //     // // Apply a universal level multiplier to all stats
    //     float levelMultiplier = 1.0f + (characterLevel * 0.01f);
    //     CriticalRate *= levelMultiplier;
    //     CriticalDamage *= levelMultiplier;
    //     AttackDamage *= levelMultiplier;
    //   //  Set a title for the weapon
        Title = $"{rarity} Tier Weapon";
    }

    private float CalculateAttackSpeed(Vector2 speedRange)
    {
        // Randomly determine the attack speed within the specified range, with lower values indicating faster speed.
        // Important: The 'y' value is the faster speed (lower number), so it's the first argument.
        return Random.Range(speedRange.y, speedRange.x);
    }
}
