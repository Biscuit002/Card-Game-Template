using UnityEngine;

[System.Serializable]
public class CardPower : MonoBehaviour
{
    [SerializeField] private string cardType = "Attack"; // Can be "Attack", "Defense", "Item", "Passive"
    [SerializeField] [Tooltip("The power value of this card")]
    [Range(0, 100)] // Adjust the range as needed for your game
    public int power = 0;
    
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
        power = Mathf.Clamp(newPower, 0, 100); // Ensure power stays within valid range
    }
}