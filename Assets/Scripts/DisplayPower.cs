using UnityEngine;
using UnityEngine.UI;

public class DisplayPower : MonoBehaviour
{
    private Text powerText;
    public CardPower cardPower;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        powerText = GetComponent<Text>();
        cardPower = FindObjectOfType<CardPower>();
    }

    // Update is called once per frame
    void Update()
    {
        //powerText.text = "Power: " + cardPower.power;
    }
}