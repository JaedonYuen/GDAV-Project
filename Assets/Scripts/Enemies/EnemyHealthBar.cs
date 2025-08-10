using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Slider healthBar; // Reference to the UI Slider component
    public GameObject enemy; // Reference to the enemy GameObject
    private EnemyHealthSystem enemyHealthSystem; // Reference to the enemy's health system
    void Start()
    {
        enemyHealthSystem = enemy.GetComponent<EnemyHealthSystem>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealthSystem != null)
        {
            healthBar.value = enemyHealthSystem.GetHealthPercentage();
        }
    }
}
