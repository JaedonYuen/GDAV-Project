using System.Collections;
using UnityEngine;


public class BasicEnemyAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public float speed = 2f; // Speed of the Gonk enemy
    public float detectionRange = 5f; // Range within which the Gonk enemy detects the player
    public float enemyThinkInterval = 0.5f; // How often the enemy checks for the player
    public float enemyFireInterval = 0.5f; // How often the enemy fires at the player
    public float enemyFireRate = 1f; // Rate at which the enemy fires
    public float distanceToPlayer = 5f; // Distance to maintain from the player
    public float distanceToFellowEnemy = 0.5f; // Distance to maintain from fellow enemies
    public Rigidbody2D enemyRigidbody;
    public GunSystem enemyWeaponSystem; // Reference to the enemy's weapon system
    public Transform enemyArm;
    

    private GameObject player; // Reference to the player GameObject


    void Start()
    {
        if (enemyRigidbody == null)
        {
            enemyRigidbody = GetComponent<Rigidbody2D>();
        }
        //Debug.Log("Gonk Enemy AI started.");
        StartCoroutine(enemyFireLoop());
        StartCoroutine(enemyMovementLoop());
    }

    void Update()
    {
        // This can be used for additional logic if needed
        if (player != null)
        {
            // Point the arm towards the player if detected
            Vector2 PlayerPosition = player != null ? player.transform.position : Vector2.zero;
            Vector2 direction = (PlayerPosition - (Vector2)enemyArm.position).normalized;
            enemyArm.right = direction;

        }
        else
        {
            // If the player is not detected, reset the arm direction
            enemyArm.right = Vector2.right; // Reset to default direction
        }
        
    }
    IEnumerator enemyFireLoop()
    {
        while (true)
        {
            if (player != null)
            {
                enemyWeaponSystem.Fire(true);
                yield return new WaitForSeconds(enemyFireInterval); // Wait before checking again
                enemyWeaponSystem.Fire(false);
            }
            yield return new WaitForSeconds(enemyFireRate); // Wait for the fire rate before firing again
        }
    }
    IEnumerator enemyMovementLoop()
    {
        while (true)
        {
            //Debug.Log("true");
            Collider2D player = CheckForPlayer();
            if (player != null)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;
                // Move towards the player
                float distance = Vector2.Distance(transform.position, player.transform.position);
                //Debug.Log(distance);
                float speedMod = GetComponent<Modifiers>()?.GetModValuesForAllTypesEquiped("enemySpeed") ?? 1f;
                if (distance > distanceToPlayer)
                {
                    enemyRigidbody.linearVelocity = direction * speed * speedMod;
                    this.player = player.gameObject;
                }
                else
                {
                    //kick back to break

                    enemyRigidbody.linearVelocity = -direction * speed * speedMod;
                    this.player = player.gameObject;
                }
            }
            else
            {
                // If the player is not detected, stop moving
                this.player = null; // Reset player reference
            }
            // Check for fellow enemies
            Collider2D fellowEnemy = CheckForFellowEnemies();
            if (fellowEnemy != null)
            {
                float distanceToDetectedFellowEnemies = Vector2.Distance(transform.position, fellowEnemy.transform.position);
                if (distanceToDetectedFellowEnemies < distanceToFellowEnemy)
                {
                    // If too close to a fellow enemy, move away
                    Vector2 direction = (transform.position - fellowEnemy.transform.position).normalized;
                    enemyRigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
                }


            }
            yield return new WaitForSeconds(enemyThinkInterval);
        }
    }


    Collider2D CheckForPlayer()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                // check to see if we can see the player
                // Vector2 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
                // RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                // if (hit.collider != null && hit.collider.CompareTag("Player"))
                // {
                //     // Player is detected, implement behavior here
                //     //Debug.Log("Player detected by Gonk enemy!");
                //     // Example: Move towards the player
                //     return hitCollider;
                // }
                return hitCollider; // Return the player collider if detected
            }
        }
        return null;
    }

    Collider2D CheckForFellowEnemies()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                return hitCollider; // Return the player collider if detected
            }
        }
        return null;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<PlayerHealthSystem>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Example damage value
            }
        }
    }
}
