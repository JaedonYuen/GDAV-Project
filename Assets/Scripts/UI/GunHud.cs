using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class GunHud : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Displays the ammo count of the gun in the hand
    public HandSystem playerHand;
    public TextMeshProUGUI ammoCountText;
    public Slider ammoSlider;
    public float lerpSpeed = 5f; // Speed of the slider lerp
    

    // Update is called once per frame
    void Update()
    {
        if (playerHand != null && playerHand.itemInhand != null)
        {
            GunSystem gun = playerHand.itemInhand.GetComponent<GunSystem>();
            if (gun != null)
            {
                // Update the ammo count text with the current ammo
                ammoCountText.text = gun.currentAmmo.ToString();
                // Update the ammo slider value smoothly
                ammoSlider.value = Mathf.Lerp(ammoSlider.value, (float)gun.currentAmmo / gun.maxAmmo, Time.deltaTime * lerpSpeed);
            }
            else
            {
                // If the item in hand is not a gun, clear the ammo count text
                ammoCountText.text = "--";
                ammoSlider.value = 0f; // Reset the slider
            }
        }
        else
        {
            // If no gun is equipped, clear the ammo count text
            ammoCountText.text = "--";
            ammoSlider.value = 0f; // Reset the slider
        }
    }
}
