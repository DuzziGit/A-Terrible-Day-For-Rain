using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public int currentLootTier = 0;

    [SerializeField]
    public int[] Tiers;

    [SerializeField]
    public float[] Weights;

    private int _rarity;
    public static LootManager Instance;

    [SerializeField]
    private WeaponLoot[] baseWeaponsPerTier;

    [SerializeField]
    private float[] dropRatesPerTier = new float[3] { 0.2f, 0.15f, 0.2f, };
    public GameObject droppedItemPrefab;
private WeaponLoot weaponLootData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void GenerateLoot()
    {
        _rarity = RarityGenerator.WeightedProb(Tiers, Weights);
    }

 public void HandleLootDrop(Vector3 spawnPosition)
    {
        float dropRate = dropRatesPerTier[currentLootTier];
        bool shouldDrop = Random.value < dropRate;

        if (shouldDrop)
        {
            GenerateLoot(); // Sets _rarity based on weighted probability
            Rarity rarity = (Rarity)_rarity; // Cast the generated rarity to Rarity enum
            weaponLootData = baseWeaponsPerTier[currentLootTier];
            int characterLevel = GameManager.Instance.playerMovement.level;

            InstantiateItem(spawnPosition, weaponLootData, currentLootTier, rarity, characterLevel);
        }
    }

    public void InstantiateItem(
        Vector3 spawnPosition,
        WeaponLoot weaponLootData,
        int tierIndex, 
        Rarity rarity, 
        int characterLevel 
    )
    {

        // Instantiate the dropped item in the game world
        GameObject droppedItem = Instantiate(
            droppedItemPrefab,
            spawnPosition,
            Quaternion.identity
        );

        DisplayItemStats displayComponent = droppedItem.GetComponent<DisplayItemStats>();
        if (displayComponent != null)
        {
       WeaponInstance weaponInstance = new WeaponInstance(weaponLootData, rarity, characterLevel);


            // Update the display with the stats from the new weapon instance
            displayComponent.UpdateStats(weaponInstance);
        }
        else
        {
            Debug.LogWarning("DisplayItemStats component not found on the dropped item prefab.");
        }
    }
}
