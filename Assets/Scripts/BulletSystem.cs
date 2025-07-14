using UnityEngine;

public class BulletSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float damage = 10f; // Damage dealt by the bullet
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to the enemy
            collision.gameObject.GetComponent<EnemyHealthSystem>().TakeDamage(damage);
        }
        Destroy(gameObject); // Destroy the bullet on collision
    }
}
