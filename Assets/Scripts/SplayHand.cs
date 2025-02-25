using UnityEngine;
using System.Collections.Generic;

public class SplayHand : MonoBehaviour
{
    public List<GameObject> cards;
    public float radius = 200f; // Radius of the arc
    public float hoverScaleFactor = 1.2f;
    public float lerpSpeed = 5.0f;

    // These define the rotation limits for extreme cards.
    // Leftmost card (at -90° arc angle) will get +maxRotation,
    // Rightmost (at 90° arc angle) will get -maxRotation.
    public float maxRotation = 30f; 

    private Vector3 defaultScale;

    void Start()
    {
        if (cards.Count > 0)
        {
            defaultScale = cards[0].transform.localScale;
        }
        ArrangeCards();
    }

    void Update()
    {
        foreach (GameObject card in cards)
        {
            Vector3 targetScale = IsMouseOver(card) ? defaultScale * hoverScaleFactor : defaultScale;
            card.transform.localScale = Vector3.Lerp(card.transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
        }
    }

    // In SplayHand.cs
    public void AddCard(GameObject card)
    {
        // Add card to hand and arrange
        cards.Add(card);
        // Your existing arrangement logic
    }

    public void RemoveCard(GameObject card)
    {
        cards.Remove(card);
        ArrangeCards();
    }

    void ArrangeCards()
    {
        if (cards.Count == 0) return;

        // t goes from 0 to 1 across our card list.
        for (int i = 0; i < cards.Count; i++)
        {
            float t = (float)i / (cards.Count - 1);
            // Interpolate an arc angle from –90° to 90°
            float arcAngle = Mathf.Lerp(-90f, 90f, t);
            float rad = arcAngle * Mathf.Deg2Rad;
            // For an upward arc:
            float xPos = radius * Mathf.Sin(rad);
            float yPos = radius * Mathf.Cos(rad);
            cards[i].transform.localPosition = new Vector3(xPos, yPos, 0);

            // Rotation: leftmost card gets +maxRotation, center 0, rightmost -maxRotation.
            float rotation = Mathf.Lerp(maxRotation, -maxRotation, t);
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, rotation);
            cards[i].transform.localScale = defaultScale;
        }
    }

    bool IsMouseOver(GameObject card)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = card.GetComponent<Collider2D>();
        return collider.OverlapPoint(mousePos);
    }
}