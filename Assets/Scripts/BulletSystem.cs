using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage = 10f; 
    public GameObject impactEffect; // Assign a prefab for the impact effect in the inspector
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealthSystem enemyHealth = collision.gameObject.GetComponent<EnemyHealthSystem>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Bullet hit enemy: " + collision.gameObject.name);
            }
            else
            {
                Debug.LogWarning("Enemy does not have an EnemyHealthSystem component.");
            }
        }
        GameObject newImpactFX = Instantiate(impactEffect, transform.position, Quaternion.identity);
        Destroy(newImpactFX, 2f); // Destroy the impact effect after 2 seconds
        Destroy(gameObject);
    }
}
