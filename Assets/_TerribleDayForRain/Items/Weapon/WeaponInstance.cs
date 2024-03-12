using UnityEngine;
using Unity.Collections;

public class WeaponInstance
{
    public float AttackDamage;
    public float AttackSpeed;
    public float CriticalRate;
    public float CriticalDamage;
    public string Title; 

    public WeaponInstance(WeaponLoot baseWeapon, Rarity rarity, int characterLevel)
    {
        InitializeWeapon(baseWeapon, rarity, characterLevel);
    }

    private void InitializeWeapon(WeaponLoot baseWeapon, Rarity rarity, int characterLevel)
    {
        // Randomize stats based on the baseWeapon's properties and rarity
        AttackSpeed = Random.Range(baseWeapon.baseAttackSpeed.x, baseWeapon.baseAttackSpeed.y);
        CriticalRate = Random.Range(baseWeapon.baseCriticalRate.x, baseWeapon.baseCriticalRate.y);
        CriticalDamage = Random.Range(baseWeapon.baseCriticalDamage.x, baseWeapon.baseCriticalDamage.y);
        AttackDamage = Random.Range(baseWeapon.baseAttackDamage.x, baseWeapon.baseAttackDamage.y);

        // Apply some scaling or modifiers based on rarity and character level
        float rarityMultiplier = 1.0f + ((float)rarity / (float)Rarity.Legendary);
        float levelMultiplier = 1.0f + (characterLevel * 0.01f); 

        // Apply multipliers to the randomized stats
        AttackSpeed *= rarityMultiplier;
        CriticalRate *= rarityMultiplier;
        CriticalDamage *= rarityMultiplier;
        AttackDamage *= levelMultiplier;

        // Set a title for the weapon
        Title = $"{rarity} Tier Weapon"; 
    }
}
