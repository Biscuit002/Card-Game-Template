using UnityEngine;

public class EnemyPower : MonoBehaviour
{
    [SerializeField] private int minPower = 10;
    [SerializeField] private int maxPowerBase = 30;
    
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
    }

    public void TakeDamage(int damage)
    {
        power -= damage;
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