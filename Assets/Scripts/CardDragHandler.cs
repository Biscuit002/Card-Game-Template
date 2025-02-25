using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform originalParent;
    private SplayHand splayHand;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        splayHand = GetComponentInParent<SplayHand>();
        StoreOriginalPosition();
    }

    private void StoreOriginalPosition()
    {
        originalPosition = transform.position;
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        StoreOriginalPosition();
        offset = transform.position - GetMouseWorldPosition();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        
        // Check if card is in any slot
        Collider2D[] slots = Physics2D.OverlapCircleAll(transform.position, 0.5f, LayerMask.GetMask("CardSlot"));
        
        if (slots.Length == 0)
        {
            ReturnToHand();
        }
    }

    private void ReturnToHand()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        if (splayHand != null)
        {
            // Use AddCard instead of ArrangeCards
            splayHand.AddCard(gameObject);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -mainCamera.transform.position.z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }
}