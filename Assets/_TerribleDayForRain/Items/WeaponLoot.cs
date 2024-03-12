using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Loot", menuName = "WeaponLoot")]
public class WeaponLoot : Loot
{
    public float BaseDamage;
    public float AttackSpeed;
    public float CriticalRate;
    public float CriticalDamage;

    // Initial ranges for each tier
    public float[] attackSpeedRangePerTier = new float[] { 1f, 0.9f, 0.75f, 0.5f }; // Minimum attack speed per tier
    public float[] criticalRateRangePerTier = new float[] { 1f, 10f, 20f, 30f }; // Minimum critical rate per tier
    public float[] criticalDamageRangePerTier = new float[] { 10f, 15f, 30f, 45f }; // Minimum critical damage per tier
    public float[] baseDamagePerTier = new float[] { 1000f, 5000f, 25000f, 125000f }; // Base damage per tier

    public void InitializeWeapon(int tierIndex, Rarity rarity, int characterLevel)
    {
        // Calculate stats based on tier and rarity
        float attackSpeedModifier = (float)rarity / (float)Rarity.Legendary; // This assumes Rarity enum values start at 0 and increase
        AttackSpeed = attackSpeedRangePerTier[tierIndex] - attackSpeedModifier * (attackSpeedRangePerTier[tierIndex] - 0.2f);

        float criticalRateModifier = (float)rarity / (float)Rarity.Legendary;
        CriticalRate = criticalRateRangePerTier[tierIndex] + criticalRateModifier * (40f - criticalRateRangePerTier[tierIndex]);

        float criticalDamageModifier = (float)rarity / (float)Rarity.Legendary;
        CriticalDamage = criticalDamageRangePerTier[tierIndex] + criticalDamageModifier * (150f - criticalDamageRangePerTier[tierIndex]);

        // Scale base damage based on tier and a small modifier per level within the tier
        BaseDamage = baseDamagePerTier[tierIndex] * (1 + (characterLevel - tierIndex * 15) * 0.01f); // 1% increase per level in tier
    }
}

public enum Rarity
{
    Common, // Assumes value 0
    Rare,   // Assumes value 1
    Epic,   // Assumes value 2
    Legendary // Assumes value 3
}
