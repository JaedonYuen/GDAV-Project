using UnityEngine;
public class CursorSystem : MonoBehaviour
{
    public Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set this to the distance from the camera to the object
        transform.position = mainCamera.ScreenToWorldPoint(mousePosition);
    }
}
