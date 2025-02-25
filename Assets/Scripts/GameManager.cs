using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private Dictionary<GameObject, int> snapTargetPowers = new Dictionary<GameObject, int>();

    public void UpdateSnapTargetPower(GameObject snapTarget, int powerValue)
    {
        if (snapTarget != null)
        {
            // Update or add the power value for the specific SnapTarget
            snapTargetPowers[snapTarget] = powerValue;
            
            // Find the TextMeshPro component in this SnapTarget and update the text
            TextMeshProUGUI textComponent = snapTarget.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "POWER: " + powerValue.ToString();
            }
        }
    }
}
