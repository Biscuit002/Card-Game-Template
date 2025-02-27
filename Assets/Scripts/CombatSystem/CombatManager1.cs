using UnityEngine;

public class CombatManager1 : MonoBehaviour
{
    private CardPickup cardPickup;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardPickup = FindObjectOfType<CardPickup>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (cardPickup.inTarget)
        {
            Debug.Log("Card is in target");
        }
    }
}
