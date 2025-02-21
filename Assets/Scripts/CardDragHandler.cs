using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform handAreaPanel;

    private Canvas canvas;
    private Transform originalParent;
    private Vector2 offset;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform, eventData.position, eventData.pressEventCamera, out offset);

        // Reparent to the canvas for proper overlaying.
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            transform.localPosition = localPoint - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Check if the card's screen position is inside the hand panel.
        if(handAreaPanel != null && RectTransformUtility.RectangleContainsScreenPoint(handAreaPanel, Input.mousePosition, eventData.pressEventCamera))
        {
            // Reparent the card back to the hand panel.
            transform.SetParent(handAreaPanel, true);
        }
        else
        {
            // Instead of detaching completely, reparent it to the canvas to keep it visible.
            transform.SetParent(canvas.transform, true);
        }
    }
}