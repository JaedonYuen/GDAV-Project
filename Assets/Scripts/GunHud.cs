using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GunHud : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public HandSystem playerHand;
    public TextMeshProUGUI ammoCountText;
    void Start()
    {
        
    }

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
            }
            else
            {
                // If the item in hand is not a gun, clear the ammo count text
                ammoCountText.text = "--";
            }
        }
        else
        {
            // If no gun is equipped, clear the ammo count text
            ammoCountText.text = "--";
        }
    }
}
