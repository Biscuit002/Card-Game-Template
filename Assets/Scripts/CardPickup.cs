using UnityEngine;
using UnityEngine.EventSystems;

public class CardPickup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isMouseDragging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMouseDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseDragging)
        {
            print("mouse dragging");
            Vector3 mousePosition = Input.mousePosition;

            // Convert screen position to world position using the camera
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

            // Ensure z is zero to keep the object on the same plane
            mousePosition.z = 0;

            // Move the object towards the mouse position
            transform.position = Vector3.MoveTowards(transform.position, mousePosition, 0.5f);
        }
    }

    // Called when the mouse is clicked on the object
    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDragging = true;
    }

    // Called when the mouse button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDragging = false;
    }
}
