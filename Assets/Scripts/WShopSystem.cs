using System;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
public class WeaponItem
{
    public string itemName; // Name of the item
    public GameObject itemPrefab; // Prefab of the item to be purchased
    public int price; // Price of the item
    public bool isPurchased; // Flag to check if the item is purchased
    public string description; // Description of the item
}


public class WShopSystem : MonoBehaviour
{

    public List<WeaponItem> shopItems; // List of items available in the shop
    public GameObject shopCanvasUI; // Canvas for the shop UI
    public GameObject shopContainerUI; // Place to store all the items in the shop
    public ShopSpawner shopSpawner; // Handles spawning of items in the shop


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void PurchaseItem(string itemName)
    {
        // Find the item in the shopItems list
        WeaponItem item = shopItems.Find(i => i.itemName.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        
        if (item == null)
        {
            Debug.LogWarning("Item not found: " + itemName);
            return;
        }

        // Check if the item is already purchased
        if (item.isPurchased)
        {
            Debug.Log("Item already purchased: " + itemName);
            return;
        }

        // Proceed with purchasing the item
        BuyItem(item);
    }
    void BuyItem(WeaponItem item){
        if (item != null && !item.isPurchased)
        {
            // Deduct the item price from the player's currency
            // Assuming you have a PlayerCurrencyManager to handle player currency
            //PlayerCurrencyManager.Instance.DeductCurrency(item.price);
            item.isPurchased = true;

            // Spawn the item in the game world
            shopSpawner.SpawnItem(item.itemPrefab);
        }
    }
}
