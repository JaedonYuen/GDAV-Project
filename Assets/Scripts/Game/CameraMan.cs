using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform player;
    public Vector3 offset;
    public Vector3 _offset;

    public float lookOffset = 10f; // How much the camera should look at the mouse position

    public float smoothSpeed = 0.125f; 

    public Camera mainCamera;

    void Start()
    {
        // Initialize the offset to the base offset
        _offset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetOffset = offset; // Default to base offset
        
        // move the camera offset based on the mouse to show more of the scene where the player is "looking"
        if (Mouse.current != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            Vector2 mouseOffset = (mousePosition - screenCenter) / Screen.width; // Normalize to screen size
            
            // Calculate target offset with mouse input
            targetOffset = offset + new Vector3(mouseOffset.x * lookOffset, mouseOffset.y * lookOffset, 0);
        }
        
        // Smoothly interpolate the offset
        _offset = Vector3.Lerp(_offset, targetOffset, smoothSpeed * Time.deltaTime * 10f);
        
        Vector3 desiredPosition = player.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
