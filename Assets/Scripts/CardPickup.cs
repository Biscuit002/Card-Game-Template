using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

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
    
    private float smallestDistance;
    public GameObject closestTarget; // Track the closest SnapTarget

    private DiscardSlot discardSlot;

    void Start()
    {
        isMouseDragging = false;
        SnapTargets = GameObject.FindGameObjectsWithTag("SnapTarget");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        originalSortingOrder = canvas.sortingOrder;
        cardPower = GetComponent<CardPower>();
        displayPower = FindObjectOfType<DisplayPower>();
        gameManager = FindObjectOfType<GameManager>();
        
        discardSlot = FindObjectOfType<DiscardSlot>();
    }

    void Update()
    {
        powerValue = cardPower.GetPower();
        
        if (isMouseDragging)
        {
            Vector3 mousePosition = Input.mousePosition;
            float distance = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
            mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, distance));
            mousePosition.z = 0;

            Vector3 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distance));
            Vector3 upperRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, distance));

            float clampedX = Mathf.Clamp(mousePosition.x, lowerLeft.x, upperRight.x);
            float clampedY = Mathf.Clamp(mousePosition.y, lowerLeft.y, upperRight.y);
            transform.position = new Vector3(clampedX, clampedY, 0);
        }
        else
        {
            checkClosestTarget();
            CheckDiscardSlot();
        }
    }

    void checkClosestTarget()
    {
        smallestDistance = float.MaxValue;
        closestTarget = null;

        foreach (var target in SnapTargets)
        {
            float currentDistance = Vector2.Distance(transform.position, target.transform.position);
            if (currentDistance < smallestDistance)
            {
                smallestDistance = currentDistance;
                closestTarget = target;
            }
        }

        if (closestTarget != null && smallestDistance < 1f)
        {
            inTarget = true;
            transform.position = Vector3.MoveTowards(transform.position, closestTarget.transform.position, 0.8f);

            // Update the specific SnapTarget with this card's power value
            gameManager.UpdateSnapTargetPower(closestTarget, powerValue);

            // Now retrieve and print the power value of the card in the SnapTarget slot
            int snapTargetPower = GetSnapTargetPower(closestTarget);
        }
        else
        {
            inTarget = false;
        }
    }

    private void CheckDiscardSlot()
    {
        if (discardSlot != null)
        {
            Vector2 discardPos = discardSlot.transform.position;
            float distance = Vector2.Distance(transform.position, discardPos);
            
            if (distance < 1f)
            {
                discardSlot.OnCardPlaced(gameObject);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isMouseDragging = true;
        //canvas.sortingOrder = 100;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMouseDragging = false;
       // canvas.sortingOrder = originalSortingOrder;
    }

    // Retrieve the power value of a SnapTarget from the GameManager
    public int GetSnapTargetPower(GameObject target)
    {
        if (gameManager.snapTargetPowers.ContainsKey(target))
        {
            return gameManager.snapTargetPowers[target];
        }
        else
        {
            Debug.LogWarning("SnapTarget not found in snapTargetPowers.");
            return 0; // Or any default value you want
        }
    }
}
