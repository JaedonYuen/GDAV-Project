using UnityEngine;
using TMPro;

public class WaveDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Displays the current wave level
    public TextMeshProUGUI waveText;
    public GameSystem gameSystem;
    void Start()
    {
        waveText.text = "Wave 0";
    }

    // Update is called once per frame
    void Update()
    {
        waveText.text = "Wave " + gameSystem.currentWaveLevel;
    }
}
