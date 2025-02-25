using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextWaveButtonController : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private Vector3 normalPosition; // Normal, clickable position
    [SerializeField] private Vector3 highPosition;   // High, unclickable position
    
    [Header("References")]
    [SerializeField] private Button nextWaveButton;  // Reference to the button component
    [SerializeField] private Transform cardSlotsFirstRow; // Reference to the first row of card slots
    [SerializeField] private Transform enemyParent;  // Reference to the parent containing all enemies
    
    [Header("Settings")]
    [SerializeField] private float lerpSpeed = 5f;   // Speed of the lerp movement
    
    private Vector3 targetPosition;
    private bool isButtonHigh = false;
    
    private void Start()
    {
        // If not assigned in inspector, try to get button component
        if (nextWaveButton == null)
            nextWaveButton = GetComponent<Button>();
            
        // Initialize normal position if not set in inspector
        if (normalPosition == Vector3.zero)
            normalPosition = transform.position;
            
        // Set initial target to normal position
        targetPosition = normalPosition;
    }
    
    private void Update()
    {
        // Check if any enemies are below the first row
        bool enemiesBelowFirstRow = CheckEnemiesBelowFirstRow();
        
        // Update target position based on enemy positions
        if (enemiesBelowFirstRow && !isButtonHigh)
        {
            isButtonHigh = true;
            targetPosition = highPosition;
            nextWaveButton.interactable = false;
        }
        else if (!enemiesBelowFirstRow && isButtonHigh)
        {
            isButtonHigh = false;
            targetPosition = normalPosition;
            nextWaveButton.interactable = true;
        }
        
        // Lerp to target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * lerpSpeed);
    }
    
    private bool CheckEnemiesBelowFirstRow()
    {
        if (cardSlotsFirstRow == null || enemyParent == null)
            return false;
            
        float firstRowY = cardSlotsFirstRow.position.y;
        
        // Check each enemy position
        foreach (Transform enemy in enemyParent)
        {
            if (enemy.position.y < firstRowY)
                return true;
        }
        
        return false;
    }
} 