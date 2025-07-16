using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth;
    private float _currentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }

    void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log("Player took damage: " + damage + ". Current health: " + _currentHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        Debug.Log("Player healed: " + amount + ". Current health: " + _currentHealth);
    }

    void Die()
    {
        Debug.Log("Player has died.");
        Application.Quit(); // Exit the game or handle player death appropriately
        // Handle player death (e.g., respawn, game over)
        // This could involve disabling the player, playing a death animation, etc.
    }
}
