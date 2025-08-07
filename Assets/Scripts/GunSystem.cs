using System.Collections;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform barrel;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; 
    public int bulletsPerShot = 1; 
    public bool isAutomatic = false; // If true, gun will fire continuously while the trigger is held
    public float spreadAngle = 0f; 
    public int maxAmmo = 30; 
    public float reloadTime = 1f; 
    public float damage = 10f;

    public bool belongsToPlayer = false; 
    
    [SerializeField] private int _currentAmmo = 0;
    public int currentAmmo
    {
        get { return _currentAmmo; }
        private set
        {
            _currentAmmo = Mathf.Clamp(value, 0, maxAmmo);
        }
    }
    private bool isReloading = false;
    private bool isFiring = false;
    private bool isTriggered = false; // Only applies when the gun is automatic
    private Coroutine autoFireCoroutine = null; 
    void Start()
    {
        _currentAmmo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator reloadCoroutine()
    {
        isReloading = true;
        //Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        _currentAmmo = maxAmmo;
        isReloading = false;
        //Debug.Log("Reload complete! Current ammo: " + _currentAmmo);
    }

    public void Fire(bool buttonState)
    {   
        //Debug.Log("Fire called with buttonState: " + buttonState);
        if (currentAmmo <= 0 || isReloading)
        {
            //Debug.Log("Out of ammo! Reloading your gun.");
            Reload();
            return;
        }

        if (isAutomatic)
        {
            isTriggered = buttonState;
            if (buttonState && autoFireCoroutine == null)
            {
                autoFireCoroutine = StartCoroutine(AutoFire());
            }
            else if (!buttonState && autoFireCoroutine != null)
            {
                StopCoroutine(autoFireCoroutine);
                autoFireCoroutine = null;
                isFiring = false; // Stop firing when the trigger is released
            }
        }
        else
        {
            if (buttonState && !isFiring)
            {
                StartCoroutine(fireBullet());
            }
        }
    }

    private IEnumerator AutoFire()
    {
        while (currentAmmo > 0 && !isReloading && isTriggered)
        {
            if (!isFiring)
            {
                StartCoroutine(fireBullet());
                _currentAmmo--;
                //Debug.Log("Fired! Current ammo: " + _currentAmmo);
            }
            yield return new WaitForSeconds(fireRate);
        }
        autoFireCoroutine = null;
    }

    IEnumerator fireBullet()
    {
        isFiring = true;
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle) * barrel.rotation;
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
            float modifiedDamage = damage;
            float totalDamageModifier = 1f;
            if (belongsToPlayer)
            {
                totalDamageModifier  = GetComponent<Modifiers>()?.GetModValuesForAllTypesEquiped("damage") ?? 1f;
            }
            else
            {
                totalDamageModifier = GetComponent<Modifiers>()?.GetModValuesForAllTypesEquiped("enemyDamage") ?? 1f;
            }

            bullet.GetComponent<BulletSystem>().damage = modifiedDamage;
            Destroy(bullet, 30f); // Destroy bullet after 30 seconds to prevent memory leaks
        }
        if (!isAutomatic) // Only decrement for semi-auto here
        {
            _currentAmmo--;
            //Debug.Log("Fired! Current ammo: " + _currentAmmo);
        }
        yield return new WaitForSeconds(fireRate); 
        isFiring = false;
    }

    public void Reload()
    {
        StartCoroutine(reloadCoroutine());
    }
}
