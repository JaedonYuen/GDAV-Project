using UnityEngine;

public class ShopSlotSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Premise, when an item doesnt get parented to the slot anymore, consider it as purcased. 
    public GameObject itemPrefab; // Prefab of the item to be purchased
    public Transform slotParent; // Parent transform where items will be placed
    public int price = 100; // Price of the item
    public bool isPurchased = false; // Flag to check if the item is purchased
    void Start()
    {
        // Initialize the shop slot system
        if (itemPrefab != null && slotParent != null)
        {
            GameObject itemInstance = Instantiate(itemPrefab, slotParent);
            itemInstance.SetActive(false); // Initially hide the item
            Debug.Log("Shop slot initialized with item: " + itemPrefab.name);
        }
        else
        {
            Debug.LogError("Item prefab or slot parent is not set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the item is purchased
        if (isPurchased && slotParent.childCount > 0)
        {
            // If the item is purchased, deactivate the item in the slot
            Transform itemInSlot = slotParent.GetChild(0);
            if (itemInSlot != null)
            {
                itemInSlot.gameObject.SetActive(false);
                Debug.Log("Item " + itemInSlot.name + " has been purchased and deactivated.");
            }
        }
    }
}
