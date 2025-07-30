using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage = 10f; // Damage dealt by the bullet
    public GameObject hitEffectPrefab; // Prefab for hit effect

    public string whoToDamage = "Enemy"; // Tag of the object to damage

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //should check if the bullet hits an enemy
        if (collision.gameObject.CompareTag(whoToDamage))
        {
            // Apply damage to the enemy
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<PlayerHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            else if (playerHealth != null)
            {
                PlayerModifiers playerModifiers = FindFirstObjectByType<PlayerModifiers>();
                float finalDamage = 0f;
                if (playerModifiers != null)
                {
                    // Apply damage reduction if playerModifiers is available
                    finalDamage = damage * playerModifiers.currentDamageModifier;
                }
                playerHealth.TakeDamage(finalDamage);
            }
            else
            {
                Debug.LogWarning("Couldnt find health component on " + collision.gameObject.name);
            }
        }
        Destroy(gameObject); // Destroy the bullet on collision
        GameObject hitEffect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        Destroy(hitEffect, 2f); // Destroy the hit effect after 2 seconds
    }
}
