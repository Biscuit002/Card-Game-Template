using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPickup : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isMouseDragging;
    public GameObject[] SnapTargets;
    public bool inTarget;

    public CardPower cardPower;
    public int powerValue;
    
    private Canvas canvas;
    private int originalSortingOrder;

    public DisplayPower displayPower;

    public GameManager gameManager;
    public  int snapTargetListValue;

    void Start()
    {
        isMouseDragging = false;
        SnapTargets = GameObject.FindGameObjectsWithTag("SnapTarget");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        originalSortingOrder = canvas.sortingOrder;
        cardPower = GetComponent<CardPower>();
        displayPower = FindObjectOfType<DisplayPower>();
        gameManager = FindObjectOfType<GameManager>();
        snapTargetListValue = 0;
    }

    void Update()
    {
        if (isMouseDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            
            // Calculated distance from camera to the card along z-axis.
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
            transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
        }
        if (!isMouseDragging)
        {
            checkClosestTarget();
            if (Vector2.Distance(transform.position, SnapTargets[snapTargetListValue].transform.position) < 1) 
            {
                inTarget = true;
            } else 
            {
                inTarget = false;
            }
        }
    }
    public void checkClosestTarget()
    {
        for (int i = 0; i < SnapTargets.Length; i++)
        {
            if (Vector2.Distance(transform.position, SnapTargets[i].transform.position) < 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, SnapTargets[i].transform.position, 0.8f);
                displayPower.powerText.text = "POWER: " + gameManager.powerSum;
            }
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