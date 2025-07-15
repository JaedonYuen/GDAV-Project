using System.Collections;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class GonkEnemyAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // this script is purely a prototype 
    //name is a ref to cyberpunk2077

    public float speed = 2f; // Speed of the Gonk enemy
    public float detectionRange = 5f; // Range within which the Gonk enemy detects the player

    public float enemyThinkInterval = 0.5f; // How often the enemy checks for the player

    public Rigidbody2D enemyRigidbody; 

    void Start()
    {
        if (enemyRigidbody == null)
        {
            enemyRigidbody = GetComponent<Rigidbody2D>();
        }
        Debug.Log("Gonk Enemy AI started.");
        StartCoroutine(enemyLoop());
    }

    IEnumerator enemyLoop()
    {
        while (true)
        {
            Debug.Log("true");
            Collider2D player = CheckForPlayer();
            if (player != null)
            {
                // Move towards the player
                
                Vector2 direction = (player.transform.position - transform.position).normalized;
                enemyRigidbody.MovePosition(enemyRigidbody.position + direction * speed * Time.fixedDeltaTime);
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
                Vector2 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange);
                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    // Player is detected, implement behavior here
                    //Debug.Log("Player detected by Gonk enemy!");
                    // Example: Move towards the player
                    return hitCollider;
                }
            }
        }
        return null;
    }
}
