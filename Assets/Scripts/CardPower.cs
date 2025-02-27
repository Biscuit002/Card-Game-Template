using UnityEngine;
using TMPro;

public class CardPower : MonoBehaviour
{
    [SerializeField] private string cardType = "Attack";
    [SerializeField] private int power = 10;
    [SerializeField] private TextMeshPro powerText; // Changed to TextMeshPro to match your setup

    private void Start()
    {
        if (powerText == null)
        {
            powerText = GetComponentInChildren<TextMeshPro>();
        }
        UpdatePowerDisplay();
        Debug.Log($"Card initialized: Type={cardType}, Power={power}");
    }

    public string GetCardType()
    {
        return cardType;
    }

    public int GetPower()
    {
        return power;
    }

    public void SetPower(int newPower)
    {
        power = newPower;
        UpdatePowerDisplay();
        Debug.Log($"Card {cardType} power updated to: {power}");
    }

    private void UpdatePowerDisplay()
    {
        if (powerText != null)
        {
            powerText.text = power.ToString();
            Debug.Log($"Updated display text to: {power}");
        }
        else
        {
            Debug.LogError($"Power Text component missing on {gameObject.name}!");
        }
    }
}