using UnityEngine;
using System.Collections.Generic;

public class SplayHand : MonoBehaviour
{
    public List<GameObject> cards;
    public float curveHeight = 50f;
    public float cardSpacing = 1.0f; // Reduced spacing
    public float hoverScaleFactor = 1.2f;
    public float lerpSpeed = 5.0f;
    public float yPos = -50f; // Assignable y position in the editor
    public float yMultiplier = 2.0f; // Multiplier for y position adjustment

    private Vector3 defaultScale;

    void Start()
    {
        if (cards.Count > 0)
        {
            defaultScale = cards[0].transform.localScale;
        }
        ArrangeCards();
    }

    void ArrangeCards()
    {
        float totalWidth = (cards.Count - 1) * cardSpacing;
        float startX = -totalWidth / 2;
        int middleIndex = cards.Count / 2;

        for (int i = 0; i < cards.Count; i++)
        {
            float xPos = startX + i * cardSpacing;
            float yOffset = Mathf.Abs(middleIndex - i) * yMultiplier;
            float adjustedYPos = yPos - yOffset;
            cards[i].transform.localPosition = new Vector3(xPos, adjustedYPos, 0);

            float angle = (middleIndex - i) * 10; // Reverse incremental rotation
            cards[i].transform.localRotation = Quaternion.Euler(0, 0, angle); // Rotate on z-axis
            cards[i].transform.localScale = defaultScale; // Set to default scale
        }
    }

    void Update()
    {
        foreach (GameObject card in cards)
        {
            Vector3 targetScale = IsMouseOver(card) ? defaultScale * hoverScaleFactor : defaultScale;
            card.transform.localScale = Vector3.Lerp(card.transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
        }
    }

    bool IsMouseOver(GameObject card)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = card.GetComponent<Collider2D>();
        return collider.OverlapPoint(mousePos);
    }
}