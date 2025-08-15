using UnityEngine;
using UnityEngine.InputSystem;
public class CursorSystem : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null)
        {
            // Follow the player's cursor.
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
            transform.position = new Vector3(worldPosition.x, worldPosition.y, transform.position.z);
        }
    }
}
