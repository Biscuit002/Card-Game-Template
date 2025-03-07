using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Dictionary<GameObject, int> snapTargetPowers = new Dictionary<GameObject, int>();

    [Header("Player Stats")]
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private float temporaryShield;
    private float temporaryBuff;
    private float buffTimeRemaining;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Update buff timer
        if (buffTimeRemaining > 0)
        {
            buffTimeRemaining -= Time.deltaTime;
            if (buffTimeRemaining <= 0)
            {
                temporaryBuff = 0;
            }
        }
    }

    public void UpdateSnapTargetPower(GameObject snapTarget, int powerValue)
    {
        if (snapTarget != null)
        {
            // Update or add the power value for the specific SnapTarget
            snapTargetPowers[snapTarget] = powerValue;
            
            // Find the TextMeshPro component in this SnapTarget and update the text
            TextMeshProUGUI textComponent = snapTarget.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "POWER: " + powerValue.ToString();
            }
        }
    }

    public void AddHealth(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void AddTemporaryShield(float amount)
    {
        temporaryShield += amount;
    }

    public void AddTemporaryBuff(float amount, float duration)
    {
        temporaryBuff = amount;
        buffTimeRemaining = duration;
    }

    // Method to get current buff value (for other systems that need it)
    public float GetCurrentBuff()
    {
        return temporaryBuff;
    }

    // Method to get current shield value
    public float GetCurrentShield()
    {
        return temporaryShield;
    }

    // Method to consume shield when taking damage
    public float UseShield(float damage)
    {
        float remainingDamage = damage;
        if (temporaryShield > 0)
        {
            if (temporaryShield >= damage)
            {
                temporaryShield -= damage;
                return 0; // No damage gets through
            }
            else
            {
                remainingDamage -= temporaryShield;
                temporaryShield = 0;
            }
        }
        return remainingDamage; // Return remaining damage after shield
    }
}
