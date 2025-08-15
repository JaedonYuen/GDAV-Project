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

    // AI explaination:
    // AI will first check for the player within a certain range.
    // If the player is detected, the AI will move towards the player and attempt to attack.
    // Attacking is done by mimicking button presses and sending that to the gun system. I have reused the gun system because its more efficient than creating a new system for enemies.
    // If an enemy feels like its too close to a fellow enemy, it will back off.
    // If an enemy feels like its too close to the player, it will also back off, to avoid getting shot (well uhh only effective for sniper enemies since they always stay very far from the player.)
    // If the player is not detected, the AI will stand still. This behavior is like this because i have made it so that the enemies always know where the player is via a large detection range for more engaging gameplay. 

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
                if (distance > distanceToPlayer) // Check to see if its too near to a player
                {
                    // If not, procced.
                    enemyRigidbody.linearVelocity = direction * speed * speedMod;
                    this.player = player.gameObject;
                }
                else
                {
                    // Move back
                    enemyRigidbody.linearVelocity = -direction * speed * speedMod;
                    this.player = player.gameObject;
                }
            }
            else
            {
                // If the player is not detected, stop moving
                // Explained why this is the case at the start of the script :)
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


    Collider2D CheckForPlayer() // Check for players via a circle overlap, and then finding the first player instance (there should only be one.)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                
                return hitCollider; 
            }
        }
        return null;
    }

    Collider2D CheckForFellowEnemies() // Check to see if its close to fellow enemies via a circle overlap, if it is, return the closest one.
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, detectionRange);
        Collider2D closestEnemy = null;
        float closestDistance = float.MaxValue;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider;
                }
            }
        }
        if (closestEnemy != null && closestDistance < detectionRange)
        {
            return closestEnemy;
        }
        return null;
    }

}
