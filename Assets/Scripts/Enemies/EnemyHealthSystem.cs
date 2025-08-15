using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Health manager for the enemy, when it dies, it rewards the player with credits :D
    public float maxHealth = 100f;
    public int rewardOnDeath = 50; // Reward given to the player when the enemy dies

    private float _currentHealth = 0f;
    public float currentHealth 
    {
        get => _currentHealth;
        private set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
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
        Debug.Log($"Enemy took {damage} damage, current health: {_currentHealth}");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy died");
        // Add death logic here, such as playing an animation or destroying the object
        PlayerCurrencyManager playerCurrencyManager = FindFirstObjectByType<PlayerCurrencyManager>(); // find the currency manager
        
        if (playerCurrencyManager != null)
        {
            playerCurrencyManager.AddCredits(rewardOnDeath); //reward
            Debug.Log($"Player rewarded with {rewardOnDeath} credits.");
        }
        else
        {
            Debug.LogWarning("PlayerCurrencyManager not found in the scene.");
        }

        Destroy(gameObject);
    }
    
    public float GetHealthPercentage()
    {
        return _currentHealth / maxHealth;
    }
}
