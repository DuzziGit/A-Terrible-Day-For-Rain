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

    // Determine the minimum values allowed based on rarity.
    // This ensures that lower rarities cannot have higher values than higher rarities.
    Vector2 critRateRange = new Vector2(
        baseWeapon.baseCriticalRate.x * (1 + rarityMultipliers[rarityIndex]),
        baseWeapon.baseCriticalRate.y * rarityMultipliers[rarityIndex]
    );
    CriticalRate = Random.Range(critRateRange.x, critRateRange.y);

    Vector2 critDamageRange = new Vector2(
        baseWeapon.baseCriticalDamage.x * (1 + rarityMultipliers[rarityIndex]),
        baseWeapon.baseCriticalDamage.y * rarityMultipliers[rarityIndex]
    );
    CriticalDamage = Random.Range(critDamageRange.x, critDamageRange.y);

    Vector2 attackDamageRange = new Vector2(
        baseWeapon.baseAttackDamage.x * (1 + rarityMultipliers[rarityIndex]),
        baseWeapon.baseAttackDamage.y * rarityMultipliers[rarityIndex]
    );
    AttackDamage = Random.Range(attackDamageRange.x, attackDamageRange.y);

    AttackSpeed = CalculateAttackSpeed(attackSpeedRanges[rarityIndex]);

    // Apply a universal level multiplier to all stats
    float levelMultiplier = 1.0f + (characterLevel * 0.01f);
    CriticalRate *= levelMultiplier;
    CriticalDamage *= levelMultiplier;
    AttackDamage *= levelMultiplier;
    // Optionally adjust AttackSpeed with levelMultiplier if needed
   // Check for caps and round off if the stats are higher than the base cap
    if (CriticalRate > baseWeapon.baseCriticalRate.y)
    {
        CriticalRate = Mathf.Floor(baseWeapon.baseCriticalRate.y);
    }
    if (CriticalDamage > baseWeapon.baseCriticalDamage.y)
    {
        CriticalDamage = Mathf.Floor(baseWeapon.baseCriticalDamage.y);
    }

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
