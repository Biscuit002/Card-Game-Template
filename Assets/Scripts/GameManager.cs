using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CardPickup[] cardObjects;
    private CardPickup cardPickup;
    public int powerSum;

    /*public static GameManager gm;
    public List<Card> deck = new List<Card>();
    public List<Card> player_deck = new List<Card>();
    public List<Card> ai_deck = new List<Card>();
    public List<Card> player_hand = new List<Card>();
    public List<Card> ai_hand = new List<Card>();
    public List<Card> discard_pile = new List<Card>();

    private void Awake()
    {
        if (gm != null && gm != this)
        {
            Destroy(gameObject);
        }
        else
        {
            gm = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }*/
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        powerSum = 0;
        cardObjects = FindObjectsOfType<CardPickup>();
        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardPickup = cardObjects[i];
            if (cardObjects[i].inTarget)
            {
                powerSum += cardObjects[i].powerValue;
            }
        }
        if (cardPickup != null)
        {
            cardPickup.displayPower.powerText.text = "POWER: " + powerSum;
        }
    }

    void Deal()
    {

    }

    void Shuffle()
    {

    }

    void AI_Turn()
    {

    }

}
