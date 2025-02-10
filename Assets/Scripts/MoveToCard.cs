using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class MoveToCard : MonoBehaviour
{
    private CardPickup cardPickup;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardPickup = FindObjectOfType<CardPickup>();
        offset = transform.position - cardPickup.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cardPickup.transform.position + offset;
    }
}
