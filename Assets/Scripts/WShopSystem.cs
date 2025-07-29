using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

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
    public PlayerCurrencyManager playerCurrencyManager; // Reference to the player's currency manager

    public List<WeaponItem> shopItems; // List of items available in the shop
    public GameObject shopCanvasUI; // Canvas for the shop UI
    public GameObject shopContainerUI; // Place to store all the items in the shop
    public ShopSpawner shopSpawner; // Handles spawning of items in the shop

    public GameObject shopButtonPrefab;
    public TextMeshProUGUI creditsText; // Text to display player's credits



    void Start()
    {
        // initialize the shop UI

        if (shopContainerUI != null)
        {
            foreach (var item in shopItems)
            {
                GameObject button = Instantiate(shopButtonPrefab, shopContainerUI.transform);
                button.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;
                button.GetComponent<Button>().onClick.AddListener(() => PurchaseItem(item.itemName));
            }
        }
        else
        {
            Debug.LogWarning("Shop container UI is not set.");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Update the credits text
        if (creditsText != null)
        {
            if (playerCurrencyManager != null)
            {
                creditsText.text = "Credits: " + playerCurrencyManager.credits.ToString();
            }
        }
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

        // Check if the player can afford the item
        if (playerCurrencyManager.CanAfford(item.price))
        {
            // Deduct the item price from the player's currency
            playerCurrencyManager.SpendCredits(item.price);

            // Buy the item
            BuyItem(item);
        }
        else
        {
            Debug.LogWarning("Not enough credits to purchase: " + itemName);
        }
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
