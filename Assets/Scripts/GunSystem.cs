using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform barrel;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; // Time in seconds between shots
    public int bulletsPerShot = 1; // Number of bullets fired per shot
    public float spreadAngle = 0f; // Angle in degrees for bullet spread
    public int maxAmmo = 30; // Maximum ammo for the gun
    public float reloadTime = 1f; // Time in seconds to reload

    public float damage = 10f; 
    private int currentAmmo = 0;
    private bool isReloading = false;
    void Start()
    {
        currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator reloadCoroutine()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete! Current ammo: " + currentAmmo);
    }
    public void Fire()
    {
        if (currentAmmo <= 0 || isReloading)
        {
            Debug.Log("Out of ammo! Reload your gun.");
            return;
        }

        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle) * barrel.rotation;
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
        }

        currentAmmo--;
        Debug.Log("Fired! Current ammo: " + currentAmmo);
    }
    
    public void Reload()
    {
        StartCoroutine(reloadCoroutine());
    }
}
