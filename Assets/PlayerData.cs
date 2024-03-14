#nullable enable

using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [SerializeField]
    private float attackSpeed;

    [SerializeField]
    private float criticalRate;

    [SerializeField]
    private float criticalDamage;

    [SerializeField]
    private float attackDamage;
    private WeaponInstance ItemInstance;

    public float AttackSpeed => attackSpeed;
    public float CriticalRate => criticalRate;
    public float CriticalDamage => criticalDamage;
    public float AttackDamage => attackDamage;

    public void SetData(float? attackSpeed = null, float? criticalRate = null, float? criticalDamage = null, float? attackDamage = null, WeaponInstance? weaponInstance = null)
    {
        if (attackSpeed.HasValue)
        {
            this.attackSpeed = attackSpeed.Value;
        }

        if (criticalRate.HasValue)
        {
            this.criticalRate = criticalRate.Value;
        }

        if (criticalDamage.HasValue)
        {
            this.criticalDamage = criticalDamage.Value;
        }

        if (attackDamage.HasValue)
        {
            this.attackDamage = attackDamage.Value;
        }
    }
}
