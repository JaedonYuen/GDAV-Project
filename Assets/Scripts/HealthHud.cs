using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthHud : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public PlayerHealthSystem playerHealthSystem;
    public Slider healthSlider;

    public float lerpSpeed = 5f; // Speed of the slider lerp
    void Start()
    {
        if (playerHealthSystem != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, (float)playerHealthSystem.currentHealth / playerHealthSystem.maxHealth, Time.deltaTime * lerpSpeed);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealthSystem != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, (float)playerHealthSystem.currentHealth / playerHealthSystem.maxHealth, Time.deltaTime * lerpSpeed);
        }
    }
}
