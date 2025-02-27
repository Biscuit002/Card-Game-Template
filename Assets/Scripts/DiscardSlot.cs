using UnityEngine;
using UnityEngine.EventSystems;
using CombatSystem;  // Add this if needed

public class DiscardSlot : MonoBehaviour
{
    [Header("Effect Settings")]
    [SerializeField] private float healthRestoreAmount = 10f;
    [SerializeField] private GameManager gameManager; // Reference to your GameManager for health/stats

    void Start()
    {
        Debug.Log("DiscardSlot started");
        // If not assigned in inspector, try to find GameManager
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    // Called when a card enters this slot (you'll need to modify CardPickup to call this)
    public void OnCardPlaced(GameObject card)
    {
        // Get the card's power component
        CardPower cardPower = card.GetComponent<CardPower>();
        
        if (cardPower != null)
        {
            // Get card value for effect scaling
            int powerValue = cardPower.GetPower();
            
            // Check card type and apply appropriate effect
            switch (cardPower.GetCardType())
            {
                case "Attack":
                    DiscardAttackCard(powerValue);
                    break;
                case "Defense":
                    DiscardDefenseCard(powerValue);
                    break;
                case "Item":
                    UseItem(powerValue);
                    break;
                case "Passive":
                    UsePassive(powerValue);
                    break;
            }
        }

        // Destroy the card after use
        Destroy(card);
    }

    private void DiscardAttackCard(int power)
    {
        // Maybe give small amount of health or other resource
        if (gameManager != null)
        {
            gameManager.AddHealth(power * 0.1f); // 10% of power as health
        }
    }

    private void DiscardDefenseCard(int power)
    {
        // Maybe give armor or temporary shield
        if (gameManager != null)
        {
            gameManager.AddTemporaryShield(power * 0.2f); // 20% of power as shield
        }
    }

    private void UseItem(int power)
    {
        // Items give full healing or other strong effects
        if (gameManager != null)
        {
            gameManager.AddHealth(healthRestoreAmount);
            // Could add other effects based on item power
        }
    }

    private void UsePassive(int power)
    {
        // Maybe give temporary buff or special effect
        if (gameManager != null)
        {
            gameManager.AddTemporaryBuff(power, 3f); // Power buff for 3 seconds
        }
    }
} 