using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class Modifier // Class of the modifier module.
{
    public string name; 
    public string description;
    public Sprite icon; // Icon representing the modifier
    public int cost; // Cost
    public List<ModValue> modTypes = new List<ModValue>(); // List of modifier types and their values
    public int onlyAppearsInWaveAndAbove = 0; // This modifier only appears in waves above this number, 0 means it can appear in any wave
}

[Serializable]
public class ModValue // A class that identifies itself as what type of value it should change, and what value to "add" to the total mod pool for that type.
{
    public string modType; // Type of modifier, e.g., "speed", "damage", "health"
    public float value; // Value of the modifier, e.g., 1.2 for 20% increase
}


public class Modifiers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public List<Modifier> playerModifiers = new List<Modifier>();
    public List<Modifier> modifiers = new List<Modifier>();

    private int maxModifiers = 4; // Maximum number of modifiers that can be equipped

    public float GetModValuesForAllTypesEquiped(string modtype) // Loop through every mod that the player equips and then loop though what it affects. If that effect is the same type, add it to the total.
    {
        float totalValue = 1f;
        foreach (Modifier modifier in playerModifiers)
            {
                foreach (ModValue modValue in modifier.modTypes)
                {
                    if (modValue.modType == modtype)
                    {
                        totalValue += modValue.value; // Multiply the values for the same mod type
                        Debug.Log($"Modifier: {modifier.name}, Type: {modValue.modType}, Value: {modValue.value}");
                    }
                }
            }

        return MathF.Max(totalValue, 0.1f); // Ensure the value still has some value in it
                
    }

    public void EquipModifier(Modifier modifier)
    {
        //replace the first empty slot with the modifier
        if (playerModifiers.Count < maxModifiers)
        {
            playerModifiers.Add(modifier);
            Debug.Log($"Equipped Modifier: {modifier.name}");
        }
        else
        {
            Debug.LogWarning("Cannot equip more modifiers, maximum limit reached.");
        }

    }
    public void UnequipModifier(string modifierName)
    {
        Modifier modifier = playerModifiers.Find(m => m.name == modifierName);
        if (modifier != null)
        {
            playerModifiers.Remove(modifier);
            Debug.Log($"Unequipped Modifier: {modifier.name}");
        }
        else
        {
            Debug.LogWarning($"Modifier {modifierName} not found in equipped modifiers.");
        }
    }

    public int CheckAvailableModifierSlots()
    {
        return maxModifiers - playerModifiers.Count;
    }
}
