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

    // Method to update stats
    public void UpdateStats(WeaponLoot loot)
    {
        AttackDamage = loot.AttackDamage;
        AttackSpeed = loot.AttackSpeed;
        CriticalRate = loot.CriticalRate;
        CriticalDamage = loot.CriticalDamage;
        Title = loot.lootName;
        PreviewImage = loot.lootSprite;
        LevelReq = loot.LevelReq;

        // You can extend this method to include any other properties that need to be set
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {

        }
    }
}
