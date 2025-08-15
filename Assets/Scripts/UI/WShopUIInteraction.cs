using UnityEngine;

public class WShopUIInteraction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Toggles the shop UI on or off every time the player either leaves or exits the shop

    public GameObject shopCanvasUI; // Reference to the shop UI canvas
    
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
