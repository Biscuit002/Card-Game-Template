using UnityEngine;
using TMPro;

public class CardSlot : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 0.5f;
    [SerializeField] private TextMeshPro powerText;
    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private bool isDefenseSlot = false;

    void Awake()
    {
        // Set this object to layer 6 (CardSlot)
        gameObject.layer = 6;
        // Set cardLayer to detect only layer 3 (Card)
        cardLayer = 1 << 3;

        // Set both SnapTarget and combat role tags
        gameObject.tag = isDefenseSlot ? "DefenseSlot" : "SnapTarget";
        
        // Add combat role tags as secondary tags using GameObject.AddComponent
        GameObject combatTagObject = new GameObject();
        combatTagObject.transform.parent = transform;
        combatTagObject.name = isDefenseSlot ? "DefenseSlot" : "AttackSlot";
        combatTagObject.tag = isDefenseSlot ? "DefenseSlot" : "AttackSlot";
    }

    void Update()
    {
        CheckForCard();
        
        // Update power display every frame in case it changes
        if (currentCardPower != null)
        {
            UpdatePowerDisplay(currentPower);
        }
    }

    private CardDragHandler currentCard;
    private CardPower currentCardPower;
    private int currentPower;

    public bool HasCard => transform.childCount > 0;

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
        if (HasCard)
        {
            CardPower cardPower = GetComponentInChildren<CardPower>();
            if (cardPower != null)
            {
                int power = cardPower.GetPower();
                Debug.Log($"Slot {gameObject.name} current power: {power}");
                return power;
            }
            Debug.LogError($"Card in slot {gameObject.name} has no CardPower component!");
        }
        return 0;
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

    public void TakeDamage(int damage)
    {
        if (HasCard)
        {
            CardPower cardPower = GetComponentInChildren<CardPower>();
            if (cardPower != null)
            {
                int currentPower = cardPower.GetPower();
                Debug.Log($"Slot {gameObject.name} before damage: {currentPower}");
                cardPower.SetPower(currentPower - damage);
                Debug.Log($"Slot {gameObject.name} after damage: {cardPower.GetPower()}");
            }
        }
    }

    public void DestroyCard()
    {
        if (HasCard)
        {
            Debug.Log($"Destroying card in slot {gameObject.name}");
            Destroy(transform.GetChild(0).gameObject);
        }
    }
}