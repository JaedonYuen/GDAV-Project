using System;
using UnityEngine;

public class LaserSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LineRenderer lineRenderer;
    public Transform laserOrigin;
    public float laserLength = 10000f; // Maximum length of the laser

    void Start()
    {
        lineRenderer.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaser();
    }

    void UpdateLaser()
    {

        
        RaycastHit hit;
        Vector3 endPosition = laserOrigin.position + laserOrigin.forward * laserLength; // Default end position
        
        if (Physics.Raycast(laserOrigin.position, laserOrigin.forward, out hit, laserLength))
        {
            endPosition = hit.point; // Set end position to the hit point
            
        }
        lineRenderer.SetPosition(0, laserOrigin.position);
        lineRenderer.SetPosition(1, endPosition);
    }
}
