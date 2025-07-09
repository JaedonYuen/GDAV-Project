using UnityEngine;
using UnityEngine.InputSystem;
public class HandSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    public void OnJump()
    {
        Debug.Log("Jump action triggered");
    }

    public void OnAttack()
    {
        Debug.Log("Attack action triggered");
        if (itemInhand != null)
        {
            GunSystem gun = itemInhand.GetComponent<GunSystem>();
            if (gun != null)
            {
                gun.Fire();
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
}
