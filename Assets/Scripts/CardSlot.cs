using UnityEngine;
using TMPro;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private TextMeshPro powerText;
    [SerializeField] [Tooltip("Set this to the Card layer only")] 
    private LayerMask cardLayer;

    private Card currentCard;
    private CardPower currentCardPower;
    private int currentPower;

    void Start()
    {
        if (powerText == null)
        {
            powerText = GetComponentInChildren<TextMeshPro>();
        }
        UpdatePowerDisplay(0);
    }

    void Update()
    {
        CheckForCard();
    }

    private void CheckForCard()
    {
        Collider2D cardCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, cardLayer);
        
        if (cardCollider != null)
        {
            CardPower cardPower = cardCollider.GetComponent<CardPower>();
            if (cardPower != null && cardPower != currentCardPower)
            {
                // New card with power detected
                currentCardPower = cardPower;
                currentCard = cardCollider.GetComponent<Card>();
                currentPower = cardPower.GetPower();
                UpdatePowerDisplay(currentPower);
            }
        }
        else if (currentCardPower != null)
        {
            // Card removed
            currentCardPower = null;
            currentCard = null;
            currentPower = 0;
            UpdatePowerDisplay(currentPower);
        }
    }

    private void UpdatePowerDisplay(int power)
    {
        if (powerText != null)
        {
            powerText.text = power.ToString();
        }
    }

    // Optional: visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}