using UnityEngine;

[CreateAssetMenu(fileName = "New Loot", menuName = "Loot Item")]
public class Loot : ScriptableObject
{
    public Sprite lootSprite;
    public string lootName;
    public int dropChance;
    public GameObject itemPrefab; // Reference to the loot prefab
}
