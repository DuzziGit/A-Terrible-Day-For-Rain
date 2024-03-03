using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{

    public List<Loot> lootList = new();
    public GameObject droppedItemPrefab;
    public Loot GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new();
        foreach (Loot item in lootList)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        if (possibleItems.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            return possibleItems[randomIndex];
        }
        else
        {
            // Handle the scenario when no items are found
            return null; // Or you can return a default Loot item or throw an exception
        }
    }
    public void InstantiateItem(Vector3 spawnPosition)
    {

        Loot droppedItem = GetDroppedItem();
        if (droppedItem != null)
        {

            GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
            lootGameObject.GetComponentInChildren<SpriteRenderer>().sprite = droppedItem.lootSprite;
        }

    }

}