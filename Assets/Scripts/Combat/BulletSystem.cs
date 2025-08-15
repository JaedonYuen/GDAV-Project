using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage = 10f; // Damage dealt by the bullet
    public GameObject hitEffectPrefab; // Prefab for hit effect
    public string whoToDamage = "Enemy"; // Tag of the object to damage
    public int bulletHealth = 1; // how many hits the bullet can take before being destroyed, if the number is higher than one, it can act as a piercing bullet, allowing us to hit multiple enemies in one go. 
    private Vector2 linearVelocity = Vector2.zero; // this variable stores the bullet's current linear velocity, its meant for piercing weapons like the sniper.

    // Update is called once per frame
    void Update()
    {
        linearVelocity = GetComponent<Rigidbody2D>().linearVelocity;
    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        //should check if the bullet hits an enemy
        // check if it hit an object in the wall
        if (collision.gameObject.CompareTag(whoToDamage))
        {
            // Apply damage to the enemy
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<PlayerHealthSystem>();
            if (enemyHealth != null)
            {
                Modifiers playerModifiers = FindFirstObjectByType<Modifiers>(); // grab all player damage mods
                float finalDamage = playerModifiers != null ? damage * playerModifiers.GetModValuesForAllTypesEquiped("damage") : damage;
                enemyHealth.TakeDamage(finalDamage);
            }
            else if (playerHealth != null)
            {
                Modifiers playerModifiers = FindFirstObjectByType<Modifiers>(); // grab all enemy damage mods
                float finalDamage = playerModifiers != null ? damage * playerModifiers.GetModValuesForAllTypesEquiped("enemyDamage") : damage;
                playerHealth.TakeDamage(finalDamage);
            }
            else
            {
                Debug.LogWarning("Couldnt find health component on " + collision.gameObject.name);
            }
            // Decrease bullet health
            bulletHealth--;
            if (bulletHealth <= 0)
            {
                
                Destroy(gameObject); // Destroy the bullet if it has no health left
            }
            else
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(linearVelocity, ForceMode2D.Impulse); // Apply the current velocity to the bullet
                }
            }
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // Destroy the hit effect after 2 seconds
        }
        else
        {
            // If it hits something else, just destroy the bullet
            GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(hitEffect, 2f); // Destroy the hit effect after 2 seconds
            Destroy(gameObject);
        }
    }
}


