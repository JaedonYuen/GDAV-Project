using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage = 10f; // Damage dealt by the bullet
    public GameObject hitEffectPrefab; // Prefab for hit effect

    public string whoToDamage = "Enemy"; // Tag of the object to damage

    public bool homing = false; // Whether the bullet is homing towards a target

    public int bulletHealth = 1; // how many hits the bullet can take before being destroyed

    private Vector2 linearVelocity = Vector2.zero; 
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        linearVelocity = GetComponent<Rigidbody2D>().linearVelocity;

        if (homing)
            {
                // Find the nearest target to home in on
                GameObject target = FindNearestTarget();
                if (target != null)
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
                        Vector2 direction = (target.transform.position - transform.position).normalized;
                        //rb.linearVelocity = Vector2.zero; // Reset velocity to avoid accumulating forces
                        rb.AddForce(direction * (distanceToTarget / 10), ForceMode2D.Impulse); // Adjust the force multiplier as needed

                    }
                }
            }
    }

    GameObject FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(whoToDamage);
        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject target in targets)
        {
            float distance = Vector2.Distance(transform.position, target.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }

        return nearestTarget;
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
                enemyHealth.TakeDamage(damage);
            }
            else if (playerHealth != null)
            {
                Modifiers playerModifiers = FindFirstObjectByType<Modifiers>();
                float finalDamage = playerModifiers != null ? damage * playerModifiers.GetModValuesForAllTypesEquiped("damage") : damage;
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


