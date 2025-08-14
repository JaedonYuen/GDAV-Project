using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ModifierShop : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Modifiers modifiers; // Reference to the Modifiers script
    public GameSystem gameSystem; // Reference to the GameSystem script
    public GameObject modifierButtonPrefab; // Prefab for the modifier button
    public Transform modifierButtonContainer; // Container to hold the modifier buttons
    public Transform equippedModifiersContainer; // Container to display equipped modifiers
    public PlayerCurrencyManager playerCurrencyManager; // Reference to the PlayerCurrencyManager script
    public TextMeshProUGUI descriptionText; // Text to display the player's currency
    void Start()
    {
        PopulateModifierShop();
        DisplayEquippedModifiers();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PopulateModifierShop()
    {
        // Clear existing buttons
        foreach (Transform child in modifierButtonContainer)
        {
            Destroy(child.gameObject);
        }
        // Get 4 random modifiers from the Modifiers script
        if (modifiers == null || modifiers.modifiers.Count == 0)
        {
            Debug.LogWarning("No modifiers available to populate the shop.");
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            if (modifiers.modifiers.Count == 0) break; // Exit if no modifiers left

            // Select a random modifier, make sure that modifier only appears in waves above the current wave
            Modifier selectedModifier = modifiers.modifiers[Random.Range(0, modifiers.modifiers.Count)];
            while (selectedModifier.onlyAppearsInWaveAndAbove > gameSystem.currentWaveLevel)
            {
                selectedModifier = modifiers.modifiers[Random.Range(0, modifiers.modifiers.Count)];
            }
            
            // Create a button for the modifier
                GameObject button = Instantiate(modifierButtonPrefab, modifierButtonContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{selectedModifier.name} - {selectedModifier.cost} C";
            button.GetComponent<Button>().onClick.AddListener(() => OnShopModifierButtonClicked(selectedModifier, button));
            // Add hover effect
            button.GetComponent<EventTrigger>().triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter,
                callback = new EventTrigger.TriggerEvent()
            });
            button.GetComponent<EventTrigger>().triggers[0].callback.AddListener((eventData) => { OnShopModifierButtonHover(selectedModifier, button); });

            GameObject icon = button.transform.Find("Icon").gameObject;
            if (icon != null)
            {
                Image iconImage = icon.GetComponent<Image>();
                if (iconImage != null && selectedModifier.icon != null)
                {
                    iconImage.sprite = selectedModifier.icon;
                }
            }

            
           
        }
    }

    void OnShopModifierButtonHover(Modifier modifier, GameObject button)
    {
        // Display the modifier's description
        if (descriptionText != null)
        {
            descriptionText.text = $"<b>{modifier.name}</b>: {modifier.description}";
        }
        else
        {
            Debug.LogWarning("Description Text is not assigned in the inspector.");
        }
    }

    void DisplayEquippedModifiers()
    {
        // Clear existing equipped modifiers
        foreach (Transform child in equippedModifiersContainer)
        {
            Destroy(child.gameObject);
        }
        // Display currently equipped modifiers
        foreach (Modifier modifier in modifiers.playerModifiers)
        {
            if (modifier.name == null || modifier.name == "") continue;
            GameObject button = Instantiate(modifierButtonPrefab, equippedModifiersContainer);
            button.GetComponentInChildren<TextMeshProUGUI>().text = modifier.name;
            button.GetComponent<Button>().onClick.AddListener(() => OnPlayerModifierButtonClicked(modifier, button));

            GameObject icon = button.transform.Find("Icon").gameObject;
            if (icon != null)
            {
                Image iconImage = icon.GetComponent<Image>();
                if (iconImage != null && modifier.icon != null)
                {
                    iconImage.sprite = modifier.icon;
                }
            }

            
        }
    }

    void OnShopModifierButtonClicked(Modifier modifier, GameObject button)
    {
        // Buy and equip the modifier
        Debug.Log($"Modifier {modifier.name} clicked.");
        if (playerCurrencyManager != null && playerCurrencyManager.CanAfford(modifier.cost))
        {
            if (modifiers.CheckAvailableModifierSlots() > 0)
            { 
                playerCurrencyManager.SpendCredits(modifier.cost);
                modifiers.EquipModifier(modifier);
                DisplayEquippedModifiers(); // Refresh the equipped modifiers display
                Debug.Log($"Equipped Modifier: {modifier.name}");

                Destroy(button); // Remove the button from the shop

            }
        }
        else
        {
            Debug.LogWarning("Not enough credits to equip this modifier.");
        }
    }

    void OnPlayerModifierButtonClicked(Modifier modifier, GameObject button)
    {
        // Sell the modifier back to the shop
        Debug.Log($"Modifier {modifier.name} clicked for unequipping.");
        if (playerCurrencyManager != null)
        {
            playerCurrencyManager.AddCredits((int)(modifier.cost * 0.8f)); // Refund 80% of the cost
            modifiers.UnequipModifier(modifier.name);
            DisplayEquippedModifiers(); // Refresh the equipped modifiers display
            Debug.Log($"Unequipped Modifier: {modifier.name}");

            Destroy(button); // Remove the button from the equipped modifiers display
        }
        else
        {
            Debug.LogWarning("PlayerCurrencyManager is not assigned.");
        }
    }
    
    public void RefreshShop()
    {
        PopulateModifierShop();
        DisplayEquippedModifiers();
    }
}
