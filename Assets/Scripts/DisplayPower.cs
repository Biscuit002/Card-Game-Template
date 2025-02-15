using UnityEngine;
using UnityEngine.UI;

public class DisplayPower : MonoBehaviour
{
    private Text powerText;
    public CardPickup cardPickup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        cardPickup = gameObject.Find(cardPickup.activeCard.name);
        if (cardPickup != null)
        {
            //powerText.text = "Power: " + cardPickup.powerValue;
            //print(cardPickup.activeCard.name + " has a power value of " + cardPickup.powerValue);
            print(cardPickup);
        }
    }
}