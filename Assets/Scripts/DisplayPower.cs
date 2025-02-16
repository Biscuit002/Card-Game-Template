using UnityEngine;
using TMPro;

public class DisplayPower : MonoBehaviour
{
    public TextMeshProUGUI powerText;
    public CardPickup cardPickup;

    void Start()
    {
        powerText = GetComponent<TextMeshProUGUI>();
    }  
}
