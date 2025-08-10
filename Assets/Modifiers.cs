using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

[Serializable]
public class Modifier
{
    public string name;
    public string description;
    public Sprite icon;
    public int cost;
    public List<ModValue> modTypes = new List<ModValue>(); // List of modifier types and their values
    public int onlyAppearsInWaveAndAbove = 0; // This modifier only appears in waves above this number, 0 means it can appear in any wave
}

[Serializable]
public class ModValue
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


    void Update()
    {
        //updat mod

    }

    public float GetModValuesForAllTypesEquiped(string modtype)
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

        return totalValue;
                
    }

    public void EquipModifier(string modifierName)
    {
        Modifier modifier = playerModifiers.Find(m => m.name == modifierName);
        if (modifier != null)
        {
            EquipModifier(modifier);
        }
        else
        {
            Debug.LogWarning($"Modifier {modifierName} not found.");
        }
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
