using UnityEngine;
using TMPro;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private TextMeshPro powerText;
    [SerializeField] private LayerMask cardLayer;

    void Awake()
    {
        // Set this object to layer 6 (CardSlot)
        gameObject.layer = 6;
        // Set cardLayer to detect only layer 3 (Card)
        cardLayer = 1 << 3;

        // Set both SnapTarget and combat role tags
        gameObject.tag = "SnapTarget";
        
        // Add combat role tags as secondary tags using GameObject.AddComponent
        GameObject combatTagObject = new GameObject();
        combatTagObject.transform.parent = transform;
        combatTagObject.name = isDefenseSlot ? "DefenseSlot" : "AttackSlot";
        combatTagObject.tag = isDefenseSlot ? "DefenseSlot" : "AttackSlot";
    }

    void Update()
    {
        CheckForCard();
    }

    private CardDragHandler currentCard;
    private CardPower currentCardPower;
    private int currentPower;

    public bool HasCard => currentCard != null;
    [SerializeField] private bool isDefenseSlot = false;

    private void CheckForCard()
    {
        Collider2D cardCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, cardLayer);
        
        if (cardCollider != null)
        {
            var dragHandler = cardCollider.GetComponent<CardDragHandler>();
            var cardPower = cardCollider.GetComponent<CardPower>();
            
            if (dragHandler != null && cardPower != null && cardPower != currentCardPower)
            {
                AttachCard(dragHandler, cardPower);
            }
        }
        else if (currentCard != null)
        {
            ClearSlot();
        }
    }

    private void AttachCard(CardDragHandler dragHandler, CardPower cardPower)
    {
        currentCard = dragHandler;
        currentCardPower = cardPower;
        currentPower = cardPower.GetPower();
        
        dragHandler.transform.SetParent(transform);
        dragHandler.transform.position = transform.position;
        UpdatePowerDisplay(currentPower);
    }

    private void ClearSlot()
    {
        currentCard = null;
        currentCardPower = null;
        currentPower = 0;
        UpdatePowerDisplay(currentPower);
    }

    private void UpdatePowerDisplay(int power)
    {
        if (powerText != null)
        {
            powerText.text = power.ToString();
        }
    }

    public int GetCurrentPower()
    {
        return currentPower;
    }

    public void RegeneratePower(float multiplier)
    {
        if (currentCardPower != null)
        {
            int regenerationAmount = Mathf.RoundToInt(currentCardPower.GetPower() * multiplier);
            currentPower = Mathf.Min(currentPower + regenerationAmount, currentCardPower.GetPower());
            UpdatePowerDisplay(currentPower);
        }
    }
}