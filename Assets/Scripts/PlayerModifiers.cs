using UnityEngine;

public class PlayerModifiers : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // very basic version of the "game modifier system" that will be used to modify player stats
    public float currentSpeedModifier = 1f;
    public float currentDamageModifier = 1f;
    public float currentHealthModifier = 1f;

    public float currentEnemySpeedModifier = 1f;
    public float currentEnemyDamageModifier = 1f;
    public float currentEnemyHealthModifier = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
