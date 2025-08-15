
using UnityEngine;
using UnityEngine.InputSystem;
public class HandSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Points the hand to the player's position.
    // Also handles grabbing, firing and droping of weapons.
    public Transform arm;
    public Transform hand;
    public GameObject itemInhand;
    public float interactionRange = 1f; // Range for interaction
    void Start()
    {
        Debug.Log("Hand System Initialized");
    }

    // Update is called once per frame
    void Update()
    {
        // point player's arm to mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        arm.transform.right = mousePosition - (Vector2)arm.transform.position;
    }




    public void OnInteract(InputValue inputValue)
    {
        Debug.Log("interacted");

        if (inputValue.isPressed)
        {
            if (itemInhand != null)
            {
                // drop
                Debug.Log("Dropped item: " + itemInhand.name);
                itemInhand.transform.position = hand.position;
                itemInhand.transform.SetParent(null);
                itemInhand = null;



            }
            else
            {

                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(hand.position, interactionRange);
                Debug.Log(hitColliders.Length + " items found in range");
                foreach (var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Item"))
                    {
                        // Interaction logic goes here later
                        Debug.Log("Interacted with item: " + hitCollider.name);
                        itemInhand = hitCollider.gameObject;

                        itemInhand.transform.SetParent(hand);

                        itemInhand.transform.localPosition = Vector3.zero;
                        itemInhand.transform.localRotation = Quaternion.identity; // Reset rotation to avoid unexpected rotations




                        return;
                    }
                }
            }
        }
    }



    public void OnAttack(InputValue inputValue)
    {
        Debug.Log("Attack action triggered");
        if (itemInhand != null)
        {
            GunSystem gun = itemInhand.GetComponent<GunSystem>();
            if (gun != null)
            {
                gun.Fire(inputValue.isPressed);
            }
        }
        // Implement firing logic here
    }

    public void OnReload()
    {
        Debug.Log("Reload action triggered");
        if (itemInhand != null)
        {
            GunSystem gun = itemInhand.GetComponent<GunSystem>();
            if (gun != null)
            {
                gun.Reload();
            }
        }
        // Implement reloading logic here
    }
    
    public void OnEnter()
    {
        
        GameSystem gameSystem = FindFirstObjectByType<GameSystem>();
        if (gameSystem != null)
        {
            if (itemInhand != null) // Make sure that the player has a weapon
            {
                gameSystem.StartWave();
            }
            else
            {
                Debug.LogWarning("You cant just fight without a weapon!");
            }
        }

    }
}
