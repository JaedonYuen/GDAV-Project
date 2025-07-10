using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth = 100f;

    // Method 1: Public property with private setter (recommended for Unity Inspector)
    [SerializeField] private float _currentHealth = 0f;
    public float currentHealth => _currentHealth; // Read-only property


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
        Destroy(gameObject);
    }
}
