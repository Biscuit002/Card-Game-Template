using UnityEngine;
using UnityEngine.EventSystems;

public class CardPickup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isMouseDragging;

    void Start()
    {
        isMouseDragging = false;
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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDragging = false;
    }
}