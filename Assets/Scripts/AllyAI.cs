using UnityEngine;
using System.Collections;
public class AllyAI : MonoBehaviour
{
    public float speed = 2f; // Speed of the Gonk enemy
    public float detectionRange = 5f; // Range within which the Gonk enemy detects the player
    public float enemyThinkInterval = 0.5f; // How often the enemy checks for the player
    public float enemyFireInterval = 0.5f; // How often the enemy fires at the player
    public float enemyFireRate = 1f; // Rate at which the enemy fires
    public float distanceToPlayer = 3f; // Distance to maintain from the player
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
        Debug.Log("Ally AI started.");
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
            Debug.Log("true");
            Collider2D player = CheckForPlayer();
            if (player != null)
            {
                // Move towards the player

                float distance = Vector2.Distance(transform.position, player.transform.position);
                Vector2 direction = Vector2.right; // Default direction
                if (distance > distanceToPlayer)
                {
                    direction = (player.transform.position - transform.position).normalized;
                    enemyRigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
                    this.player = player.gameObject;
                }
                else
                {
                    //kick back to break
                    direction = (transform.position - player.transform.position).normalized;
                    enemyRigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
                    this.player = player.gameObject;
                }

                //flip the enemy sprite based on movement direction
                if (direction.x > 0)
                {
                    SpriteRenderer enemySprite = GetComponent<SpriteRenderer>();
                    if (enemySprite != null)
                    {
                        enemySprite.flipX = false; // Face right
                    }
                }
                else if (direction.x < 0)
                {
                    SpriteRenderer enemySprite = GetComponent<SpriteRenderer>();
                    if (enemySprite != null)
                    {
                        enemySprite.flipX = true; // Face left
                    }
                }
            }
            else
            {
                // If the player is not detected, stop moving
                this.player = null; // Reset player reference
            }
            yield return new WaitForSeconds(enemyThinkInterval);
        }
    }


    Collider2D CheckForPlayer()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
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
