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
    }

    private void UpdatePowerDisplay()
    {
        if (powerText != null)
        {
            powerText.text = power.ToString();
        }
        else
        {

        }
    }
}