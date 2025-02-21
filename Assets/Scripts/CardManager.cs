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
        if(deck.Count > 0)
        {
            GameObject newCard = Instantiate(cardPrefab, handArea);
            
            // Ensure card is visible
            RectTransform rectTransform = newCard.GetComponent<RectTransform>();
            rectTransform.sizeDelta = cardSize;
            
            Image cardImage = newCard.GetComponent<Image>();
            if(cardImage != null)
            {
                cardImage.color = Color.white; // Set to pure white for full visibility
            }
            
            hand.Add(newCard);
            deck.RemoveAt(deck.Count - 1);
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