using UnityEngine;

public class CombatManager1 : MonoBehaviour
{
    private CardPickup cardPickup;
    private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        cardPickup = FindObjectOfType<CardPickup>();
        gameManager = FindObjectOfType<GameManager>();

        //GameObject currentSnapTarget = gameManager.snapTargetPowers[gameManager.];
        //print(currentSnapTarget);
        /*if (cardPickup.inTarget)
        {
            Debug.Log("Card is in target");
        }*/
    }
    void Combat()
    {
        Debug.Log("Combat");
    }
}
