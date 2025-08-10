using UnityEngine;

public class WShopUIInteraction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject shopCanvasUI; // Reference to the shop UI canvas

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
