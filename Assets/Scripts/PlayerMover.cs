using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //top down movement, uses the new Unity Input System
    public Rigidbody2D playerRigidbody;
    public float speed = 5f;
    public Transform arm;
    private Vector2 movementInput;

    void Start()
    {

        if (playerRigidbody == null)
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            if (playerRigidbody == null)
            {
                Debug.LogError("Rigidbody2D component is not assigned and could not be found on the GameObject.");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the player based on the input received
        Move(movementInput);
        // point player to mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        arm.transform.right = mousePosition - (Vector2)arm.transform.position;

    }

    // This method is called by Unity's Input System when using "Send Messages"
    // The method name must match your Input Action name (e.g., "Move", "Movement", etc.)
    public void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }

    public void Move(Vector2 direction)
    {
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = direction * speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D component is not assigned.");
        }
    }
    
    
}


