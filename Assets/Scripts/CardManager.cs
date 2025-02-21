using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handArea;
    [SerializeField] private Button drawButton;
    [SerializeField] private Button removeButton;
    
    private List<GameObject> deck = new List<GameObject>();
    private List<GameObject> hand = new List<GameObject>();

    [SerializeField] private Vector2 cardSize = new Vector2(200f, 300f); // Made larger for better visibility
    
    private void Start()
    {
        // Initialize deck with cards
        for (int i = 0; i < 6; i++)
        {
            // Deal card...
            hand.Add(cardPrefab);
            deck.RemoveAt(deck.Count - 1);
        }
        
        drawButton.onClick.AddListener(DrawCard);
        removeButton.onClick.AddListener(RemoveCard);
    }
        private void DrawCard()
    {
        // Desired hand size
        int desiredHandSize = 6;
        
        // Count how many cards are currently inside handArea
        int currentCount = handArea.childCount;
        
        // Calculate how many cards need to be added
        int missingCount = desiredHandSize - currentCount;
        
        // Only add cards for the missing slots, leaving moved-out cards intact.
        for (int i = 0; i < missingCount; i++)
        {
            if(deck.Count > 0)
            {
                GameObject newCard = Instantiate(cardPrefab, handArea);
                
                // Ensure card is visible by setting its RectTransform size
                RectTransform rectTransform = newCard.GetComponent<RectTransform>();
                rectTransform.sizeDelta = cardSize;
                
                // Optionally set the image color.
                Image cardImage = newCard.GetComponent<Image>();
                if(cardImage != null)
                {
                    cardImage.color = Color.white; // pure white for full visibility
                }
                
                // Add the new card to your hand list (if you're keeping track)
                hand.Add(newCard);
                // Remove the card from the deck.
                deck.RemoveAt(deck.Count - 1);
            }
        }
    }
    
    private void RemoveCard()
    {
        if(hand.Count > 0)
        {
            GameObject cardToRemove = hand[hand.Count - 1];
            hand.RemoveAt(hand.Count - 1);
            Destroy(cardToRemove);
        }
    }
}