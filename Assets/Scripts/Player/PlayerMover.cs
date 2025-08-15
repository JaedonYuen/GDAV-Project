using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //top down movement, uses the new Unity Input System
    public Rigidbody2D playerRigidbody;
    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f; // Duration of the dash in seconds
    public float dashCooldown = 1f;
    public Transform arm;
    public SpriteRenderer armSprite;
    //public PlayerModifiers playerModifiers; // Reference to PlayerModifiers script
    public Modifiers playerModifiers;
    private Vector2 movementInput;

    private SpriteRenderer spriteRenderer;

    private float modifiedSpeed;

    private bool isDashing = false;
    private bool canDash = true; // Flag to check if the player can dash

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component is not found on the GameObject.");
        }

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
    void FixedUpdate()
    {
        // Apply any modifier
        float speedMod = playerModifiers.GetModValuesForAllTypesEquiped("speed");
        modifiedSpeed = speed * speedMod; // Apply the speed modifier

        // Move the player based on the input received
        //only move if not dashing
        if (!isDashing && playerRigidbody != null)
        {
            Move(movementInput);
        }
        // Flip the player sprite based on movement direction
        if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false; // Face right
            armSprite.sortingOrder = 1; // Ensure arm is in front
        }
        else if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true; // Face left
            armSprite.sortingOrder = -1; // Ensure arm is behind
        }
        // point player to mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        arm.transform.right = mousePosition - (Vector2)arm.transform.position;

    }

    IEnumerator Dash()
    {
        if (playerRigidbody != null && canDash && !isDashing)
        {
            isDashing = true;
            canDash = false; // Prevent further dashes until cooldown is over
            Vector2 dashDirection = movementInput.normalized; // Get the direction of the dash
            playerRigidbody.AddForce(dashDirection * dashSpeed, ForceMode2D.Impulse);
            yield return new WaitForSeconds(dashDuration);
            isDashing = false;
            yield return new WaitForSeconds(dashCooldown); // Wait for the cooldown period
            canDash = true; // Allow dashing again after cooldown
        }
    }

    // This method is called by Unity's Input System when using "Send Messages"
    // The method name must match your Input Action name (e.g., "Move", "Movement", etc.)
    public void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();
    }
    

    public void OnJump() //using the default jump key lel
    {
        //Debug.Log("Jump action triggered");
        StartCoroutine(Dash());
    }

    public void Move(Vector2 direction)
    {
        //Debug.Log($"Moving player with direction: {direction} and speed: {modifiedSpeed}");
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = direction * modifiedSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D component is not assigned.");
        }
    }

    



}


