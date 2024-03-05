using TMPro;
using UnityEngine;

public class DisplayItemStats : MonoBehaviour
{
    public float AttackDamage;
    public float AttackSpeed;
    public float CriticalDamage;
    public float CriticalRate;
    public string Title;
    public Sprite PreviewImage;
    public int LevelReq;

    public TMP_Text AttackDamageText;
    public TMP_Text AttackSpeedText;
    public TMP_Text CriticalDamageText;
    public TMP_Text CriticalRateText;
    public TMP_Text TitleText;
    public GameObject ItemPreview; // Make sure this is assigned in the inspector to your preview prefab or GameObject

    // Method to update stats
    public void UpdateStats(WeaponLoot loot)
    {
        // Update the internal stats
        AttackDamage = loot.AttackDamage;
        AttackSpeed = loot.AttackSpeed;
        CriticalRate = loot.CriticalRate;
        CriticalDamage = loot.CriticalDamage;
        Title = loot.lootName;
        PreviewImage = loot.lootSprite;
        //LevelReq = loot.LevelReq; // Uncomment and use if Level Requirement is part of your system

        // Update the UI elements with direct assignments
        AttackDamageText.text = AttackDamage.ToString();
        AttackSpeedText.text = AttackSpeed.ToString();
        CriticalDamageText.text = CriticalDamage.ToString();
        CriticalRateText.text = CriticalRate.ToString();
        TitleText.text = Title;
    }

}
