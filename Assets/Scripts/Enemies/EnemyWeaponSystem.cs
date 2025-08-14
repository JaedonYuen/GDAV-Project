using UnityEngine;
using System.Collections;


public class EnemyWeaponSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform barrel; // where the bullets are shot from
    public GameObject bulletPrefab; // The bullet itself
    public float bulletSpeed = 20f; // Speed of said bullet
    public float fireRate = 0.5f; // Fire rate of the gun (NOT THE ENEMY, thats handled by the enemy AI script)
    public int bulletsPerShot = 1; // How many bullets per shot
    public bool isAutomatic = false; // If true, gun will fire continuously while the trigger is held
    public float spreadAngle = 0f; // Spread angle of bullets
    public int maxAmmo = 30; // Max ammunition
    public float reloadTime = 1f; // Reload time

    public float damage = 10f; // Damage dealt

    [SerializeField] private int _currentAmmo = 0; // Internal ammo counter
    public int currentAmmo // Readonly ammo counter
    {
        get { return _currentAmmo; }
        private set
        {
            _currentAmmo = Mathf.Clamp(value, 0, maxAmmo);
        }
    }
    private bool isReloading = false; // Indicates if the weapon is currently reloading
    private bool isFiring = false; // Indicates if the weapon is currently firing
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
        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        _currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reload complete! Current ammo: " + _currentAmmo);
    }

    public void Fire(bool buttonState)
    {
        Debug.Log("Fire called with buttonState: " + buttonState);
        if (currentAmmo <= 0 || isReloading)
        {
            Debug.Log("Out of ammo! Reloading your gun.");
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
                Debug.Log("Fired! Current ammo: " + _currentAmmo);
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
            bullet.GetComponent<BulletSystem>().damage = damage;
        }
        if (!isAutomatic) // Only decrement for semi-auto here
        {
            _currentAmmo--;
            Debug.Log("Fired! Current ammo: " + _currentAmmo);
        }
        yield return new WaitForSeconds(fireRate);
        isFiring = false;
    }

    public void Reload()
    {
        StartCoroutine(reloadCoroutine());
    }
}
