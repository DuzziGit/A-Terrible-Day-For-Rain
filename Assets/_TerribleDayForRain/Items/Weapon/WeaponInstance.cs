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
        int rarityIndex = (int)rarity;

        // CriticalRate
        float critRateCap =
            baseWeapon.baseCriticalRate.x
            + (baseWeapon.baseCriticalRate.y - baseWeapon.baseCriticalRate.x)
                * rarityMultipliers[rarityIndex];
        CriticalRate = Random.Range(baseWeapon.baseCriticalRate.x, critRateCap);

        // CriticalDamage
        float critDamageCap =
            baseWeapon.baseCriticalDamage.x
            + (baseWeapon.baseCriticalDamage.y - baseWeapon.baseCriticalDamage.x)
                * rarityMultipliers[rarityIndex];
        CriticalDamage = Random.Range(baseWeapon.baseCriticalDamage.x, critDamageCap);

        // AttackDamage
        float attackDamageCap =
            baseWeapon.baseAttackDamage.x
            + (baseWeapon.baseAttackDamage.y - baseWeapon.baseAttackDamage.x)
                * rarityMultipliers[rarityIndex];
        AttackDamage = Random.Range(baseWeapon.baseAttackDamage.x, attackDamageCap);
   // Scale the rarity effect more significantly
        float rarityEffectScale = 0.15f * rarityIndex; // Increase this value to make rarity have a bigger impact

         AttackSpeed = CalculateAttackSpeed(attackSpeedRanges[rarityIndex]);


        // Apply a universal level multiplier to all stats
        float levelMultiplier = 1.0f + (characterLevel * 0.01f);
        CriticalRate *= levelMultiplier;
        CriticalDamage *= levelMultiplier;
        AttackDamage *= levelMultiplier;
        // Optionally adjust AttackSpeed with levelMultiplier if needed

        // Set a title for the weapon
        Title = $"{rarity} Tier Weapon";
    }
        private float CalculateAttackSpeed(Vector2 speedRange)
    {
        // Randomly determine the attack speed within the specified range, with lower values indicating faster speed.
        // Important: The 'y' value is the faster speed (lower number), so it's the first argument.
        return Random.Range(speedRange.y, speedRange.x);
    }
}
