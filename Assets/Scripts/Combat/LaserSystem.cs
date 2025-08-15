using System;
using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // A system that mimiks the laser of a real laser pointer for guns. This is used for the super accurate weapon the sniper, to ensure players can line up their shots right, since the entire premise of that gun is to make sure that they can chain shots to kill multiple enemies in one bullet
    public LineRenderer lineRenderer;
    public Transform laserOrigin;
    public float laserLength = 10000f; // Maximum length of the laser
    public LayerMask hitLayers = -1; // What layers the laser can hit
    public bool isEnabled = true; // Toggle laser on/off

    void Start()
    {
        // Auto-find components if not assigned
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
        
        if (laserOrigin == null)
            laserOrigin = transform;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
        }
        else
        {
            Debug.LogError("LaserSystem: LineRenderer component not found! Please assign one in the inspector or add a LineRenderer component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled && lineRenderer != null)
        {
            UpdateLaser();
        }
    }

    void UpdateLaser()
    {
        if (laserOrigin == null) return;

        // Raycast, like a real laser would.
        RaycastHit2D hit = Physics2D.Raycast(laserOrigin.position, laserOrigin.right, laserLength, hitLayers);
        
        Vector3 endPosition;
        if (hit.collider != null)
        {
            endPosition = hit.point; // Set end position to the hit point
            //Debug.Log($"Laser hit: {hit.collider.name} at distance: {hit.distance}");
        }
        else
        {
            endPosition = laserOrigin.position + laserOrigin.right * laserLength; // Default end position
        }
        // actually render that laser
        lineRenderer.SetPosition(0, laserOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    // Method to toggle laser on/off
    public void SetLaserEnabled(bool enabled)
    {
        isEnabled = enabled;
        if (lineRenderer != null)
            lineRenderer.enabled = enabled;
    }
}
