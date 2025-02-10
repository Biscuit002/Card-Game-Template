using UnityEngine;

public class AddCardButton : MonoBehaviour
{
    // Reference to the SplayHand component that manages the hand.
    public SplayHand splayHand;
    // Prefab of the card to be instantiated.
    public GameObject cardPrefab;
    // Parent transform where the new card will be instantiated (should be the same as SplayHand's transform).
    public Transform cardParent;

    // This method should be hooked up to the UI Button's OnClick() event.
    public void OnAddCardPressed()
    {
        // Instantiate a new card as a child using local positioning.
        GameObject newCard = Instantiate(cardPrefab, cardParent, false);
        // Reset its local scale to match the prefab.
        newCard.transform.localScale = cardPrefab.transform.localScale;
        // Add the new card to the hand.
        splayHand.AddCard(newCard);
    }
}