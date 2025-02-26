using UnityEngine;
using TMPro;

public class EnemyPower : MonoBehaviour
{
    [SerializeField] private int minPower = 10;
    [SerializeField] private int maxPowerBase = 30;
    [SerializeField] private TextMeshPro powerText; // Changed back to TextMeshPro for 3D text
    
    private int power;
    private bool isProcessed = false;
    
    public bool IsProcessed => isProcessed;
    public bool IsDead => power <= 0;

    void Start()
    {
        // Calculate max power based on wave number
        int waveMultiplier = FindObjectOfType<EnemyScript>().currentWave;
        int adjustedMaxPower = maxPowerBase + (waveMultiplier * 5);
        
        // Set random power value
        power = Random.Range(minPower, adjustedMaxPower);
        
        // Update the power display
        UpdatePowerDisplay();
    }

    private void UpdatePowerDisplay()
    {
        if (powerText != null)
        {
            powerText.text = power.ToString();
        }
    }

    public void TakeDamage(int damage)
    {
        power -= damage;
        UpdatePowerDisplay();
    }

    public int DealDamage(int defensePower)
    {
        int remainingDamage = Mathf.Max(0, power - defensePower);
        return remainingDamage;
    }

    public int GetPower()
    {
        return power;
    }

    public void MarkAsProcessed()
    {
        isProcessed = true;
    }
} 