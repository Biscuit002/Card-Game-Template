using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isMouseDragging;
    public GameObject SnapTarget;

    public CardPower cardPower;
    public int powerValue;
    
    private Canvas canvas;
    private int originalSortingOrder;

    void Start()
    {
        isMouseDragging = false;
        SnapTarget = GameObject.Find("SnapTarget");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        originalSortingOrder = canvas.sortingOrder;
        cardPower = GetComponent<CardPower>();
    }

    void Update()
    {
        if (isMouseDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            
            // Calculate distance from camera to the card along z-axis.
            float distance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            // Convert mouse position to world coordinates at the card's distance.
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distance));
            mousePosition.z = 0; // Lock z
            
            // Calculate screen boundaries in world space at the card's depth.
            Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distance));
            Vector3 upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));
            
            // Clamp the mouse position.
            float clampedX = Mathf.Clamp(mousePosition.x, lowerLeft.x, upperRight.x);
            float clampedY = Mathf.Clamp(mousePosition.y, lowerLeft.y, upperRight.y);
            Vector3 clampedPosition = new Vector3(clampedX, clampedY, 0);
            
            // Move the object toward the clamped mouse position.
            transform.position = Vector3.MoveTowards(transform.position, clampedPosition, 0.5f);
        }
        if (!isMouseDragging)
        {
            // Snap the card back to its original position.
            if (Vector2.Distance(transform.position, SnapTarget.transform.position) < 2) 
            {
                transform.position = Vector3.MoveTowards(transform.position, SnapTarget.transform.position, 0.5f);
                powerValue = cardPower.power;
                print("Power value: " + powerValue);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        // If the card collides with another card, snap back to the original position.
        if (other.gameObject.CompareTag("Snaptarget"))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, 0), 0.9f);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDragging = true;
        canvas.sortingOrder = 100;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDragging = false;
        canvas.sortingOrder = originalSortingOrder;
    }
}