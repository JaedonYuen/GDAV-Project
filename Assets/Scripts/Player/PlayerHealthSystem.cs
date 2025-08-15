using UnityEngine;
using System.Collections;
public class PlayerHealthSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float maxHealth = 100f; // Maximum health of the player
    public float currentHealth;
    public GameObject gameOverScreen; // Reference to the game over screen UI
    private float _currentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
        }
    }
    public float healthRegenRate = .5f; // time in seconds to regenerate health
    public float healthRegenStep = 5f; // Health regeneration step amount

    void Start()
    {
        _currentHealth = maxHealth;
        StartCoroutine(RegenerateHealth());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator RegenerateHealth()
    {
        while (true)
        {
            Modifiers playerModifiers = FindFirstObjectByType<Modifiers>();
            float healthRegenModifier = playerModifiers != null ? playerModifiers.GetModValuesForAllTypesEquiped("healthRegen") : 1f;
            
            if (_currentHealth < maxHealth)
            {
                Heal(healthRegenStep * healthRegenModifier);
            }
            
            yield return new WaitForSeconds(healthRegenRate);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        //Debug.Log("Player took damage: " + damage + ". Current health: " + _currentHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
    }

    void Die()
    {
        Debug.Log("Player has died.");
        gameOverScreen.SetActive(true);
        Animator animator = gameOverScreen.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("GameOver");
        }
        StopAllCoroutines(); // Stop health regeneration when the player dies
                             // pause
        //Time.timeScale = 0f; // Pause the game
    }
}
