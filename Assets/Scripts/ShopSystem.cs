using System;
using UnityEngine;
using System.Collections.Generic;
[Serializable]
public class ShopItem
{
    public string itemName; // Name of the item
    public GameObject itemPrefab; // Prefab of the item to be purchased
    public int price; // Price of the item
    public bool isPurchased; // Flag to check if the item is purchased
    public string description; // Description of the item
}


public class ShopSystem : MonoBehaviour
{

    public List<ShopItem> shopItems; // List of items available in the shop
    public GameObject shopCanvasUI; // Canvas for the shop UI
    public GameObject shopContainerUI; // Place to store all the items in the shop
    public ShopSlotSystem shopSlot; // Handles spawning of items in the shop


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered the shop trigger area.");
        if (other.CompareTag("Player"))
        {
            // Show the shop UI
            shopCanvasUI.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hide the shop UI
            shopCanvasUI.SetActive(false);
        }
    }
}
