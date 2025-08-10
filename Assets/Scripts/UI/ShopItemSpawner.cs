using UnityEngine;

public class ShopSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //Premise, when an item doesnt get parented to the slot anymore, consider it as purcased. 
    public Transform spawnLocation; // Location where items will be spawned
    void Start()
    {
        // Initialize the shop slot system

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the item is purchased

    }
    
    public void SpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab != null && spawnLocation != null)
        {
            GameObject item = Instantiate(itemPrefab, spawnLocation.position, Quaternion.identity);
            item.transform.SetParent(spawnLocation); // Set the parent to the spawn location
            Debug.Log("Item spawned: " + item.name);
        }
        else
        {
            Debug.LogWarning("Item prefab or spawn location is not set.");
        }
    }
}
