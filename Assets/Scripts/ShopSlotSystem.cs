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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
