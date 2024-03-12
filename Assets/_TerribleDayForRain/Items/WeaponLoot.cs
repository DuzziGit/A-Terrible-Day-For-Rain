using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Loot", menuName = "WeaponLoot")]
public class WeaponLoot : Loot
{
 // Stats for a single tier
    public Vector2 baseAttackSpeed;
    public Vector2 baseCriticalRate;
    public Vector2 baseCriticalDamage;
    public Vector2 baseAttackDamage;
}

public enum Rarity
{
    Common, // Assumes value 0
    Rare,   // Assumes value 1
    Epic,   // Assumes value 2
    Legendary // Assumes value 3
}
