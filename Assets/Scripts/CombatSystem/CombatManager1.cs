using UnityEngine;

public class CombatManager1 : MonoBehaviour
{
    private CardPickup cardPickup;
    private GameManager gameManager;
    private int power;
    // Start is called once before the fi
    // first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardPickup = FindObjectOfType<CardPickup>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameManager = FindObjectOfType<GameManager>();
        cardPickup = FindObjectOfType<CardPickup>();
        if (gameManager != null && cardPickup != null)
        {
            power = gameManager.snapTargetPowers[cardPickup.gameObject];
            print(power);
        }


        /*float miniimumDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        gameManager = FindObjectOfType<GameManager>();
        for (int i = 0; i < gameManager.snapTargetPowers; i++)
        {
            //find closest target
            float distance = Vector3.Distance(cardPickup.transform.position, gameManager.snapTargetPowers[i].transform.position);
        }
        cardPickup = FindObjectOfType<CardPickup>();
        if (cardPickup.inTarget)
        {
            Debug.Log("Card is in target");
        }*/
    }
}
