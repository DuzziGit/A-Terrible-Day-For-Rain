using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [SerializeField]
    private float AttackSpeed;

    [SerializeField]
    private float CriticalRate;

    [SerializeField]
    private float CriticalDamage;

    [SerializeField]
    private float AttackDamage;

    private WeaponInstance ItemInstance;

    public void SetData(WeaponInstance instance)
    {
        AttackDamage = instance.AttackDamage;
        AttackSpeed = instance.AttackSpeed;
        CriticalDamage = instance.CriticalDamage;
        CriticalRate = instance.CriticalRate;
        ItemInstance = instance;
    }

    public void UpdateDisplay(WeaponInstance instance)
    {
        GetComponent<DisplayItemStats>().UpdateStats(instance);
    }


    public void SetPlayerData(){
        GameManager.Instance.Player.GetComponent<PlayerData>().SetData(attackSpeed: AttackSpeed, criticalRate:CriticalRate,criticalDamage:CriticalDamage,attackDamage:AttackDamage);
    }
}
