using UnityEngine;

public class PlayerCurrencyManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int initialCredits = 100; // Initial credits for the player
    private int _credits;
    public int credits
    {
        get => _credits;
        set
        {
            _credits = value;
            
        }
    }

    public bool CanAfford(int cost)
    {
        return _credits >= cost;
    }

    public void AddCredits(int amount)
    {
        _credits += amount;
        
    }

    public void SpendCredits(int amount)
    {
        if (CanAfford(amount))
        {
            _credits -= amount;
            
        }
        else
        {
            Debug.LogWarning("Not enough credits to spend.");
        }
    }

    void Start()
    {
        credits = initialCredits; // Initialize credits
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
