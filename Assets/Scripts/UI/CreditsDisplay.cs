using UnityEngine;
using TMPro;
public class CreditsDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Displays the credits of the player
    public PlayerCurrencyManager playerCurrencyManager; // Reference to the player's currency manager
    public TextMeshProUGUI creditsText; // Text component to display credits

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCurrencyManager != null && creditsText != null)
        {
            creditsText.text = $"{playerCurrencyManager.credits} C";
        }
    }
}
