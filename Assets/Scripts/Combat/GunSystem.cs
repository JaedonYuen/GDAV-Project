using System.Collections;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform barrel; // Where the bullets come from
    public GameObject bulletPrefab; // Bullet itself
    public float bulletSpeed = 20f;
    public float fireRate = 0.5f; 
    public int bulletsPerShot = 1; 
    public bool isAutomatic = false; // If true, gun will fire continuously while the trigger is held
    public float spreadAngle = 0f; 
    public int maxAmmo = 30; 
    private int _maxAmmo = 30; // This is the base max ammo, can be modified by modifiers
    public float reloadTime = 1f; 
    public float damage = 10f;
    public AudioClip fireSound; // Sound to play when firing
    public AudioClip reloadSound; // Sound to play when reloading

    public bool belongsToPlayer = false; // This is a cruicial boolean. Since the Gun system is universal, it is utilised by both players and enemies. However, this is required so we know to only boost the mag size of the player if the player has a mag mod installed during their run.
    
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
        _maxAmmo = maxAmmo;
        _currentAmmo = _maxAmmo;
        
    }

    // Update is called once per frame
    void Update()
    {
        Modifiers modifiers = FindAnyObjectByType<Modifiers>();
        //Debug.Log(modifiers);
        if (modifiers != null)
        {
            float mod = modifiers.GetModValuesForAllTypesEquiped("ammo");
            //Debug.Log(mod);
            _maxAmmo = (int)(mod * maxAmmo); // Apply ammo modifier
        }
    }

    IEnumerator reloadCoroutine() // Relaod the gun
    {
        isReloading = true;
        
        //Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);
        _currentAmmo = _maxAmmo;
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
                autoFireCoroutine = StartCoroutine(AutoFire()); // We have to know what the firing coroutine is so we can turn it off when the trigger is released.
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
        if (fireSound != null)
        {
            //vary the sound volume and pitch slightly for each shot
            AudioSource.PlayClipAtPoint(fireSound, barrel.position, Random.Range(0.8f, 1.2f));



        }
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle) * barrel.rotation; // Give the bullet a random spread angle.
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = bulletRotation * Vector2.right * bulletSpeed;
            float modifiedDamage = damage;
            // btw, damage used to be calculated manually, as in hardcoded via the gun. Bullets now handle it based on the health system that the bullet interacts with.
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
