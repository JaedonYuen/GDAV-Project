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
    public List<string> modTypes; // Types of modifiers this can apply to, e.g., "speed", "damage", "health"
    public float speedModifier = 1f;
    public float damageModifier = 1f;
    public float healthModifier = 1f;

    public float enemySpeedModifier = 1f;
    public float enemyDamageModifier = 1f;
    public float enemyHealthModifier = 1f;

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

        foreach (var modifier in playerModifiers)
        {
            if (modifier.modTypes.Contains(modtype))
            {
                switch (modtype)
                {
                    case "speed":
                        totalValue *= modifier.speedModifier;
                        break;
                    case "damage":
                        totalValue *= modifier.damageModifier;
                        break;
                    case "health":
                        totalValue *= modifier.healthModifier;
                        break;
                    case "enemySpeed":
                        totalValue *= modifier.enemySpeedModifier;
                        break;
                    case "enemyDamage":
                        totalValue *= modifier.enemyDamageModifier;
                        break;
                    case "enemyHealth":
                        totalValue *= modifier.enemyHealthModifier;
                        break;
                    default:
                        Debug.LogWarning($"Unknown mod type: {modtype}");
                        break;
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
